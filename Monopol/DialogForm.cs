using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopol
{
    public partial class DialogForm : Form
    {
        public DialogForm(string text)
        {
            InitializeComponent();
            richTextBox1.Text = text;
            this.ActiveControl = buttonYes;
        }

        public DialogForm(string text, bool ok)
        {
            InitializeComponent();
            richTextBox1.Text = text;
            buttonNo.Hide();
            buttonYes.Text = "OK";
            buttonYes.Location = new Point(this.Width / 2 - (buttonYes.Width / 2), buttonYes.Location.Y);
            this.ActiveControl = buttonYes;
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
