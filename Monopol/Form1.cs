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
using System.Windows;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Monopol
{
    public partial class Form1 : Form
    {
        List<Bitmap> icons = new List<Bitmap>();

        Game game = new Game();
        Graphics graphics;
        Bitmap background = new Bitmap(global::Monopol.Properties.Resources.Board, new Size(685, 685));
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
            toolStripButton2.Enabled = true;
            game.throw_dice();
            DisplayMessage(game.GetCurrPlayer().name + " slår " + game.lastThrow.Sum());
            
            UpdateGraphics();
            if (game.getCurrSpace().GetType() == typeof(Property))
            {
                Property p = (Property)game.getCurrSpace();
                if (p.owner == "")
                {
                    if (DisplayDialog(game.GetCurrPlayer().name + ": \n" + "Vill du köpa " + game.getCurrSpace().name + " för " + p.cost.ToString() + " kr?"))
                    {
                        if (game.GetCurrPlayer().cash >= p.cost)
                            game.GetCurrPlayer().BuyProperty();
                        else
                            DisplayMessage("Du har inte råd.");
                    }
                }
                else if (p.owner != game.GetCurrPlayer().name)
                {
                    DisplayMessage(game.GetCurrPlayer().name + " hyr " + game.getCurrSpace().name + " och betalar " + p.rent.ToString() + " kr i hyra till " + p.owner);
                }

            }

            if (game.currentBisys.message != "")
            {
                if (game.currentBisys.value < 0)
                    DisplayMessage("Bisyssla!\n" + game.GetCurrPlayer().name + " " + game.currentBisys.message + "\nDu förlorar " + game.currentBisys.value.ToString().Remove(0, 1) + " kr.");
                else
                    DisplayMessage("Bisyssla!\n" + game.GetCurrPlayer().name + " " + game.currentBisys.message + "\nDu cashar in " + game.currentBisys.value.ToString() + " kr.");
                game.resetBisys();
            }

            updateStats();
            if (game.checkIfGameOver())
            {
                DisplayMessage("Spelet är över!");
                endGame();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            using (SellForm sellForm = new SellForm(ref game))
            {
                sellForm.ShowDialog(this);
            }
        }

        private void updateStats()
        {
            statsTextBox.Text = "";
            foreach (Player p in game.players)
            {
                if (p.active)
                {
                    statsTextBox.SelectionColor = p.color;
                    statsTextBox.AppendText(p.name + Environment.NewLine);
                    statsTextBox.SelectionColor = System.Drawing.Color.Black;
                    statsTextBox.AppendText(p.cash + " kr " + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    statsTextBox.SelectionColor = System.Drawing.Color.Gray;
                    statsTextBox.AppendText(p.name + Environment.NewLine + p.cash + " kr " + Environment.NewLine + Environment.NewLine);
                    statsTextBox.SelectionColor = System.Drawing.Color.Black;
                }

            }

        }


        private void UpdateGraphics()
        {
            updateStats();
            graphics = pictureBox1.CreateGraphics();
            graphics.DrawImage(background, 0, 0);
            Point gfxPos = new Point(0, 0);

            for (int i = 0; i < game.players.Count(); i++)
            {
                if (game.players[i].active)
                {
                    if (game.players[i].position == 0)
                        gfxPos = new Point(44 + i * 2, 38 + i * 2);

                    else if (game.players[i].position <= 10)
                        gfxPos = new Point(100 + (game.players[i].position - 1) * 57 + (i * 2), 10 + (i * 2));

                    else if (game.players[i].position > 10 && game.players[i].position <= 20)
                        gfxPos = new Point((i * 2) + 630, 100 + (game.players[i].position - 11) * 57 + (i * 2));

                    else if (game.players[i].position > 20 && game.players[i].position <= 30)
                        gfxPos = new Point(680 - ((game.players[i].position - 19) * 57 + (i * 2)), 602 + (i * 2));

                    else if (game.players[i].position > 30)
                        gfxPos = new Point(10 + (i * 2), 660 - ((game.players[i].position - 29) * 57 + (i * 2)));

                    graphics.FillEllipse(new SolidBrush(game.players[i].color), new Rectangle(new Point(gfxPos.X - 3, gfxPos.Y - 3), new Size(35, 35)));
                    graphics.DrawImage(icons[i], gfxPos);
                }
            }
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

        private bool DisplayMessage(string text)
        {
            DialogForm dialog = new DialogForm(text, true);
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

        private void endGame()
        {
            this.Close();
        }

        private void pictureBox1_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            UpdateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Menu menu = new Menu(ref game);
            menu.StartPosition = FormStartPosition.CenterParent;
            if (!(menu.ShowDialog(this) == DialogResult.Yes))
                Application.Exit();

            game.board_init(ref game.board);
            foreach (Player p in game.players)
            {
                icons.Add(new Bitmap(global::Monopol.Properties.Resources.west, 30, 30));
            }
            updateStats();
            UpdateGraphics();
        }



    }
}
