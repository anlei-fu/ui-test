using System;
using System.Drawing;
using System.Windows.Forms;

namespace UiTets
{
    public   class Serializer
    {
        public EventRecord Deserialize(string text)
        {
            var record = new EventRecord();
            
            var parenthesis = text.IndexOf("(");
            var right = text.IndexOf(")", parenthesis);
            var content = text.Substring(parenthesis, right - parenthesis);
            try
            {
                record.EventType = (EventType)Enum.Parse(typeof(EventType), text.Substring(0, parenthesis));
                text = text.Substring(parenthesis, text.Length - 1 - parenthesis);

                switch (record.EventType)
                {
                    case EventType.KeyDown:
                    case EventType.KeyUp:
                        record.Key = (Keys)Enum.Parse(typeof(Keys), content);
                        break;
                    case EventType.MouseLeftDown:
                    case EventType.MouseLeftUp:
                    case EventType.MouseMiddleDown:
                    case EventType.MouseMiddleUp:
                    case EventType.MouseRightDown:
                    case EventType.MouseRightUp:
                        var segs = content.Split(',');
                        record.Location = new Point(int.Parse(segs[0]), int.Parse(segs[1]));
                        break;
                    case EventType.Sleep:
                        record.Value = int.Parse(content);
                        break;
                    case EventType.Text:
                        record.Text = ParseString(content);
                        break;
                    default:
                        break;
                }
             
            }
            catch
            {
                return null;
            }

            return record;
        }

        public string Serialize(EventRecord record)
        {
            if (record.EventType != EventType.MouseEvent)
                return $"{record.EventType}({record.Key.Serialize()})\r\n";

            return $"{record.EventType}({record.Location.X},{record.Location.Y})\r\n";
        }

        private string ParseString(string str)
        {
            return str.Trim().Substring(1, str.Trim().Length - 2);
        }
    }
}
