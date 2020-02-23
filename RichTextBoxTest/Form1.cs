using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RichTextBoxTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            richTextBox1.Text = "hello word";
            var length = richTextBox1.Text.Length;
            richTextBox1.Select(0, richTextBox1.Text.Length);
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.Text += " jasmin";
            richTextBox1.Select(length, richTextBox1.Text.Length - length);
            richTextBox1.SelectionColor = Color.Blue;
            richTextBox1.SelectionLength = 0;

        }
    }
}
