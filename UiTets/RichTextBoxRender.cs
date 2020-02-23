using System.Drawing;
using System.Windows.Forms;

namespace UiTets
{
    public class RichTextBoxRender
    {

        private RichTextBox _box;

        private string _lastText = "";

        public RichTextBoxRender(RichTextBox box)
        {
            _box = box;
            _box.TextChanged += (x, y) =>
            {
                Render(_box.Text);
            };
        }

        private void Render(string current)
        {
            var pos = FindChangePost(current);
            var lineStart = FindLineStart(current, pos);
            var lineEnd = FindLineEnd(current, pos);
            var line = current.Substring(lineStart, lineEnd - lineStart);
            if (line.Trim().StartsWith("//"))
            {
                _box.Select(lineStart, lineEnd - lineStart);
                _box.SelectionColor = Color.Green;
                _box.SelectionLength = 0;
                _box.SelectionColor = Color.Black;
            }
            else
            {
                var funcStart = line.IndexOf("(");
                if (funcStart != -1)
                {
                    _box.Select(lineStart, funcStart);
                    _box.SelectionColor = Color.Blue;
                    _box.SelectionLength = 0;
                    _box.SelectionColor = Color.Black;
                }
            }

            _box.ScrollToCaret();
            _lastText = current;

        }

        private int FindChangePost(string current)
        {
            for (int i = 0; i < current.Length; i++)
            {
                if (_lastText.Length <= i || _lastText[i] != current[i])
                    return i;
            }

            return current.Length;
        }

        private int FindLineStart(string current, int pos)
        {
            var start = current.LastIndexOf("\n", pos);
            return start == -1 ? 0 : start;
        }

        private int FindLineEnd(string current, int pos)
        {
            var end = current.IndexOf("\n", pos);

            return end == -1 ? current.Length : end;
        }
    }
}
