using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UiTest
{
    public class TestRunner
    {
        private readonly object _lock = new object();

        private readonly object _pauseLock = new object();

        private bool _stop = true;

        private bool _pause = false;

        private bool _realStopped = false;

        private EventEmitter _eventSender = new EventEmitter();

        public bool IsRunning => !_stop && !_realStopped;

        public Action<string> Printer;

        public Action<string> Infoer;

        public Action<string> SnapShoter;

        public Action<string> SetClipBoard;

        public event Action<string> Finished;

        public void Run(Project project)
        {

            lock (_lock)
            {
                if (!_stop)
                    return;

                _stop = false;
            }

            Task.Run(() =>
            {
                Thread.Sleep(100);
                foreach (var item in project.Commands)
                {

                    lock (_lock)
                    {
                        if (_pause)
                        {
                            Monitor.Wait(_pauseLock, int.MaxValue);
                        }

                        if (_stop)
                        {
                            break;
                        }
                    }

                    switch (item.CommandType)
                    {
                        case CommandType.CLICK:
                           _eventSender.SendMouseLeftDonw(item.Location.Cast());
                           _eventSender.SendMouseLeftUp(item.Location.Cast());
                            break;
                        case CommandType.DOUBEL_CLICK:
                            _eventSender.SendMouseLeftDonw(item.Location.Cast());
                            _eventSender.SendMouseLeftUp(item.Location.Cast());
                            _eventSender.SendMouseLeftDonw(item.Location.Cast());
                            _eventSender.SendMouseLeftUp(item.Location.Cast());
                            break;
                        case CommandType.PRINT:
                            Printer?.Invoke((string)item.Value);
                            break;
                        case CommandType.WAIT:
                            Thread.Sleep((int)((long)item.Value));
                            break;
                        case CommandType.SCROLL_UP:
                            break;
                        case CommandType.SCROLL_DOWN:
                            break;
                        case CommandType.TYPE:
                            SetClipBoard?.Invoke((string)item.Value);
                            _eventSender.SendKeyDown(Keys.LControlKey);
                            _eventSender.SendKeyDown(Keys.A);
                            _eventSender.SendKeyUp(Keys.A);
                            _eventSender.SendKeyDown(Keys.V);
                            _eventSender.SendKeyUp(Keys.LControlKey);
                            _eventSender.SendKeyUp(Keys.V);
                            break;
                        case CommandType.KEY_DOWN:
                            _eventSender.SendKeyDown((Keys)item.Value);
                            break;
                        case CommandType.KEY_UP:
                            _eventSender.SendKeyUp((Keys)item.Value);
                            break;
                        case CommandType.MOUSE_DOWN:
                            break;
                        case CommandType.MOUSE_UP:
                            break;
                        case CommandType.LEFT_MOUSE_DOWN:
                            _eventSender.SendMouseLeftDonw(item.Location.Cast());
                            break;
                        case CommandType.LEFT_MOUSE_UP:
                            _eventSender.SendMouseLeftUp(item.Location.Cast());
                            break;
                        case CommandType.MIDDLE_MOUSE_DOWN:
                            _eventSender.SendMouseMiddleDown(item.Location.Cast());
                            break;
                        case CommandType.MIDDLE_MOUSE_UP:
                            _eventSender.SendMouseMiddleUp(item.Location.Cast());
                            break;
                        case CommandType.RIGHT_MOUSE_DOWN:
                            _eventSender.SendMouseRightDown(item.Location.Cast());
                            break;
                        case CommandType.RIGHT_MOUSE_UP:
                            _eventSender.SendMouseRightUp(item.Location.Cast());
                            break;
                        case CommandType.MOVE:
                            break;
                        case CommandType.INFO:
                            Infoer?.Invoke((string)item.Value);
                            break;
                        case CommandType.SNAPSHOT:
                            SnapShoter?.Invoke((string)item.Value);
                            break;
                        case CommandType.PAUSE:
                            Pause();
                            break;
                        default:
                            break;
                    }
                }

                Stop();
                Finished?.Invoke(project.Name);
            });

           
        }

        public void Stop()
        {
            lock (_lock)
            {
                StopFinal();

                _stop = true;
                Monitor.PulseAll(_lock);
            }
        }

        public void Pause()
        {
            lock (_lock)
            {
                if (!_pause)
                {
                    _pause = true;
                }
            }
        }

        public void Resume()
        {
            lock (_lock)
            {
                if (_pause)
                {
                    _pause = false;
                    Monitor.PulseAll(_pauseLock);
                }
            }
        }

        private void StopFinal()
        {
            foreach(var item in Enum.GetValues(typeof(Keys)))
            {
                //_eventSender.SendKeyUp((Keys)item);
            }
        }
    }
}
