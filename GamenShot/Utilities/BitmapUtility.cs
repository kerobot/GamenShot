using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace GamenShot.Utilities
{
    class BitmapUtility
    {
        /// <summary>
        /// BitmapをARGB配列に変換する
        /// </summary>
        /// <param name="bitmap">変換元の32bit ARGB Bitmap</param>
        /// <returns>1pixel = 4byte (+3:A, +2:R, +1:G, +0:B) に変換したbyte配列</returns>
        public static byte[] BitmapToByteArray(Bitmap bmp)
        {
            // Bitmapの矩形を得る
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            // 変換先のBitmapのメモリ展開を開始
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            // Bitmapの先頭アドレスを取得
            IntPtr ptr = bmpData.Scan0;
            // 32bpp ARGBフォーマットで値を格納
            int bytes = bmp.Width * bmp.Height * 4;
            byte[] argbArray = new byte[bytes];
            // Bitmapをbyte[]へコピー
            Marshal.Copy(ptr, argbArray, 0, bytes);
            // 変換先のBitmapのメモリ展開を終了
            bmp.UnlockBits(bmpData);
            return argbArray;
        }

        /// <summary>
        /// ARGB配列をBitmapに変換する
        /// </summary>
        /// <param name="argbArray">1pixel = 4byte (+3:A, +2:R, +1:G, +0:B) に変換したbyte配列</param>
        /// <param name="bmp">変換先のBitmap。あらかじめ必要な大きさを確保しておく</param>
        /// <returns>変換先のBitmap</returns>
        public static Bitmap ByteArrayToBitmap(byte[] argbArray, Bitmap bmp)
        {
            // Bitmapの矩形を得る
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            // 変換先のBitmapのメモリ展開を開始
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            // Bitmapの先頭アドレスを取得
            IntPtr ptr = bmpData.Scan0;
            // ARGB配列をBitmapへコピー
            Marshal.Copy(argbArray, 0, ptr, argbArray.Length);
            // 変換先のBitmapのメモリ展開を終了
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Bitmapを32bitのARGB配列に変換
        /// </summary>
        /// <param name="bmp">変換元のBitmap</param>
        /// <returns>変換後のARGB配列</returns>
        public static byte[] BitmapTo32bitARGBArray(Bitmap bmp)
        {
            byte[] argbArray = null;
            using (Bitmap bmp32 = new Bitmap(bmp.Width, bmp.Height))
            {
                // Bitmapが32bitARGB形式ではない場合でも32bitARGBに変換
                using (Graphics g = Graphics.FromImage(bmp32))
                {
                    var rect32 = new Rectangle(0, 0, bmp32.Width, bmp32.Height);
                    var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    g.DrawImage(bmp, rect32, rect, GraphicsUnit.Pixel);
                }
                argbArray = BitmapToByteArray(bmp32);
            }
            return argbArray;
        }
    }
}
