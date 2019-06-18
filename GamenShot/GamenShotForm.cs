using GamenShot.Hook;
using GamenShot.Utilities;
using GamenShot.nQuant;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GamenShot
{
    public partial class GamenShotForm : Form
    {
        /// <summary>
        /// 通知領域のアイコン
        /// </summary>
        private NotifyIcon icon = null;

        /// <summary>
        /// キャプチャ中
        /// </summary>
        private bool isCapturing = false;

        /// <summary>
        /// キャプチャの種類
        /// </summary>
        private enum CaptureType : int
        {
            Clipboard = 1,      // クリップボード
            Bitmap,             // ビットマップ
            Png                 // PNG
        }

        /// <summary>
        /// キャプチャの対象
        /// </summary>
        private enum CaptureTarget : int
        {
            RectArea = 1,       // 矩形指定
            ActiveWindow,       // アクティブウィンドウ
            Desktop             // デスクトップ
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GamenShotForm()
        {
            // コンポーネントの初期化
            InitializeComponent();
            // コンポーネントの設定
            this.SetComponents();
            // キーフックの初期化
            this.InitializeKeyHook();
            // 設定の初期化
            this.InitializeSettings();
        }

        /// <summary>
        /// コンポーネントの設定
        /// </summary>
        private void SetComponents()
        {
            // タスクバーに表示しない
            this.ShowInTaskbar = false;

            // 通知アイコン
            this.icon = new NotifyIcon
            {
                Icon = Properties.Resources.GamenShot,
                Visible = true,
                Text = "画面ショット"
            };

            ContextMenuStrip menu = new ContextMenuStrip();

            // 通知アイコンのメニュー項目（設定）
            ToolStripMenuItem menuItemSetting = new ToolStripMenuItem
            {
                Text = "&設定"
            };
            menuItemSetting.Click += new EventHandler(ToolStripMenuItemSetting_Click);
            menu.Items.Add(menuItemSetting);

            // 通知アイコンのメニュー項目（保存フォルダ）
            ToolStripMenuItem menuItemSaveFolder = new ToolStripMenuItem
            {
                Text = "&保存フォルダ"
            };
            menuItemSaveFolder.Click += new EventHandler(ToolStripMenuItemSaveFolder_Click);
            menu.Items.Add(menuItemSaveFolder);

            // 通知アイコンのメニュー項目（クリップボード）
            ToolStripMenuItem menuItemClipboard = new ToolStripMenuItem
            {
                Text = "&クリップボード"
            };
            menu.Items.Add(menuItemClipboard);

            // 通知アイコンのメニュー項目（矩形領域選択）
            ToolStripMenuItem menuItemClipboardRectArea = new ToolStripMenuItem
            {
                Text = "&矩形領域指定(Ctrl+Shift+F10)",
                Tag = CaptureType.Clipboard
            };
            menuItemClipboardRectArea.Click += new EventHandler(ToolStripMenuItemRectArea_Click);
            menuItemClipboard.DropDownItems.Add(menuItemClipboardRectArea);

            // 通知アイコンのメニュー項目（アクティブウィンドウ）
            ToolStripMenuItem menuItemClipboardActiveWindow = new ToolStripMenuItem
            {
                Text = "&アクティブウィンドウ(Ctrl+Shift+F11)",
                Tag = CaptureType.Clipboard
            };
            menuItemClipboardActiveWindow.Click += new EventHandler(ToolStripMenuItemActiveWindow_Click);
            menuItemClipboard.DropDownItems.Add(menuItemClipboardActiveWindow);

            // 通知アイコンのメニュー項目（デスクトップ全体）
            ToolStripMenuItem menuItemClipboardDesktop = new ToolStripMenuItem
            {
                Text = "&デスクトップ全体(Ctrl+Shift+F12)",
                Tag = CaptureType.Clipboard
            };
            menuItemClipboardDesktop.Click += new EventHandler(ToolStripMenuItemDesktop_Click);
            menuItemClipboard.DropDownItems.Add(menuItemClipboardDesktop);

            // 通知アイコンのメニュー項目（PNG）
            ToolStripMenuItem menuItemPng = new ToolStripMenuItem
            {
                Text = "&PNG保存"
            };
            menu.Items.Add(menuItemPng);

            // 通知アイコンのメニュー項目（矩形領域選択）
            ToolStripMenuItem menuItemPngRectArea = new ToolStripMenuItem
            {
                Text = "&矩形領域指定(Ctrl+Alt+F10)",
                Tag = CaptureType.Png
            };
            menuItemPngRectArea.Click += new EventHandler(ToolStripMenuItemRectArea_Click);
            menuItemPng.DropDownItems.Add(menuItemPngRectArea);

            // 通知アイコンのメニュー項目（アクティブウィンドウ）
            ToolStripMenuItem menuItemPngActiveWindow = new ToolStripMenuItem
            {
                Text = "&アクティブウィンドウ(Ctrl+Alt+F11)",
                Tag = CaptureType.Png
            };
            menuItemPngActiveWindow.Click += new EventHandler(ToolStripMenuItemActiveWindow_Click);
            menuItemPng.DropDownItems.Add(menuItemPngActiveWindow);

            // 通知アイコンのメニュー項目（デスクトップ全体）
            ToolStripMenuItem menuItemPngDesktop = new ToolStripMenuItem
            {
                Text = "&デスクトップ全体(Ctrl+Alt+F12)",
                Tag = CaptureType.Png
            };
            menuItemPngDesktop.Click += new EventHandler(ToolStripMenuItemDesktop_Click);
            menuItemPng.DropDownItems.Add(menuItemPngDesktop);

            // 通知アイコンのメニュー項目（Bitmap）
            ToolStripMenuItem menuItemBitmap = new ToolStripMenuItem
            {
                Text = "&Bitmap保存"
            };
            menu.Items.Add(menuItemBitmap);

            // 通知アイコンのメニュー項目（矩形領域選択）
            ToolStripMenuItem menuItemBitmapRectArea = new ToolStripMenuItem
            {
                Text = "&矩形領域指定(Ctrl+Shift+Alt+F10)",
                Tag = CaptureType.Bitmap
            };
            menuItemBitmapRectArea.Click += new EventHandler(ToolStripMenuItemRectArea_Click);
            menuItemBitmap.DropDownItems.Add(menuItemBitmapRectArea);

            // 通知アイコンのメニュー項目（アクティブウィンドウ）
            ToolStripMenuItem menuItemBitmapActiveWindow = new ToolStripMenuItem
            {
                Text = "&アクティブウィンドウ(Ctrl+Shift+Alt+F11)",
                Tag = CaptureType.Bitmap
            };
            menuItemBitmapActiveWindow.Click += new EventHandler(ToolStripMenuItemActiveWindow_Click);
            menuItemBitmap.DropDownItems.Add(menuItemBitmapActiveWindow);

            // 通知アイコンのメニュー項目（デスクトップ全体）
            ToolStripMenuItem menuItemBitmapDesktop = new ToolStripMenuItem
            {
                Text = "&デスクトップ全体(Ctrl+Shift+Alt+F12)",
                Tag = CaptureType.Bitmap
            };
            menuItemBitmapDesktop.Click += new EventHandler(ToolStripMenuItemDesktop_Click);
            menuItemBitmap.DropDownItems.Add(menuItemBitmapDesktop);

            // 通知アイコンのメニュー項目（終了）
            ToolStripMenuItem menuItemExit = new ToolStripMenuItem
            {
                Text = "&終了"
            };
            menuItemExit.Click += new EventHandler(ToolStripMenuItemExit_Click);
            menu.Items.Add(menuItemExit);
            this.icon.ContextMenuStrip = menu;

            // 通知アイコンのダブルクリック時のイベントハンドラ
            this.icon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);

            // フォームを閉じる際のイベントハンドラ
            this.FormClosing += new FormClosingEventHandler(GamenShotForm_FormClosing);
        }

        /// <summary>
        /// キーフックの初期化
        /// </summary>
        private void InitializeKeyHook()
        {
            // キーフックを行っていなければキーフックイベントを追加
            if (!KeyboardHook.IsHooking)
            {
                KeyboardHook.AddEvent(KeyboardHook_GetKeyState);
                KeyboardHook.Start();
            }
        }

        /// <summary>
        /// 設定の初期化
        /// </summary>
        private void InitializeSettings()
        {
            // 保存フォルダが未設定もしくは存在しない場合はユーザーのデスクトップを設定しておく
            var saveFolderPath = Properties.Settings.Default.SaveForderPath;
            if (string.IsNullOrWhiteSpace(saveFolderPath) || !Directory.Exists(saveFolderPath))
            {
                saveFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                Properties.Settings.Default.SaveForderPath = saveFolderPath;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 通知アイコンをダブルクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            // 設定画面の表示
            this.ShowSettings();
        }

        /// <summary>
        /// 通知アイコンの設定をクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemSetting_Click(object sender, EventArgs e)
        {
            // 設定画面の表示
            this.ShowSettings();
        }

        /// <summary>
        /// 通知アイコンの保存フォルダをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemSaveFolder_Click(object sender, EventArgs e)
        {
            // 保存フォルダを開く
            var saveFolderPath = Properties.Settings.Default.SaveForderPath;
            if (!string.IsNullOrWhiteSpace(saveFolderPath) && Directory.Exists(saveFolderPath))
            {
                Process.Start(saveFolderPath);
            }
            else
            {
                MessageBox.Show("保存フォルダを設定してください。", "画面ショット", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 通知アイコンの矩形領域をクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemRectArea_Click(object sender, EventArgs e)
        {
            // 矩形領域キャプチャ
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            var captureType = (CaptureType)menuItem.Tag;
            this.CaptureImage(CaptureTarget.RectArea, captureType);
        }

        /// <summary>
        /// 通知アイコンのアクティブウィンドウをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemActiveWindow_Click(object sender, EventArgs e)
        {
            // アクティブウィンドウキャプチャ
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            var captureType = (CaptureType)menuItem.Tag;
            this.CaptureImage(CaptureTarget.ActiveWindow, captureType);
        }

        /// <summary>
        /// 通知アイコンのデスクトップをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemDesktop_Click(object sender, EventArgs e)
        {
            // デスクトップキャプチャ
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            var captureType = (CaptureType)menuItem.Tag;
            this.CaptureImage(CaptureTarget.Desktop, captureType);
        }

        /// <summary>
        /// 通知アイコンの終了をクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            // 通知アイコンを非表示
            this.icon.Visible = false;
            // アプリケーションの終了
            Application.Exit();
        }

        /// <summary>
        /// キーフックのイベントメソッド
        /// </summary>
        /// <param name="state">キーボードの状態</param>
        void KeyboardHook_GetKeyState(ref KeyboardHook.StateKeyboard state)
        {
            if (this.isCapturing && state.Stroke == KeyboardHook.Stroke.KEY_UP)
            {
                this.isCapturing = false;
            }
            else if (this.isCapturing)
            {
                return;
            }
            else if(!this.isCapturing && state.Stroke == KeyboardHook.Stroke.KEY_DOWN)
            {
                this.isCapturing = true;
            }
#if DEBUG
            Debug.WriteLine($"Ctrl:{state.WithControl.ToString()}, Shift:{state.WithShift.ToString()}, Alt:{state.WithAlt.ToString()}, F11:{(state.Key == Keys.F11).ToString()}");
#endif
            // Ctrlキー
            if (state.WithControl)
            {
                // Shiftキー＋ALTキー
                if (state.WithShift && state.WithAlt)
                {
                    // F10キー
                    if (state.Key == Keys.F10)
                    {
                        // 矩形指定→Bitmap
                        this.CaptureImage(CaptureTarget.RectArea, CaptureType.Bitmap);
                    }
                    // F11キー
                    else if (state.Key == Keys.F11)
                    {
                        // アクティブウィンドウ→Bitmap
                        this.CaptureImage(CaptureTarget.ActiveWindow, CaptureType.Bitmap);
                    }
                    // F12キー
                    else if (state.Key == Keys.F12)
                    {
                        // デスクトップ→Bitmap
                        this.CaptureImage(CaptureTarget.Desktop, CaptureType.Bitmap);
                    }
                }
                // Shiftキー
                else if (state.WithShift)
                {
                    // F10キー
                    if (state.Key == Keys.F10)
                    {
                        // 矩形指定→クリップボード
                        this.CaptureImage(CaptureTarget.RectArea, CaptureType.Clipboard);
                    }
                    // F11キー
                    else if (state.Key == Keys.F11)
                    {
                        // アクティブウィンドウ→クリップボード
                        this.CaptureImage(CaptureTarget.ActiveWindow, CaptureType.Clipboard);
                    }
                    // F12キー
                    else if (state.Key == Keys.F12)
                    {
                        // デスクトップ→クリップボード
                        this.CaptureImage(CaptureTarget.Desktop, CaptureType.Clipboard);
                    }
                }
                // ALTキー
                else if (state.WithAlt)
                {
                    // F10キー
                    if (state.Key == Keys.F10)
                    {
                        // 矩形指定→PNG
                        this.CaptureImage(CaptureTarget.RectArea, CaptureType.Png);
                    }
                    // F11キー
                    else if (state.Key == Keys.F11)
                    {
                        // アクティブウィンドウ→PNG
                        this.CaptureImage(CaptureTarget.ActiveWindow, CaptureType.Png);
                    }
                    // F12キー
                    else if (state.Key == Keys.F12)
                    {
                        // デスクトップ→PNG
                        this.CaptureImage(CaptureTarget.Desktop, CaptureType.Png);
                    }
                }
            }
        }

        /// <summary>
        /// フォームの参照ボタンをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void SaveFolderPathButton_Click(object sender, EventArgs e)
        {
            // 保存フォルダを取得
            var saveFolderPath = Properties.Settings.Default.SaveForderPath;
            // フォルダ選択ダイアログ
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "保存フォルダを選択してください。";
            fbd.ShowNewFolderButton = true;
            fbd.SelectedPath = saveFolderPath;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                // 保存フォルダを設定
                this.saveFolderPathTextBox.Text = fbd.SelectedPath;
            }
        }

        /// <summary>
        /// フォームのOKボタンをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            // 設定画面の非表示（保存あり）
            this.HideSettings(true);
        }

        /// <summary>
        /// フォームのキャンセルボタンをクリックした際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            // 設定画面の非表示（保存なし）
            this.HideSettings();
        }

        /// <summary>
        /// フォームを閉じる際に呼び出されるイベントメソッド
        /// </summary>
        /// <param name="sender">イベント送出元</param>
        /// <param name="e">イベント引数</param>
        private void GamenShotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 設定画面が表示されていたらフォームを閉じずに切り替えのみ行う
            if (this.Visible)
            {
                // フォームを閉じない
                e.Cancel = true;
                // 設定画面の非表示（保存なし）
                this.HideSettings();
            }
        }

        /// <summary>
        /// 設定画面の表示
        /// </summary>
        private void ShowSettings()
        {
            // 表示済みの場合は何もしない
            if (this.Visible)
            {
                return;
            }
            // キーボードフック中断
            KeyboardHook.Pause();

            // 保存フォルダを取得
            var saveFolderPath = Properties.Settings.Default.SaveForderPath;
            // 保存フォルダを設定
            this.saveFolderPathTextBox.Text = saveFolderPath;
            // 製品名
            AssemblyProductAttribute attribute = (AssemblyProductAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute));
            this.nameValueLabel.Text = attribute.Product;
            // バージョン
            Assembly assembly = Assembly.GetExecutingAssembly();
            this.versionValueLabel.Text = assembly.GetName().Version.ToString();

            // 設定画面の最前面設定
            this.TopMost = true;
            // 設定画面の表示
            this.Visible = true;
        }

        /// <summary>
        /// 設定画面の非表示
        /// </summary>
        /// <param name="isSave">設定を保存する場合true</param>
        private void HideSettings(bool isSave = false)
        {
            // 非表示済みの場合は何もしない
            if (!this.Visible)
            {
                return;
            }
            // 設定を保存する場合
            if(isSave)
            {
                // 保存フォルダを取得
                var saveFolderPath = this.saveFolderPathTextBox.Text;
                if (string.IsNullOrWhiteSpace(saveFolderPath) || !Directory.Exists(saveFolderPath))
                {
                    MessageBox.Show("存在する保存フォルダを指定して下さい。", "画面ショット", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                // 保存フォルダを設定
                Properties.Settings.Default.SaveForderPath = saveFolderPath;
                // 設定の保存
                Properties.Settings.Default.Save();
            }
            // 設定画面の非表示
            this.Visible = false;
            // 設定画面の最前面解除
            this.TopMost = false;
            // キーボードフック再開
            KeyboardHook.Start();
        }

        /// <summary>
        /// 指定した対象を指定した種類でキャプチャする
        /// </summary>
        /// <param name="target">キャプチャ対象</param>
        /// <param name="type">キャプチャ種類</param>
        private void CaptureImage(CaptureTarget target, CaptureType type)
        {
            // 矩形領域
            if (target == CaptureTarget.RectArea)
            {
                using (Bitmap screenBitmap = this.CaptureScreen())
                using (CaptureForm captureForm = new CaptureForm(screenBitmap))
                {
                    captureForm.ShowDialog();
                    using (Bitmap captureBitmap = captureForm.CaptureBitmap)
                    {
                        this.SaveImage(captureBitmap, type);
                    }
                }
            }
            // デスクトップ
            else if (target == CaptureTarget.Desktop)
            {
                using (Bitmap captureBitmap = this.CaptureScreen())
                {
                    this.SaveImage(captureBitmap, type);
                }
            }
            // アクティブウィンドウ
            else if (target == CaptureTarget.ActiveWindow)
            {
                using (Bitmap captureBitmap = this.CaptureActiveWindow())
                {
                    this.SaveImage(captureBitmap, type);
                }
            }
        }

        /// <summary>
        /// 指定した種類でキャプチャを保存する
        /// </summary>
        /// <param name="bitmap">キャプチャしたBitmap</param>
        /// <param name="type">キャプチャ種類</param>
        private void SaveImage(Bitmap bitmap, CaptureType type)
        {
            if (bitmap == null)
            {
                return;
            }
            // 保存フォルダを取得
            var saveFolderPath = Properties.Settings.Default.SaveForderPath;
            // クリップボード
            if (type == CaptureType.Clipboard)
            {
                // PNGに変換したうえでクリップボードに格納
                using (MemoryStream stream = new MemoryStream())
                {
                    //bitmap.Save(stream, ImageFormat.Png);
                    // インデックスカラー(8bit)に変換
                    var quantizer = new WuQuantizer();
                    using (var quantized = quantizer.QuantizeImage(bitmap, 10, 70))
                    {
                        quantized.Save(stream, ImageFormat.Png);
                    }
                    var data = new DataObject("PNG", stream);
                    Clipboard.Clear();
                    Clipboard.SetDataObject(data, true);
                }
            }
            // PNG
            else if (type == CaptureType.Png)
            {
                //bitmap.Save(Path.Combine(saveFolderPath, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png"), ImageFormat.Png);
                // インデックスカラー(8bit)に変換
                var quantizer = new WuQuantizer();
                using (var quantized = quantizer.QuantizeImage(bitmap, 10, 70))
                {
                    quantized.Save(Path.Combine(saveFolderPath, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.png"), ImageFormat.Png);
                }
            }
            // Bitmap
            else if (type == CaptureType.Bitmap)
            {
                bitmap.Save(Path.Combine(saveFolderPath, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.bmp"), ImageFormat.Bmp);
            }
        }

        /// <summary>
        /// スクリーン全体のキャプチャ
        /// </summary>
        /// <returns>キャプチャしたBMP画像</returns>
        private Bitmap CaptureScreen()
        {
            IntPtr displayDC = IntPtr.Zero;
            Graphics graphics = null;
            IntPtr hDC = IntPtr.Zero;
            try
            {
                // プライマリモニタのデバイスコンテキストを取得
                displayDC = NativeAPIUtility.GetDC(IntPtr.Zero);
                // Bitmapの作成
                Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                // Graphicsの作成
                graphics = Graphics.FromImage(bmp);
                // Graphicsのデバイスコンテキストを取得
                hDC = graphics.GetHdc();
                // Bitmapに画像をコピー
                NativeAPIUtility.BitBlt(hDC, 0, 0, bmp.Width, bmp.Height, displayDC, 0, 0, NativeAPIUtility.SRCCOPY);
                return bmp;
            }
            finally
            {
                if(hDC != IntPtr.Zero)
                {
                    graphics.ReleaseHdc(hDC);
                }
                if(graphics != null)
                {
                    graphics.Dispose();
                }
                if (displayDC != IntPtr.Zero)
                {
                    NativeAPIUtility.ReleaseDC(IntPtr.Zero, displayDC);
                }
            }
        }

        /// <summary>
        /// アクティブウィンドウのキャプチャ
        /// </summary>
        /// <returns>キャプチャしたBMP画像</returns>
        private Bitmap CaptureActiveWindow()
        {
            IntPtr hWnd = IntPtr.Zero;
            IntPtr windowDC = IntPtr.Zero;
            Graphics graphics = null;
            IntPtr hDC = IntPtr.Zero;
            try
            {
                // アクティブウィンドウのデバイスコンテキストを取得
                hWnd = NativeAPIUtility.GetForegroundWindow();
                windowDC = NativeAPIUtility.GetWindowDC(hWnd);
                // ウィンドウサイズを取得
                NativeAPIUtility.RECT rect = new NativeAPIUtility.RECT();
                // TODO:クラシックモードを考慮していないことに注意（Aaro有効を前提）
                NativeAPIUtility.DwmGetWindowAttribute(hWnd, (int)NativeAPIUtility.DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out var bounds, Marshal.SizeOf(typeof(NativeAPIUtility.RECT)));
                NativeAPIUtility.GetWindowRect(hWnd, ref rect);
                // Bitmapの作成
                Bitmap bmp = new Bitmap(bounds.right - bounds.left, bounds.bottom - bounds.top);
                // Graphicsの作成
                graphics = Graphics.FromImage(bmp);
                // Graphicsのデバイスコンテキストを取得
                hDC = graphics.GetHdc();
                // Bitmapに画像をコピー
                NativeAPIUtility.BitBlt(hDC, 0, 0, bmp.Width, bmp.Height, windowDC, bounds.left - rect.left, bounds.top - rect.top, NativeAPIUtility.SRCCOPY);
                return bmp;
            }
            finally
            {
                if (hDC != IntPtr.Zero)
                {
                    graphics.ReleaseHdc(hDC);
                }
                if (graphics != null)
                {
                    graphics.Dispose();
                }
                if (windowDC != IntPtr.Zero)
                {
                    NativeAPIUtility.ReleaseDC(hWnd, windowDC);
                }
            }
        }
    }
}
