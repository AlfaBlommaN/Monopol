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
using System.IO;

namespace Monopol
{
    public partial class Form1 : Form
    {
        FileSystemWatcher watcher = new FileSystemWatcher("Data\\", "*.xml");

        Bitmap icon = new Bitmap(global::Monopol.Properties.Resources.west, 30, 30);

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


        /// <summary>
        /// Det som skall hända när spelaren trycker på slå tärning-knappen.
        /// Denna funktion kallar på diverse funktioner i spellogiken beroende på
        /// vilka beslut en spelare gör med olika pop-up rutor i GUI.
        /// </summary>
        private void throwDice()
        {
            toolStripButton2.Enabled = true;
            buttonDice.Text = "Nästa spelare";
            game.ThrowDice();
            DisplayMessage(game.GetCurrPlayer().name + " slår " + game.lastThrow.Sum());

            UpdateGraphics();   // Uppdatera grafiken en gång när spelaren flyttat på sig.
            updateStats();

            while (game.GetCurrPlayer().pendingQueries.Count > 0)
            {
                BuySellQuery query = game.GetCurrPlayer().pendingQueries.Dequeue();
                if (query.type == BuyOrSell.Sell)
                {
                    game.GetCurrPlayer().SetDecision(DisplayDialog("Vill du köpa " + query.property + " av " + query.sender + " för " + query.offer + "?"));
                    if (game.GetCurrPlayer().GetDecision(query.offer))
                    {
                        if (game.GetCurrPlayer().AcceptQuery(query, game))
                            UpdateGraphics();
                        else
                            DisplayMessage("Du har inte råd.");
                    }
                }
            }

            if (game.getCurrSpace() is Property)
            {
                Property p = (Property)game.getCurrSpace();
                if (p.owner == "")
                {
                    game.GetCurrPlayer().SetDecision(DisplayDialog(game.GetCurrPlayer().name + ": \n" + "Vill du köpa " + game.getCurrSpace().name + " för " + p.cost.ToString() + " kr?"));
                    if (game.GetCurrPlayer().GetDecision(p.cost))
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

            if (game.GetCurrPlayer().prisoner)
            {
                game.GetCurrPlayer().SetDecision(DisplayDialog(game.GetCurrPlayer().name + " är och har en väldigt mysig fika hos Granntant Åsa, men plöstsligt så välter du hennes whiskeyskåp och hon blir skogstokig! Du måste betala 2000 kr."));
                if (game.GetCurrPlayer().GetDecision(2000))
                    game.GetCurrPlayer().GetOutOfJail();
            }

            if (!game.GetCurrPlayer().active)
            {
                DisplayMessage(game.GetCurrPlayer().name + " har gått i pension och är inte längre med i spelet.");
            }

            UpdateGraphics();   // Uppdatera grafiken igen när spelaren har gjort sina beslut.
            updateStats();      // Visa nya värden i statistikrutan

            if (game.checkIfGameOver())
            {
                var remaining = from p in game.players
                                   where p.active == true
                                   select p;
                DisplayMessage("Spelet är över!\n" + remaining.Single().name + " har vunnit!");
                endGame();
                return;
            }

            game.Save(watcher); // Eftersom ändringar i spelet har skett, sparas spelet till Data/State.xml

            if (game.GetCurrPlayer() is AI)
                throwDice();
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            using (SellForm sellForm = new SellForm(ref game))
            {
                sellForm.ShowDialog(this);
            }
            toolStripButton2.Enabled = false;
        }


        /// <summary>
        /// Uppdaterar informationen som visas i textrutan till höger om spelpanen.
        /// </summary>
        private void updateStats()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

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

            Control.CheckForIllegalCrossThreadCalls = true;
        }


        /// <summary>
        /// Sköter all grafik som inte är uppbyggd av vanliga forms-objekt.
        /// Dvs spelplanen, spelares ikoner och indikatorer för vem som äger en gata
        /// </summary>
        private void UpdateGraphics()
        {
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
                    graphics.DrawImage(icon, gfxPos);
                    DrawOwnerRectangle(game.players[i]);
                }
            }
        }

        /// <summary>
        /// Funktion som ritar ut en färgad rektangel runt de gator som ägs av en specifik spelare.
        /// Används av UpdateGraphics-funktionen.
        /// </summary>
        /// <param name="p">Spelare</param>
        private void DrawOwnerRectangle(Player p)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < game.board.Count(); ++i)
            {
                if (game.board[i].GetType() == typeof(Property) && ((Property)game.board[i]).owner == p.name)
                {
                    indexes.Add(i - 1);
                }
            }

            foreach (int i in indexes)
            {
                Rectangle r = new Rectangle(0, 0, 0, 0);
                if (i < 10)
                    r = new Rectangle(new Point(86 + (i * 57), 0), new Size(57, 85));
                else if (i >= 10 && i < 20)
                    r = new Rectangle(new Point(599, (i - 10) * 57 + 85), new Size(85, 57));
                else if (i >= 20 && i < 30)
                    r = new Rectangle(new Point(599 - ((i - 19) * 57), 598), new Size(57, 85));
                else if (i >= 30)
                    r = new Rectangle(new Point(0, 598 - ((i - 29) * 57)), new Size(85, 57));

                graphics.DrawRectangle(new Pen(p.color, 3), r);
            }
        }


        private void ResposeButton_Click(object sender, EventArgs e)
        {
            game.GetCurrPlayer().BuyProperty();
        }


        /// <summary>
        /// En av två funktioner som använder sig av DialogForm-klassen för att visa ett meddelande.
        /// DisplayDialog låter spelaren trycka ja eller nej för att göra ett beslut baserat på meddelandet som skrivs ut.
        /// Används t.ex. vid köp av egendom.
        /// </summary>
        /// <param name="text">Meddelande</param>
        /// <returns></returns>
        private bool DisplayDialog(string text)
        {
            DialogForm dialog = new DialogForm(text, game.GetCurrPlayer() is AI);
            dialog.StartPosition = FormStartPosition.CenterParent;
            return dialog.ShowDialog() == DialogResult.Yes;
        }

        /// <summary>
        /// En av två funktioner som använder sig av DialogForm-klassen för att visa ett meddelande.
        /// DisplayMessage visar ett meddelande. Spelaren kan bara trycka på OK.
        /// Används t.ex. när man får ett meddelande om bisysslor.
        /// </summary>
        /// <param name="text">Meddelande</param>
        /// <returns></returns>
        private bool DisplayMessage(string text)
        {
            DialogForm dialog = new DialogForm(text, true, game.GetCurrPlayer() is AI);
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
            watcher.Changed += new FileSystemEventHandler(WatcherChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Menu menu = new Menu(ref game);
            menu.StartPosition = FormStartPosition.CenterParent;
            var result = menu.ShowDialog(this);
            if (result == DialogResult.Yes)
            {
                game.LoadInitData();
                updateStats();
                UpdateGraphics();
            }
            else if (result == DialogResult.OK)
            {
                game.LoadInitData();
                game.LoadState(watcher);
                updateStats();
                UpdateGraphics();
            }
            else
                Application.Exit();


        }

        /// <summary>
        /// Funktion som kallas när watcher har upptäckt att filer i Datamappen har ändrats.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WatcherChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            game.LoadState(watcher);
            UpdateGraphics();
            updateStats();
            DisplayMessage("Filer har ändrats!\nSpelet har uppdaterats till senaste version");
        }



    }
}
