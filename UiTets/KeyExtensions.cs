using System;
using System.Windows.Forms;

namespace UiTets
{
    public static class KeyExtensions
    {
        public static string Serialize(this Keys key)
        {
            return key == Keys.Oemcomma ? "comma" : key.ToString();
        }

        public static Keys Deserialize(this string text)
        {
            text=text.Trim();
            return text == "comma" ? Keys.Oemcomma : (Keys)Enum.Parse(typeof(Keys), text);
        }
    }
}
