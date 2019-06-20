using GamenShot.Hook;
using System;
using System.Threading;
using System.Windows.Forms;

namespace GamenShot
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Mutex名
            string mutexName = "GamenShot";
            // Mutexオブジェクトを作成
            Mutex mutex = new Mutex(false, mutexName);
            bool hasHandle = false;
            try
            {
                try
                {
                    // ミューテックスの所有権を要求
                    hasHandle = mutex.WaitOne(0, false);
                }
                catch (System.Threading.AbandonedMutexException)
                {
                    // 別のアプリケーションがミューテックスを解放しないで終了している
                    hasHandle = true;
                }

                if (hasHandle == false)
                {
                    // ミューテックスの所有権を得られなかった場合は起動していると判断して終了
                    MessageBox.Show("すでに起動しています。", "画面ショット", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // アプリケーションの実行
                using (var gamenShotForm = new GamenShotForm())
                {
                    try
                    {
                        Application.Run();
                    }
                    finally
                    {
                        // キーボードフックの停止
                        if (KeyboardHook.IsHooking)
                        {
                            KeyboardHook.Stop();
                        }
                    }
                }
            }
            finally
            {
                if (hasHandle)
                {
                    // ミューテックスを解放
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
        }
    }
}
