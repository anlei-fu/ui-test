using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace UiTest
{
    public  class SnapShoter
    {
        private string _outputFolder;
        public SnapShoter(string outputFolder)
        {
            _outputFolder = outputFolder;
        }
        public string SnapShot()
        {
            var fileName = DateTime.Now.ToString("yyyyMMddhhmmss");
            using (Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height), CopyPixelOperation.SourceCopy);
                    var font = SystemFonts.DefaultFont;
                    font = new Font(font.FontFamily, 16);
                    g.DrawString("Jasmine Ui Tester",font,new SolidBrush(Color.FromArgb(255,0,0,0)),new System.Drawing.Point(Screen.PrimaryScreen.Bounds.Width-300, Screen.PrimaryScreen.Bounds.Height-100));
                }

                bitmap.Save($"{_outputFolder}/{fileName}.png", ImageFormat.Png);
            }

            return $"{_outputFolder}/{fileName}.png";
        }
    }
}
