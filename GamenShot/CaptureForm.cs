using System;
using System.Drawing;
using System.Windows.Forms;

namespace GamenShot
{
    public partial class CaptureForm : Form
    {
        /// <summary>
        /// 矩形選択対象のビットマップ
        /// </summary>
        private Bitmap screenBitmap = null;
        /// <summary>
        /// 矩形選択カウンタ
        /// </summary>
        private int actionCount = 0;
        /// <summary>
        /// 矩形選択開始位置
        /// </summary>
        private Point startPoint = new Point();
        /// <summary>
        /// 前回描画用
        /// </summary>
        private Rectangle currentRectangle = new Rectangle();
        /// <summary>
        /// 座標画面
        /// </summary>
        private CoordinateForm coordinateForm = null;
        /// <summary>
        /// 元のカーソル
        /// </summary>
        private Cursor originalCursor = null;
        /// <summary>
        /// 矩形選択したビットマップ
        /// </summary>
        public Bitmap CaptureBitmap { get; private set; } = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="screenBitmap">スクリーン画像</param>
        public CaptureForm(Bitmap screenBitmap)
        {
            InitializeComponent();
            this.screenBitmap = screenBitmap;
        }

        /// <summary>
        /// フォームが初めて表示される直前に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CaptureForm_Load(object sender, EventArgs e)
        {
            // 最大化
            this.WindowState = FormWindowState.Maximized;
            // 最前面表示
            this.TopMost = true;
            // 境界線なし
            this.FormBorderStyle = FormBorderStyle.None;
            // ダブルバッファ
            this.DoubleBuffered = true;
            // 画像
            this.pictureBox.Image = this.screenBitmap;
            // マウスカーソル変更
            this.originalCursor = this.Cursor;
            this.Cursor = Cursors.Cross;
            // 座標画面
            this.coordinateForm = new CoordinateForm(this.screenBitmap, Cursor.Position);
            this.coordinateForm.Show(this);
        }

        /// <summary>
        /// キー押下の際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            // ESCキー
            if (e.KeyCode == Keys.Escape)
            {
                if (this.actionCount == 0)
                {
                    this.CancelSelectRectangle();
                }
                else if (this.actionCount > 0)
                {
                    this.RevertSelectRectangle();
                }
            }
            // Enterキー
            else if(e.KeyCode == Keys.Enter)
            {
                Point mousePoint = Cursor.Position;
                if (this.actionCount == 0)
                {
                    this.StartSelectRectangle(mousePoint);
                }
                else if (this.actionCount == 1)
                {
                    this.EndSelectRectangle(mousePoint);
                }
            }
            // 上下左右移動
            else if(this.actionCount == 0 || this.actionCount == 1)
            {
                Point mousePoint = Cursor.Position;
                if (e.KeyCode == Keys.Up)
                {
                    mousePoint.Y += -1;
                }
                else if (e.KeyCode == Keys.Down)
                {
                    mousePoint.Y += 1;
                }
                else if (e.KeyCode == Keys.Left)
                {
                    mousePoint.X += -1;
                }
                else if (e.KeyCode == Keys.Right)
                {
                    mousePoint.X += 1;
                }
                Cursor.Position = mousePoint;
            }
        }

