using System.Drawing;
using System.Windows.Forms;

namespace UiTest
{
    public  class ClipBoardUtil
    {
        public static void SetText(string text)
        {
            Clipboard.SetText(text);
        }

        public static void SetImage(Image img)
        {

        }
    }
}
