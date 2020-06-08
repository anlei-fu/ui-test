using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace UiTest
{
    /// <summary>
    /// Send mouse event
    /// </summary>
    internal class MouseEventSender
    {
        enum MouseEventFlag : uint
        {
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);

        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);

        public void SendMouseLeftDonw(System.Drawing.Point location)
        {
           MouseEvent(location,MouseEventFlag.LeftDown);
        }

        public void SendMouseLeftUp(System.Drawing.Point location)
        {
            MouseEvent(location, MouseEventFlag.LeftUp);
        }

        public void SendMouseMiddleDown(System.Drawing.Point location)
        {
            MouseEvent(location, MouseEventFlag.MiddleDown);
        }

        public void SendMouseMiddleUp(System.Drawing.Point location)
        {
            MouseEvent(location, MouseEventFlag.MiddleUp);
        }

        public void SendMouseRightDown(System.Drawing.Point location)
        {
            MouseEvent(location, MouseEventFlag.RightDown);
        }

        public void SendMouseRightUp(System.Drawing.Point location)
        {
            MouseEvent(location, MouseEventFlag.RightDown);
        }

        private void MouseEvent(System.Drawing.Point location, MouseEventFlag flag)
        {
            SetCursorPos(location.X, location.Y);
            mouse_event(flag, 0, 0, 0, UIntPtr.Zero);
        }

    }

    internal class KeyBoardEventSender
    {
        [DllImport("USER32.DLL")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);  //导入模拟键盘的方法


        public void SendKeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, 0);
        }

        public void SendKeyUp(Keys key)
        {
            keybd_event((byte)key, 0, 2, 0);
        }
    }

    /// <summary>
    /// Send event
    /// </summary>
    public class EventEmitter
    {
        private MouseEventSender _mouseEventSender = new MouseEventSender();

        private KeyBoardEventSender _keyBoardEventSender = new KeyBoardEventSender();

        public void SendKeyUp(Keys key)
        {
            _keyBoardEventSender.SendKeyUp(key);
        }

        public void SendKeyDown(Keys key)
        {
            _keyBoardEventSender.SendKeyDown(key);
        }

        public void SendMouseLeftDonw(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseLeftDonw(location);
        }

        public void SendMouseLeftUp(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseLeftUp(location);
        }

        public void SendMouseMiddleDown(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseMiddleDown(location);
        }

        public void SendMouseMiddleUp(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseMiddleUp(location);
        }

        public void SendMouseRightDown(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseRightDown(location);
        }

        public void SendMouseRightUp(System.Drawing.Point location)
        {
            _mouseEventSender.SendMouseRightUp(location);
        }
    }
}
