using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UiTets
{
    public partial class NewTestForm : Form
    {

        private Regex _regex = new Regex("^[0-9a-zA-Z]+$");

        public NewTestForm()
        {
            InitializeComponent();

            btn_ok.Click += (x, y) =>
            {
                Create();
            };
        }

        public Func<string, string, string, bool> DoCreateTest;

        public void Create()
        {
            if (!_regex.IsMatch(test_name_box.Text))
            {
                MessageBox.Show("Invalid character of test name", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DoCreateTest != null && DoCreateTest(test_name_box.Text, page_box.Text, description_box.Text))
            {
                Hide();
            }
            else
            {
                MessageBox.Show("Create failed, test already exist! ", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
