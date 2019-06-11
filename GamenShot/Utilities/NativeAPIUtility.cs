using System;
using System.Runtime.InteropServices;

namespace GamenShot.Utilities
{
    public class NativeAPIUtility
    {
        /// <summary>
        /// RECT構造体
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// ウィンドウ属性
        /// </summary>
        [Flags]
        public enum DwmWindowAttribute : int
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        }

        /// <summary>
        /// ラスタオペレーション
        /// </summary>
        public enum BinaryRasterOperations : int
        {
            R2_BLACK = 1,
            R2_NOTMERGEPEN = 2,
            R2_MASKNOTPEN = 3,
            R2_NOTCOPYPEN = 4,
            R2_MASKPENNOT = 5,
            R2_NOT = 6,
            R2_XORPEN = 7,
            R2_NOTMASKPEN = 8,
            R2_MASKPEN = 9,
            R2_NOTXORPEN = 10,
            R2_NOP = 11,
            R2_MERGENOTPEN = 12,
            R2_COPYPEN = 13,
            R2_MERGEPENNOT = 14,
            R2_MERGEPEN = 15,
            R2_WHITE = 16
        }

        /// <summary>
        /// ペンスタイル
        /// </summary>
        public enum PenStyle : int
        {
            PS_SOLID = 0,
            PS_DASH = 1,
            PS_DOT = 2,
            PS_DASHDOT = 3,
            PS_DASHDOTDOT = 4,
            PS_NULL = 5,
            PS_INSIDEFRAME = 6,
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0x0000000F,
            PS_ENDCAP_ROUND = 0x00000000,
            PS_ENDCAP_SQUARE = 0x00000100,
            PS_ENDCAP_FLAT = 0x00000200,
            PS_ENDCAP_MASK = 0x00000F00,
            PS_JOIN_ROUND = 0x00000000,
            PS_JOIN_BEVEL = 0x00001000,
            PS_JOIN_MITER = 0x00002000,
            PS_JOIN_MASK = 0x0000F000,
            PS_COSMETIC = 0x00000000,
            PS_GEOMETRIC = 0x00010000,
            PS_TYPE_MASK = 0x000F0000
        };

        /// <summary>
        ///  指定されたウィンドウのクライアント領域またはスクリーン全体に対応するディスプレイデバイスコンテキストのハンドルを取得する
        /// </summary>
        /// <param name="hwnd">デバイスコンテキストを取得するウィンドウのハンドル</param>
        /// <returns>デバイスコンテキストのハンドル</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        /// <summary>
        /// ラスターオペレーションコード 0x00CC0020 (SRCCOPY)
        /// </summary>
        public const int SRCCOPY = 13369376;

        /// <summary>
        /// ラスターオペレーションコード 0x40000000 (CAPTUREBLT)
        /// </summary>
        public const int CAPTUREBLT = 1073741824;