        /// <summary>
        /// マウスクリックの際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            // 左ボタン
            if (e.Button == MouseButtons.Left)
            {
                if (this.actionCount == 0)
                {
                    this.StartSelectRectangle(new Point(e.X, e.Y));
                }
                else if (this.actionCount == 1)
                {
                    this.EndSelectRectangle(new Point(e.X, e.Y));
                }
            }
            // 右ボタン
            else if (e.Button == MouseButtons.Right)
            {
                if (this.actionCount > 0)
                {
                    this.RevertSelectRectangle();
                }
            }
        }

        /// <summary>
        /// マウス移動の際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.actionCount == 1)
            {
                this.MoveSelectRectangle(new Point(e.X, e.Y));
            }
            this.coordinateForm.UpdateCoordinate(new Point(e.X, e.Y));
        }

        /// <summary>
        /// フォームを閉じる際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CaptureForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 座標画面を閉じる
            if(this.coordinateForm != null)
            {
                this.coordinateForm.Close();
            }
            // マウスカーソル戻す
            if (this.originalCursor != this.Cursor)
            {
                this.Cursor = this.originalCursor;
            }
        }

        /// <summary>
        /// 矩形選択開始
        /// </summary>
        /// <param name="point">マウスカーソルの位置</param>
        private void StartSelectRectangle(Point point)
        {
            // 開始位置を初期化
            this.startPoint = point;
            this.currentRectangle = new Rectangle();
            // 行動カウンタ = 1
            this.actionCount = 1;
        }

        /// <summary>
        /// 矩形選択移動
        /// </summary>
        /// <param name="point">マウスカーソルの位置</param>
        private void MoveSelectRectangle(Point point)
        {
            // 前回描画したラバーバンドの消去
            if(!this.currentRectangle.IsEmpty)
            {
                this.DrawScreenRectangle(this.currentRectangle);
            }
            // 今回描画するラバーバンドの矩形計算
            this.currentRectangle = this.GetScreenRectangle(this.startPoint, point);
            // 今回描画するラバーバンドの描画
            this.DrawScreenRectangle(this.currentRectangle);
        }

        /// <summary>
        /// 矩形選択戻し
        /// </summary>
        private void RevertSelectRectangle()
        {
            // 前回描画したラバーバンドの消去
            if (!this.currentRectangle.IsEmpty)
            {
                this.DrawScreenRectangle(this.currentRectangle);
            }
            // 行動カウンタを減算
            this.actionCount--;
        }

        /// <summary>
        /// 矩形選択中止
        /// </summary>
        private void CancelSelectRectangle()
        {
            // フォームを閉じる
            this.Close();
        }

        /// <summary>
        /// 矩形選択終了
        /// </summary>
        /// <param name="point">マウスカーソルの位置</param>
        private void EndSelectRectangle(Point point)
        {
            Rectangle rectangle = this.GetClientRectangle(this.startPoint, point);
            this.CaptureBitmap = this.CaptureRegion(rectangle);
            this.Close();
        }

        /// <summary>
        /// 2点の座標から左上座標と右下座標を取得
        /// </summary>
        /// <param name="p1">座標1</param>
        /// <param name="p2">座標2</param>
        /// <returns>左上座標と右下座標のタプル</returns>
        private (Point, Point) GetRegion(Point p1, Point p2)
        {
            // 左上座標
            var upperLeftPoint = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            // 右下座標
            var bottomRightPoint = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
            // 左上座標と右下座標のタプル
            return (upperLeftPoint, bottomRightPoint);
        }

        /// <summary>
        /// スクリーン座標の矩形範囲を取得
        /// </summary>
        /// <param name="p1">座標1</param>
        /// <param name="p2">座標2</param>
        /// <returns>スクリーン座標の矩形範囲</returns>
        private Rectangle GetScreenRectangle(Point p1, Point p2)
        {
            (var sp1, var sp2) = this.GetRegion(this.PointToScreen(p1), this.PointToScreen(p2));
            return new Rectangle(sp1.X, sp1.Y, this.GetLength(sp1.X, sp2.X), this.GetLength(sp1.Y, sp2.Y));
        }

        /// <summary>
        /// クライアント座標の矩形範囲を取得
        /// </summary>
        /// <param name="p1">座標1</param>
        /// <param name="p2">座標2</param>
        /// <returns>クライアント座標の矩形範囲</returns>
        private Rectangle GetClientRectangle(Point p1, Point p2)
        {
            (var cp1, var cp2) = this.GetRegion(p1, p2);
            return new Rectangle(cp1.X, cp1.Y, this.GetLength(cp1.X, cp2.X), this.GetLength(cp1.Y, cp2.Y));
        }

        /// <summary>
        /// 開始と終了の差（絶対値）を取得
        /// </summary>
        /// <param name="start">開始</param>
        /// <param name="end">終了</param>
        /// <returns>差（絶対値）</returns>
        private int GetLength(int start, int end)
        {
            // 座標点から長さを取得するため、1ピクセル足す
            return Math.Abs(start - end) + 1;
        }

        /// <summary>
        /// ラバーバンドの描画
        /// </summary>
        /// <param name="rectangle">描画する矩形範囲</param>
        private void DrawScreenRectangle(Rectangle rectangle)
        {
            // HACK:NVIDIA GeForce GT 710 では、Refreshしないとラバーバンドが残る
            this.pictureBox.Refresh();
            // ラバーバンドの描画
            ControlPaint.DrawReversibleFrame(rectangle, Color.FromArgb(255,Color.Red), FrameStyle.Dashed);
        }

        /// <summary>
        /// 矩形範囲をビットマップとして切り出す
        /// </summary>
        /// <param name="rectangle">矩形範囲</param>
        /// <returns>切り出したビットマップ</returns>
        private Bitmap CaptureRegion(Rectangle rectangle)
        {
            // 画面全体のBitmapから指定した矩形範囲のBitmapを切り出す
            Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawImage(this.screenBitmap, 0, 0, rectangle, GraphicsUnit.Pixel);
            }
            return bmp;
        }
    }
}
