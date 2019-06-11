namespace GamenShot.Hook
{
    public static class InputSimulator
    {
        /// <summary>
        /// P/Invoke
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// 仮想キーコードをスキャンコード、または文字の値（ASCII 値）へ変換します。
            /// また、スキャンコードを仮想コードへ変換することもできます。
            /// </summary>
            /// <param name="wCode">仮想キーコードまたはスキャンコード</param>
            /// <param name="wMapType">実行したい変換の種類</param>
            /// <returns></returns>
            [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
            public extern static int MapVirtualKey(int wCode, int wMapType);

            /// <summary>
            /// キーストローク、マウスの動き、ボタンのクリックなどを合成します。
            /// </summary>
            /// <param name="nInputs"><paramref name="pInputs"/> 配列内の構造体の数を指定します。</param>
            /// <param name="pInputs">INPUT 構造体の配列へのポインタを指定します。構造体はそれぞれキーボードまたはマウス入力ストリームに挿入されるイベントを表します。</param>
            /// <param name="cbsize">INPUT 構造体のサイズを指定します。cbSize パラメータの値が INPUT 構造体のサイズと等しくない場合、関数は失敗します。</param>
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);
        }

        /// <summary>
        /// シミュレートされたマウスイベントの構造体
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int X;
            public int Y;
            public int Data;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        /// <summary>
        /// シミュレートされたキーボードイベントの構造体
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public short VirtualKey;
            public short ScanCode;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        /// <summary>
        /// キーボードやマウス以外の入力デバイスによって生成されたシミュレートされたメッセージの構造体
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        /// <summary>
        /// キーストローク、マウスの動き、マウスクリックなどの入力イベントの構造体
        /// </summary>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Input
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public int Type;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public MouseInput Mouse;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public KeyboardInput Keyboard;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public HardwareInput Hardware;
        }

        /// <summary>
        /// マウス動作の列挙型
        /// </summary>
        public enum MouseStroke
        {
            MOVE = 0x0001,
            LEFT_DOWN = 0x0002,
            LEFT_UP = 0x0004,
            RIGHT_DOWN = 0x0008,
            RIGHT_UP = 0x0010,
            MIDDLE_DOWN = 0x0020,
            MIDDLE_UP = 0x0040,
            X_DOWN = 0x0080,
            X_UP = 0x0100,
            WHEEL = 0x0800
        }

        /// <summary>
        /// キーボード動作の列挙型
        /// </summary>
        public enum KeyboardStroke
        {
            KEY_DOWN = 0x0000,
            KEY_UP = 0x0002
        }

        /// <summary>
        /// KEYEVENTF_UNICODE
        /// </summary>
        private const int KBD_UNICODE = 0x0004;

        /// <summary>
        /// マウス入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="flag">移動とクリックのオプション</param>
        /// <param name="data">シミュレートされたマウスイベントに関する情報（ホイール回転量又はXボタン番号）</param>
        /// <param name="absolute">マウス座標を絶対値で指定する場合は true</param>
        /// <param name="x">水平位置または移動量</param>
        /// <param name="y">垂直位置または移動量</param>
        public static void AddMouseInput(ref System.Collections.Generic.List<Input> inputs, MouseStroke flag, int data, bool absolute, int x, int y)
        {
            AddMouseInput(ref inputs, new System.Collections.Generic.List<MouseStroke> { flag }, data, absolute, x, y);
        }

        /// <summary>
        /// マウス入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="flags">移動とクリックのオプション</param>
        /// <param name="data">シミュレートされたマウスイベントに関する情報（ホイール回転量又はXボタン番号）</param>
        /// <param name="absolute">マウス座標を絶対値で指定する場合は true</param>
        /// <param name="x">水平位置または移動量</param>
        /// <param name="y">垂直位置または移動量</param>
        public static void AddMouseInput(ref System.Collections.Generic.List<Input> inputs, System.Collections.Generic.List<MouseStroke> flags, int data, bool absolute, int x, int y)
        {
            if (flags == null)
            {
                return;
            }

            int mouseFlags = 0;

            foreach (MouseStroke f in flags)
            {
                mouseFlags |= (int)f;
            }

            if (absolute)
            {
                // ABSOLUTE = 0x8000
                mouseFlags |= 0x8000;

                x *= (65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width);
                y *= (65535 / System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            }

            AddMouseInput(ref inputs, mouseFlags, data, x, y, 0, 0);
        }

        /// <summary>
        /// マウス入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="flags">移動とクリックのオプション</param>
        /// <param name="data">シミュレートされたマウスイベントに関する情報（ホイール回転量又はXボタン番号）</param>
        /// <param name="x">水平位置または移動量</param>
        /// <param name="y">垂直位置または移動量</param>
        /// <param name="time">ミリ秒単位でのイベントのタイムスタンプ</param>
        /// <param name="extraInfo">マウスイベントに関連する追加の値</param>
        public static void AddMouseInput(ref System.Collections.Generic.List<Input> inputs, int flags, int data, int x, int y, int time, int extraInfo)
        {
            Input input = new Input();
            input.Type = 0; // MOUSE = 0
            input.Mouse.Flags = flags;
            input.Mouse.Data = data;
            input.Mouse.X = x;
            input.Mouse.Y = y;
            input.Mouse.Time = time;
            input.Mouse.ExtraInfo = extraInfo;

            inputs.Add(input);
        }

        /// <summary>
        /// キーボード入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="srcStr">入力する文字列</param>
        public static void AddKeyboardInput(ref System.Collections.Generic.List<Input> inputs, string srcStr)
        {
            if (System.String.IsNullOrEmpty(srcStr))
            {
                return;
            }

            foreach (char s in srcStr)
            {
                AddKeyboardInput(ref inputs, (int)KeyboardStroke.KEY_DOWN | KBD_UNICODE, 0, (short)s, 0, 0);
                AddKeyboardInput(ref inputs, (int)KeyboardStroke.KEY_UP | KBD_UNICODE, 0, (short)s, 0, 0);
            }
        }

        /// <summary>
        /// キーボード入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="flags">キーボードの動作</param>
        /// <param name="key">入力するキー</param>
        public static void AddKeyboardInput(ref System.Collections.Generic.List<Input> inputs, KeyboardStroke flags, System.Windows.Forms.Keys key)
        {
            int keyboardFlags = (int)flags | KBD_UNICODE;
            short virtualKey = (short)key;
            short scanCode = (short)NativeMethods.MapVirtualKey(virtualKey, 0);

            AddKeyboardInput(ref inputs, keyboardFlags, virtualKey, scanCode, 0, 0);
        }

        /// <summary>
        /// キーボード入力のイベントを追加します。
        /// </summary>
        /// <param name="inputs">入力イベントのリスト</param>
        /// <param name="flags">キーストロークのオプション</param>
        /// <param name="virtualKey">仮想キーコード</param>
        /// <param name="scanCode">キーのハードウェアスキャンコード</param>
        /// <param name="time">ミリ秒単位でのイベントのタイムスタンプ</param>
        /// <param name="extraInfo">キーストロークに関連付けられた付加価値</param>
        public static void AddKeyboardInput(ref System.Collections.Generic.List<Input> inputs, int flags, short virtualKey, short scanCode, int time, int extraInfo)
        {
            Input input = new Input();
            input.Type = 1; // KEYBOARD = 1
            input.Keyboard.Flags = flags;
            input.Keyboard.VirtualKey = virtualKey;
            input.Keyboard.ScanCode = scanCode;
            input.Keyboard.Time = time;
            input.Keyboard.ExtraInfo = extraInfo;

            inputs.Add(input);
        }

        /// <summary>
        /// 入力イベントを実行します。
        /// </summary>
        public static void SendInput(System.Collections.Generic.List<Input> inputs)
        {
            Input[] inputArray = inputs.ToArray();
            SendInput(inputArray);
        }

        /// <summary>
        /// 入力イベントを実行します。
        /// </summary>
        public static void SendInput(Input[] inputs)
        {
            NativeMethods.SendInput(inputs.Length, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        }
    }
}
