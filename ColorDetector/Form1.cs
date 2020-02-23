using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ColorDetector
{
    public partial class FormMain : Form
    {
        private Color _color;
        private int _r;
        private int _g;
        private int _b;
        private int _a=255;
        private bool _isCapturing;
        private MouseHook m_event_hook;
        private KeyboardEventHook k_event_hook;
        private Point _startPoint;
        private Point _endPoint;
        public FormMain()
        {
            InitializeComponent();
            k_event_hook = new KeyboardEventHook();
            k_event_hook.KeyDownEvent += (x, y) =>
            {
                Trace.WriteLine(y.KeyCode);
                if (y.KeyCode == Keys.LControlKey)
                {
                    _isCapturing = true;
                }
               
            };

            k_event_hook.KeyUpEvent += (x, y) =>
            {
                if (y.KeyCode == Keys.LControlKey)
                {
                    _isCapturing = false;
                }
            };

            k_event_hook.Start();

             m_event_hook = new MouseHook();

            m_event_hook.MouseDownEvent += (x, y) =>
            {
                if (_isCapturing)
                {
                    if (y.Button == MouseButtons.Left)
                    {
                        RunCapture(y.Location.X, y.Location.Y);
                    }
                    else if (y.Button == MouseButtons.Middle)
                    {
                        log_box.AppendText($"set start point ${y.Location}\r\n");
                        _startPoint = y.Location;
                    }
                    else
                    {
                        log_box.AppendText($"hirizotol distance is {y.Location.X-_startPoint.X}\r\n" +
                                           $"vertical distance is {y.Location.Y-_startPoint.Y}\r\n");
                    }
                }
            };

            m_event_hook.SetHook();
            track_r.ValueChanged += (x, y) =>
            {
                ChangeColor();

            };

            track_g.ValueChanged += (x, y) =>
            {
                ChangeColor();
            };

            track_b.ValueChanged += (x, y) =>
            {
                ChangeColor();
            };

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChangeColor();

        }

        private void ChangeColor()
        {
            _r = track_r.Value;
            _g = track_g.Value;
            _b = track_b.Value;
            lable_r.BackColor = Color.FromArgb(_r, 0, 0);
            lable_g.BackColor = Color.FromArgb(0, _g, 0);
            lable_b.BackColor = Color.FromArgb(0, 0, _b);
            _color = Color.FromArgb(_a,_r, _g, _b);
            lable_color.BackColor = _color;
            CopyColor();

        }

        private void CopyColor()
        {
            var htmlColorString = ColorTranslator.ToHtml(_color);
            Clipboard.SetText(htmlColorString);
            log_box.AppendText($"color is {htmlColorString}, set into clipboad \r\n");
        }

        private void RunCapture(int x, int y)
        {
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), CopyPixelOperation.SourceCopy);
                }
                x = x < 0 ? 1 : x;
                y = y < 0 ? 1 : y;

                var pixle = bitmap.GetPixel(x, y);
                track_r.Value = pixle.R;
                track_g.Value = pixle.G;
                track_b.Value = pixle.B;
                track_a.Value = pixle.A;
                ChangeColor();
            }
        }

        private void abountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CopyRight().ShowDialog();
        }
    }

    public class Win32Api
    {
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        //安装钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        //调用下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }


    public class MouseHook
    {
        private Point point;
        private Point Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;
                    if (MouseMoveEvent != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e);
                    }
                }
            }
        }
        private int hHook;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;

        public const int WH_MOUSE_LL = 14;
        public Win32Api.HookProc hProc;
        public MouseHook()
        {
            this.point = new Point();
        }
        public int SetHook()
        {
            hProc = new Win32Api.HookProc(MouseHookProc);
            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                MouseButtons button = MouseButtons.None;
                int clickCount = 0;
                switch ((Int32)wParam)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_MBUTTONDOWN:
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        MouseDownEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_LBUTTONUP:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_RBUTTONUP:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_MBUTTONUP:
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        MouseUpEvent?.Invoke(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                }

                this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;


        public delegate void MouseDownHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseDownEvent;

        public delegate void MouseUpHandler(object sender, MouseEventArgs e);
        public event MouseUpHandler MouseUpEvent;
    }

}
