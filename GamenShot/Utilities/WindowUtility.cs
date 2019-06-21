using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace GamenShot.Utilities
{
    public abstract class WindowHandles : IEnumerable<IntPtr>
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal delegate bool EnumWindowsProcDelegate(IntPtr windowHandle, IntPtr lParam);

        internal List<IntPtr> handles;

        public WindowHandles()
        {
            handles = new List<IntPtr>();
        }

        public IEnumerator<IntPtr> GetEnumerator()
        {
            return handles.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return handles.GetEnumerator();
        }

        internal bool EnumWindowProc(IntPtr handle, IntPtr lParam)
        {
            handles.Add(handle);
            return true;
        }
    }

    /// <summary>
    /// トップレベルウィンドウのウィンドウハンドルを列挙する機能を提供します。
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class TopLevelWindowHandles : WindowHandles
    {
        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumWindows([MarshalAs(UnmanagedType.FunctionPtr)] EnumWindowsProcDelegate enumProc, IntPtr lParam);
        }

        /// <summary>
        /// トップレベルウィンドウのウィンドウハンドルを列挙します。
        /// </summary>
        public TopLevelWindowHandles() : base()
        {
            handles = new List<IntPtr>();
            NativeMethods.EnumWindows(EnumWindowProc, default(IntPtr));
        }
    }

    /// <summary>
    /// 親ウィンドウの子ウィンドウのウィンドウハンドルを列挙する機能を提供します。
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class ChildWindowHandles : WindowHandles
    {
        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumChildWindows(IntPtr handle, [MarshalAs(UnmanagedType.FunctionPtr)] EnumWindowsProcDelegate enumProc, IntPtr lParam);
        }

        /// <summary>
        /// 親ウィンドウのウィンドウハンドル
        /// </summary>
        public IntPtr WindowHandle { private set; get; }

        /// <summary>
        /// 親ウィンドウの子ウィンドウのウィンドウハンドルを取得します。
        /// </summary>
        /// <param name="windowHandle">親ウィンドウのウィンドウハンドル</param>
        public ChildWindowHandles(IntPtr windowHandle) : base()
        {
            this.WindowHandle = windowHandle;
            NativeMethods.EnumChildWindows(this.WindowHandle, EnumWindowProc, default(IntPtr));
        }
    }

    /// <summary>
    /// スレッドに所属するトップレベルウィンドウのウィンドウハンドルを列挙する機能を提供します。
    /// </summary>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class ThreadWindowHandles : WindowHandles
    {
        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumThreadWindows(uint threadId, [MarshalAs(UnmanagedType.FunctionPtr)] EnumWindowsProcDelegate enumProc, IntPtr lParam);
        }

        /// <summary>
        /// スレッドID
        /// </summary>
        public uint ThreadID { private set; get; }

        /// <summary>
        /// スレッドに所属するトップレベルウィンドウのウィンドウハンドルを取得します。
        /// </summary>
        /// <param name="threadId">スレッドID</param>
        public ThreadWindowHandles(uint threadId) : base()
        {
            this.ThreadID = threadId;
            NativeMethods.EnumThreadWindows(threadId, EnumWindowProc, default(IntPtr));
        }
    }

    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class WindowInfo
    {
        [SuppressUnmanagedCodeSecurity]
        private static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        }

        public IntPtr Handle { get; set; }

        public string WindowText { get; set; }

        public string ClassName { get; set; }

        public WindowInfo(IntPtr handle)
        {
            this.Handle = handle;
            var length = NativeMethods.GetWindowTextLength(this.Handle);
            if (length > 0)
            {
                // ウィンドウテキスト
                StringBuilder text = new StringBuilder(length + 1);
                NativeMethods.GetWindowText(this.Handle, text, text.Capacity);
                this.WindowText = text.ToString();
                // クラス名
                StringBuilder name = new StringBuilder(256);
                NativeMethods.GetClassName(this.Handle, name, name.Capacity);
                this.ClassName = name.ToString();
            }
        }
    }
}
