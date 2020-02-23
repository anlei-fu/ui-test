using System;
using System.Windows.Forms;

namespace UiTets
{
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
        }

        public  Action<string, string, string> DoEditTest;

        public  Action<string, string> DoEditProject;

        public void EditProject(string name,string description)
        {
            ShowDialog();
        }

        public void EditTest(string name,string page,string description)
        {
            ShowDialog();
        }
    }
}
