using System;
using System.Windows.Forms;

namespace UiTets
{
    public partial class ReNameForm : Form
    {
        public ReNameForm()
        {
            InitializeComponent();

        }

        public Action<String> OnNewName;

        protected override void OnClosed(EventArgs e)
        {
            Hide();
        }
    }
}
