using System;
using System.Drawing;
using System.Windows.Forms;

namespace UiTets
{
    public partial class ViewTetsForm : Form
    {
        public ViewTetsForm()
        {
            InitializeComponent();

            btn_ok.Click += (x, y) =>
            {
                Hide();
            };
        }

        protected override void OnClosed(EventArgs e)
        {
            Hide();
        }

        public void ViewProject(string name,string description)
        {
            page_box.Visible = false;
            page_lable.Visible = false;
            description_lable.Location = page_lable.Location;
            description_box.Location = page_box.Location;
            btn_ok.Location = new Point(btn_ok.Location.X, btn_ok.Location.Y - 30);
            ShowDialog();

        }

        public void ViewTest(string name,string page,string description)
        {
            page_box.Visible = true;
            page_lable.Visible = true;
            description_lable.Location = page_lable.Location;
            description_box.Location = page_box.Location;
            btn_ok.Location = new Point(btn_ok.Location.X, btn_ok.Location.Y - 30);
            test_name_box.Text = name;
            ShowDialog();
        }
    }
}
