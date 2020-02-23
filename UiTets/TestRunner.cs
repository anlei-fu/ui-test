using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UiTets
{
    public class TestRunner
    {
        private readonly object _lock = new object();

        private bool _stop = true;

        private bool _realStopped = false;

        private EventSender _eventSender = new EventSender();

        public bool IsRunning => !_stop && !_realStopped;

        public void Run(List<EventRecord> records)
        {
            lock (_lock)
            {
                if (!_stop)
                    return;

                _stop = false;
            }

            Task.Run(() =>
            {
                foreach (var item in records)
                {
                    lock (_lock)
                    {
                        if (_stop)
                        {
                            StopFinal();
                            return;
                        }

                    }

                    Thread.Sleep(item.Value);
                    switch (item.EventType)
                    {
                        case EventType.KeyDown:
                            _eventSender.SendKeyDown(item.Key);
                            break;
                        case EventType.KeyUp:
                            _eventSender.SendKeyUp(item.Key);
                            break;
                        case EventType.MouseLeftDown:
                            _eventSender.SendMouseLeftDonw(item.Location);
                            break;
                        case EventType.MouseLeftUp:
                            _eventSender.SendMouseLeftUp(item.Location);
                            break;
                        case EventType.MouseMiddleDown:
                            _eventSender.SendMouseMiddleDown(item.Location);
                            break;
                        case EventType.MouseMiddleUp:
                            _eventSender.SendMouseMiddleUp(item.Location);
                            break;
                        case EventType.MouseRightDown:
                            _eventSender.SendMouseRightDown(item.Location);
                            break;
                        case EventType.MouseRightUp:
                            _eventSender.SendMouseRightUp(item.Location);
                            break;
                        default:
                            break;
                    }
                }
            });
        }

        public void Stop()
        {
            lock (_lock)
            {
                _stop = true;
            }

            while (!_realStopped) ;
            _realStopped = false;
        }

        private void StopFinal()
        {
            foreach(var item in Enum.GetValues(typeof(Keys)))
            {
                _eventSender.SendKeyUp((Keys)item);
            }
            _realStopped = true;
        }
    }
}
