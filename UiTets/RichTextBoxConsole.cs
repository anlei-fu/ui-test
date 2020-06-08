using System.Drawing;
using System.Windows.Forms;

namespace UiTest
{
    public class RichTextBoxConsole
    {

        private readonly RichTextBox _box;

        public RichTextBoxConsole(RichTextBox box)
        {
            _box = box;
        }

        public Color ForeColor { get; set; } = Color.WhiteSmoke;

        public Color BackColor { get => _box.BackColor; set { _box.BackColor = value; } }

        public void Info(string msg)
        {
            AddCore(Color.Green, "Info", msg);
        }

        public void Warning(string msg)
        {
            AddCore(Color.Yellow, "Warning", msg);
        }

        public void Error(string msg)
        {
            AddCore(Color.Red, "Error", msg);
        }

        public void Log(string msg)
        {
            _box.InvokeAfterHandleCreated(() =>
            {
                _box.Text += $"{msg}\r\n";
            });
            
        }

        private void AddCore(Color color, string title, string msg)
        {
            _box.InvokeAfterHandleCreated(() =>
            {
               // _box.ForeColor = color;
                _box.Text += title;
             //   _box.ForeColor = ForeColor;
                _box.Text += $":{msg}\r\n";
            });
        }

        public void Clear()
        {
            _box.Clear();
        }

    }
}
