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
    public partial class Form1 : Form
    {
        List<PictureBox> playerImages;
        Game game = new Game();
        public Form1()
        {
            InitializeComponent();
            this.ActiveControl = toolStrip1;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            throwDice();
        }

        private void throwDice()
        {
            game.throw_dice();
            if (game.getCurrSpace().GetType() == typeof(Property))
            {
                Property p = (Property)game.getCurrSpace();
                if (p.owner == "")
                {
                    if (DisplayDialog(game.GetCurrPlayer().name + ": \n" + "Vill du köpa " + game.getCurrSpace().name + " för " + p.cost.ToString() + " kr?"))
                        game.GetCurrPlayer().BuyProperty();

                }

            }

            updateStats();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void updateStats()
        {
            statsTextBox.Text = "";
            foreach (Player p in game.players)
			{
                statsTextBox.AppendText(p.name + Environment.NewLine + p.cash + " kr " + Environment.NewLine + Environment.NewLine);
			}

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Menu menu = new Menu(ref game))
            {
                menu.ShowDialog(this);
            }
            game.board_init(ref game.board);
            updateStats();
        }

        private void ResposeButton_Click(object sender, EventArgs e)
        {

            game.GetCurrPlayer().BuyProperty();
        }

        private bool DisplayDialog(string text)
        {
            DialogForm dialog = new DialogForm(text);
            dialog.StartPosition = FormStartPosition.CenterParent;
            return dialog.ShowDialog() == DialogResult.Yes;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                throwDice();
            }
        }

        private void toolStrip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                throwDice();
            }
        }
    }
}
