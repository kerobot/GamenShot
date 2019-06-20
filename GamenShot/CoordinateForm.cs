using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GamenShot
{
    public partial class CoordinateForm : Form
    {
        /// <summary>
        /// 親フォーム
        /// </summary>
        private CaptureForm captureForm = null;
        /// <summary>
        /// 矩形選択対象のビットマップ
        /// </summary>
        private Bitmap screenBitmap = null;
        /// <summary>
        /// マウスカーソルの初期位置
        /// </summary>
        private Point mousePoint = new Point();
        /// <summary>
        /// 元のカーソル
        /// </summary>
        private Cursor originalCursor = null;
        /// <summary>
        /// 拡大率
        /// </summary>
        private double rate = 2.0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="screenBitmap">矩形選択対象のビットマップ</param>
        /// <param name="mousePoiont">マウスカーソルの初期位置</param>
        public CoordinateForm(CaptureForm captureForm, Bitmap screenBitmap, Point mousePoiont)
        {
            // コンポーネントの初期化
            InitializeComponent();
            // マウスイベントハンドラの初期化
            this.InitializeMouseEventHandler(this);
            // 親フォーム
            this.captureForm = captureForm;
            // 矩形選択対象のBitmap
            this.screenBitmap = screenBitmap;
            // マウス位置
            this.mousePoint = mousePoiont;
        }

        private void InitializeMouseEventHandler(Control control)
        {
            // イベントの設定（親フォームは除外）
            if (control != this)
            {
                control.MouseDown += (sender, e) => this.OnMouseDown(e);
                control.MouseMove += (sender, e) => this.OnMouseMove(e);
                //control.MouseUp += (sender, e) => this.OnMouseUp(e);
            }
            // 子コントロールに対するイベントの設定
            foreach (Control child in control.Controls)
            {
                InitializeMouseEventHandler(child);
            }
        }

        /// <summary>
        /// フォームが初めて表示される直前に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CoordinateForm_Load(object sender, EventArgs e)
        {
            // 座標表示と表示領域の更新
            this.UpdateCoordinate(this.mousePoint);
        }

        /// <summary>
        /// キー押下の際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CoordinateForm_KeyDown(object sender, KeyEventArgs e)
        {
            // キー操作を親フォームの処理とする
            this.captureForm.CaptureForm_KeyDown(sender, e);
        }

        /// <summary>
        /// マウスクリックの際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CoordinateForm_MouseDown(object sender, MouseEventArgs e)
        {
            // マウス操作を親フォームの処理とする
            this.captureForm.PictureBox_MouseDown(sender, e);
        }

        /// <summary>
        /// マウス移動の際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CoordinateForm_MouseMove(object sender, MouseEventArgs e)
        {
            // マウス操作を親フォームの処理とする
            this.captureForm.PictureBox_MouseMove(sender, e);
        }

        /// <summary>
        /// フォームを閉じる際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CoordinateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 描画用のビットマップを破棄
            Bitmap bitmap = (Bitmap)this.pictureBox.Image;
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// マウスポインタがピクチャボックスの表示範囲に入った際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            // マウスカーソル変更
            this.originalCursor = this.Cursor;
            this.Cursor = Cursors.Cross;
        }

        /// <summary>
        /// マウスポインタがピクチャボックスの表示範囲から出た際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            // マウスカーソル戻す
            if (this.originalCursor != this.Cursor)
            {
                this.Cursor = this.originalCursor;
            }
        }

        /// <summary>
        /// 座標表示と表示領域の更新
        /// </summary>
        /// <param name="mousePoint">マウスカーソルの位置</param>
        public void UpdateCoordinate(Point mousePoint)
        {
            // 表示座標更新
            this.Text = $"X:{mousePoint.X} Y:{mousePoint.Y}";
            // 表示領域更新
            var srcRectangle = this.GetCoordinateRectangle(mousePoint, out var deviationPoint);
            this.DrawCoordinateImage(srcRectangle);
            this.DrawCoordinateLine(deviationPoint);
        }

        /// <summary>
        /// マウスカーソルの位置をもとに表示領域の矩形を取得（拡大率考慮）
        /// </summary>
        /// <param name="mousePoint">マウスカーソルの位置</param>
        /// <param name="deviationPoint">マウスカーソルの位置差分（画面端の移動量）</param>
        /// <returns>表示領域の矩形</returns>
        private Rectangle GetCoordinateRectangle(Point mousePoint, out Point deviationPoint)
        {
            // マウスカーソルの位置差分（画面端の移動量）
            deviationPoint = new Point(0, 0);
            // X位置
            int width = (int)Math.Round(this.pictureBox.Width / this.rate);
            int x = mousePoint.X - (width / 2);
            if (x < 0)
            {
                deviationPoint.X = x;
                x = 0;
            }
            else if (x > this.screenBitmap.Width - width)
            {
                deviationPoint.X = mousePoint.X - (this.screenBitmap.Width - width / 2);
                x = this.screenBitmap.Width - width;
            }
            // Y位置
            int height = (int)Math.Round(this.pictureBox.Height / this.rate);
            int y = mousePoint.Y - (height / 2);
            if (y < 0)
            {
                deviationPoint.Y = y;
                y = 0;
            }
            else if (y > this.screenBitmap.Height - height)
            {
                deviationPoint.Y = mousePoint.Y - (this.screenBitmap.Height - height / 2);
                y = this.screenBitmap.Height - height;
            }
            // 表示領域の矩形
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// 表示領域の矩形をもとに表示領域を更新
        /// </summary>
        /// <param name="srcRectangle">表示領域の矩形</param>
        private void DrawCoordinateImage(Rectangle srcRectangle)
        {
            // 更新前のビットマップを取得
            Bitmap oldBitmap = (Bitmap)this.pictureBox.Image;
            try
            {
                // 描画用のビットマップ
                Bitmap canvas = new Bitmap(this.pictureBox.Width, this.pictureBox.Height);
                using (Graphics graphics = Graphics.FromImage(canvas))
                {
                    // 指定した位置とサイズで描画
                    var dstRectangle = new Rectangle(0, 0, this.pictureBox.Width, this.pictureBox.Height);
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.DrawImage(this.screenBitmap, dstRectangle, srcRectangle, GraphicsUnit.Pixel);
                }
                // 表示更新
                this.pictureBox.Image = canvas;
                this.pictureBox.Refresh();
            }
            finally
            {
                // 更新前のビットマップを破棄
                if (oldBitmap != null)
                {
                    oldBitmap.Dispose();
                }
            }
        }

        /// <summary>
        /// マウスカーソルの位置差分（画面端の移動量）をもとに線を描画
        /// </summary>
        /// <param name="deviationPoint">マウスカーソルの位置差分（画面端の移動量）</param>
        private void DrawCoordinateLine(Point deviationPoint)
        {
            using (Graphics graphics = Graphics.FromImage(this.pictureBox.Image))
            {
                // 黄色ペン
                Pen pen = new Pen(Color.Yellow, 1);
                pen.DashStyle = DashStyle.Solid;

                // 線を左上にずらすピクセル数
                int upLeft = (int)(rate / 2) + 1;
                if ((int)rate % 2 == 0 )
                {
                    upLeft--;
                }
                // 線を右下にずらすピクセル数
                int downRight = (int)(rate / 2) + 1;

                // 縦線
                var verticalX1 = (int)Math.Round(this.pictureBox.Width / 2 - upLeft + deviationPoint.X * this.rate);
                var verticalY1 = 0;
                var verticalX2 = (int)Math.Round(this.pictureBox.Width / 2 + downRight + deviationPoint.X * this.rate);
                var verticalY2 = this.pictureBox.Height;
                if (verticalX1 >= 0)
                {
                    graphics.DrawLine(pen, verticalX1, verticalY1, verticalX1, verticalY2);
                }
                if (verticalX2 < this.pictureBox.Width)
                {
                    graphics.DrawLine(pen, verticalX2, verticalY1, verticalX2, verticalY2);
                }
                // 横線
                var horizontalX1 = 0;
                var horizontalY1 = (int)Math.Round(this.pictureBox.Height / 2 - upLeft + deviationPoint.Y * this.rate);
                var horizontalX2 = this.pictureBox.Width;
                var horizontalY2 = (int)Math.Round(this.pictureBox.Height / 2 + downRight + deviationPoint.Y * this.rate);
                if (horizontalY1 >= 0)
                {
                    graphics.DrawLine(pen, horizontalX1, horizontalY1, horizontalX2, horizontalY1);
                }
                if (horizontalY2 < this.pictureBox.Height)
                {
                    graphics.DrawLine(pen, horizontalX1, horizontalY2, horizontalX2, horizontalY2);
                }
                // 表示更新
                this.pictureBox.Refresh();
            }
        }
    }
}