        /// <summary>
        /// 画像のビットブロック転送を行う
        /// </summary>
        /// <param name="hDestDC">コピー先デバイスコンテキスト</param>
        /// <param name="x">コピー先x座標</param>
        /// <param name="y">コピー先y座標</param>
        /// <param name="nWidth">コピーする幅</param>
        /// <param name="nHeight">コピーする高さ</param>
        /// <param name="hSrcDC">コピー元デバイスコンテキスト</param>
        /// <param name="xSrc">コピー元x座標</param>
        /// <param name="ySrc">コピー元y座標</param>
        /// <param name="dwRop">ラスターオペレーションコード</param>
        /// <returns>成功：0以外の値、失敗：0</returns>
        [DllImport("gdi32.dll")]
        public static extern int BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        /// <summary>
        /// デバイスコンテキストを解放する
        /// </summary>
        /// <param name="hwnd">デバイスコンテキストを解放するウィンドウのハンドル</param>
        /// <param name="hdc">デバイスコンテキストのハンドル</param>
        /// <returns>デバイスコンテキストが解放された場合：1、解放されなかった場合：0</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        /// <summary>
        /// 非クライアント領域（タイトルバー、メニュー、スクロールバーなど）を含むウィンドウ全体のデバイスコンテキストのハンドルを取得する
        /// </summary>
        /// <param name="hwnd">デバイスコンテキストを取得するウィンドウのハンドル。0 (NULL) を指定すると、スクリーン全体のデバイスコンテキストを取得</param>
        /// <returns>指定されたウィンドウのデバイスコンテキストのハンドル</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        /// <summary>
        /// フォアグラウンドウィンドウ（現在ユーザーが作業している最前面のウィンドウ）のハンドルを取得する
        /// </summary>
        /// <returns>フォアグラウンドウィンドウのハンドル</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// ウィンドウの座標をスクリーン座標系で取得する
        /// </summary>
        /// <param name="hwnd">座標を取得するウィンドウのハンドル</param>
        /// <param name="lpRect">取得したウィンドウ座標を格納するためのRECT構造体のアドレス</param>
        /// <returns>成功：0以外の値、失敗：0</returns>
        [DllImport("user32.dll")]
        public static extern int GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        /// <summary>
        /// ウィンドウに適用されている指定のデスクトップウィンドウマネージャー（DWM）属性の現在の値を取得する
        /// </summary>
        /// <param name="hwnd">属性値を取得するウィンドウのハンドル</param>
        /// <param name="dwAttribute">取得する対象のフラグ</param>
        /// <param name="pvAttribute">取得した結果を受け取るRECT構造体のアドレス</param>
        /// <param name="cbAttribute">取得した結果の属性値のサイズ（バイト単位）</param>
        /// <returns>HRESULT</returns>
        [DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

        /// <summary>
        /// 現在の前景モードを設定する
        /// </summary>
        /// <param name="hdc">前景モードを設定したいデバイスコンテキストのハンドル</param>
        /// <param name="fnDrawMode">前景モード</param>
        /// <returns>成功：0以外の値、失敗：0</returns>
        [DllImport("gdi32")]
        public static extern int SetROP2(IntPtr hdc, int fnDrawMode);

        /// <summary>
        /// 現在の位置を更新して、指定された点を新しい現在の位置にする
        /// </summary>
        /// <param name="hdc">デバイスコンテキストのハンドル</param>
        /// <param name="x">新しい現在の位置の x 座標</param>
        /// <param name="y">新しい現在の位置の y 座標</param>
        /// <param name="prev">それまでの現在の位置を格納するポインタ</param>
        /// <returns>成功：0以外の値、失敗：0</returns>
        [DllImport("gdi32")]
        public static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr prev);

        /// <summary>
        /// 現在の位置と、指定された終点を結ぶ直線を描画する
        /// </summary>
        /// <param name="hdc"> デバイスコンテキストのハンドル</param>
        /// <param name="x">直線の終点の x 座標</param>
        /// <param name="y">直線の終点の y 座標</param>
        /// <returns>成功：0以外の値、失敗：0</returns>
        [DllImport("gdi32")]
        public static extern bool LineTo(IntPtr hdc, int x, int y);

        /// <summary>
        /// 指定されたスタイル、幅、色を持つ論理ペンを作成する
        /// </summary>
        /// <param name="fnPenStyle">ペンのスタイル</param>
        /// <param name="nWidth">ペンの幅</param>
        /// <param name="crColor">ペンの色</param>
        /// <returns>成功：論理ペンのハンドル、失敗：NULL</returns>
        [DllImport("gdi32")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        /// <summary>
        /// 指定されたデバイスコンテキストのオブジェクトを選択する
        /// </summary>
        /// <param name="hdc">デバイスコンテキストのハンドル</param>
        /// <param name="hObject">選択するオブジェクトのハンドル</param>
        /// <returns>成功：置きかえられる前に選択されていたオブジェクトのハンドル、失敗：NULL</returns>
        [DllImport("gdi32")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);
    }
}
