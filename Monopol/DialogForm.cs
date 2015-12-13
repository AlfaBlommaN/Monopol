using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monopol
{
    public partial class DialogForm : Form
    {
        private bool isAI;
        public DialogForm(string text, bool isAI)
        {
            InitializeComponent();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Text = text;
            this.ActiveControl = buttonYes;
            this.isAI = isAI;
        }

        public DialogForm(string text, bool ok, bool isAI)
        {
            InitializeComponent();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox1.Text = text;
            buttonNo.Hide();
            buttonYes.Text = "OK";
            buttonYes.Location = new Point(this.Width / 2 - (buttonYes.Width / 2), buttonYes.Location.Y);
            this.ActiveControl = buttonYes;
            this.isAI = isAI;

        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private async void DialogForm_Shown(object sender, EventArgs e)
        {
            if (isAI)
            {
                this.buttonNo.Enabled = false;
                this.buttonYes.Enabled = false;
                Debug.WriteLine("Hej nu ska vi vänta!");
                await Task.Delay(50);
                this.Close();
            }
        }

    }
}
