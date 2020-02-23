using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UiTets
{
    public partial class NewProjectForm : Form
    {
        private Regex _regex = new Regex("^[0-9a-zA-Z]+$");

        public NewProjectForm()
        {
            InitializeComponent();
            btn_ok.Click += (x, y) =>
            {
                Create();
            };
        }

        public Func<string, string, bool> DoCreateProject;

        public void Create()
        {
            if (!_regex.IsMatch(project_name_box.Text))
            {
                MessageBox.Show("Invalid character of project name", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DoCreateProject != null && DoCreateProject(project_name_box.Text, description_box.Text))
            {
                Hide();
            }
            else
            {
                MessageBox.Show("Create failed, projec already exist! ", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
