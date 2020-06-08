using System;
using System.Drawing;
using System.Windows.Forms;

namespace UiTest
{
    public  class EventCapture
    {
        private MouseEventHook _mouseHook;

        private KeyboardEventHook _keyboardHook;

        private KeyStatusManager _keyStatusManager = new KeyStatusManager();

        public EventCapture()
        {
            _mouseHook = new MouseEventHook();
            _keyboardHook = new KeyboardEventHook();

            Init();
        }

        /// <summary>
        /// LControl + Alt + P
        /// </summary>
        public event Action Pause;

        /// <summary>
        /// LControl + Alt +  R
        /// </summary>
        public event Action Resume;

        /// <summary>
        /// LControl + Alt + S
        /// </summary>
        public event Action Stop;

        /// <summary>
        /// LControl + Alt + B
        /// </summary>
        public event Action Start;

        /// <summary>
        /// LControl + Alt + Q
        /// </summary>
        public event Action Restart;

        /// <summary>
        /// LControl + Alt + Mouse Left
        /// </summary>
        public event Action<System.Drawing.Point> Position;

        public bool IsCapturing { get; private set; } = false;

        public void StartCapture()
        {
            if (IsCapturing)
                return;

            _mouseHook.Start();
            _keyboardHook.Start();
            IsCapturing = true;
        }

        public void StopCapture()
        {
            if (!IsCapturing)
                return;

            _mouseHook.Stop();
            _keyboardHook.Stop();
            IsCapturing = false;
        }

        private void Init()
        {
            _keyboardHook.KeyDownEvent += (x, y) =>
            {
                _keyStatusManager.SetKeyDown(y.KeyCode);

                if (y.KeyCode == Keys.S && _keyStatusManager.IsAllDown(Keys.LControlKey, Keys.LMenu))
                    Stop?.Invoke();

                if (y.KeyCode == Keys.B && _keyStatusManager.IsAllDown(Keys.LControlKey, Keys.LMenu))
                    Start?.Invoke();

                if (y.KeyCode == Keys.P && _keyStatusManager.IsAllDown(Keys.LControlKey, Keys.LMenu))
                   Pause?.Invoke();

                if (y.KeyCode == Keys.R && _keyStatusManager.IsAllDown(Keys.LControlKey, Keys.LMenu))
                    Resume?.Invoke();

                if (y.KeyCode == Keys.Q && _keyStatusManager.IsAllDown(Keys.LControlKey, Keys.LMenu))
                    Restart?.Invoke();
            };

            _keyboardHook.KeyUpEvent += (x, y) =>
            {
                _keyStatusManager.SetKeyUp(y.KeyCode);
            };

            _mouseHook.MouseDownEvent += (x, y) =>
            {
                if (y.Button == MouseButtons.Left && _keyStatusManager.IsAllDown(Keys.LControlKey,Keys.LMenu))
                    Position?.Invoke(y.Location);
            };
        }
    }
}
