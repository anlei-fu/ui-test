using System;

namespace UiTets
{
    public  class EventCapture
    {
        private bool _capturing = false;

        private MouseEventHook _mouseHook;

        private KeyboardEventHook _keyboardHook;

        public EventCapture()
        {
            _mouseHook = new MouseEventHook();
            _keyboardHook = new KeyboardEventHook();

            Init();
        }

        public event Action<EventRecord> OnRecord;

        public bool IsCapturing => _capturing;

        public void StartCapture()
        {
            if (_capturing)
                return;

            _mouseHook.Start();
            _keyboardHook.Start();
            _capturing = true;
        }

        public void StopCapture()
        {
            if (!_capturing)
                return;

            _mouseHook.Stop();
            _keyboardHook.Stop();
            _capturing = false;
        }

        private void Init()
        {
            _keyboardHook.KeyDownEvent += (x, y) =>
            {
                var record = new EventRecord()
                {
                    EventType = EventType.KeyDown,
                    Key = y.KeyCode,
                };

                OnRecord?.Invoke(record);
            };

            _keyboardHook.KeyUpEvent += (x, y) =>
            {
                var record = new EventRecord()
                {
                    EventType = EventType.KeyUp,
                    Key = y.KeyCode,
                };

                OnRecord?.Invoke(record);
            };

            _mouseHook.MouseDownEvent += (x, y) =>
            {
                var record = new EventRecord()
                {
                    Location = y.Location,
                };

                switch (y.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        record.EventType = EventType.MouseLeftDown;
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        record.EventType = EventType.MouseRightDown;
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        record.EventType = EventType.MouseMiddleDown;
                        break;
                    default:
                        return;
                }

                OnRecord?.Invoke(record);
            };

            _mouseHook.MouseUpEvent += (x, y) =>
            {
                var record = new EventRecord()
                {
                    Location = y.Location,
                };

                switch (y.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        record.EventType = EventType.MouseLeftUp;
                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        record.EventType = EventType.MouseRightUp;
                        break;
                    case System.Windows.Forms.MouseButtons.Middle:
                        record.EventType = EventType.MouseMiddleUp;
                        break;
                    default:
                        return;
                }

                OnRecord?.Invoke(record);
            };
        }
    }
}
