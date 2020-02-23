using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace UiTets
{
    public class EventRecorder
    {
        private List<EventRecord> _records = new List<EventRecord>();

        private Dictionary<Keys, KeyStatus> _keyStatus = new Dictionary<Keys, KeyStatus>();

        private DateTime _lastRecordTime;

        public EventRecorder()
        {
            foreach (var item in Enum.GetValues(typeof(Keys)))
            {
                if (!_keyStatus.ContainsKey((Keys)item))
                    _keyStatus.Add((Keys)item, KeyStatus.Up);
            }

        }

        public void AddEvent(EventRecord record)
        {
            record.Value = (int)(DateTime.Now - _lastRecordTime).TotalMilliseconds;
            _records.Add(record);

            if (record.EventType == EventType.KeyEvent)
            {
                if (record.EventType == EventType.KeyDown)
                {
                    _keyStatus[record.Key] = KeyStatus.Down;
                }
                else
                {
                    _keyStatus[record.Key] = KeyStatus.Up;
                }
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return _keyStatus[key] == KeyStatus.Down;
        }

    }
}
