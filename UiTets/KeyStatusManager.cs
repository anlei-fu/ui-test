using System.Collections.Concurrent;
using System.Diagnostics;
using System.Windows.Forms;

namespace UiTest
{
    public  class KeyStatusManager
    {
        private ConcurrentDictionary<Keys, KeyStatus> _keyStatus = new ConcurrentDictionary<Keys, KeyStatus>();

        public void SetKeyUp(Keys key)
        {
            Trace.WriteLine($"key {key} up");
            PutIfAbsent(key);
            _keyStatus[key] = KeyStatus.Up;
        }

        public void SetKeyDown(Keys key)
        {
            Trace.WriteLine($"key {key} down");
            PutIfAbsent(key);
            _keyStatus[key] = KeyStatus.Down;
        }

        private void PutIfAbsent(Keys key)
        {
            _keyStatus.TryAdd(key, KeyStatus.Down);
        }

        public bool IsAllDown(params Keys[] keys)
        {
            foreach (var key in keys)
            {
                if (!_keyStatus.ContainsKey(key)||_keyStatus[key] != KeyStatus.Down)
                    return false;
            }

            return true;
            
        }
    }
}
