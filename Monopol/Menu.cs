using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Monopol
{
    public partial class Menu : Form
    {

        Game game;

        public Menu(ref Game game)
        {
            InitializeComponent();
            this.game = game;
            this.ActiveControl = textBoxPlayerName;
        }

        private void buttonAddPlayer_Click(object sender, EventArgs e)
        {
            addPlayer();
        }

        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            startGame();
        }

        private void addPlayer()
        {
            if (textBoxPlayerName.Text != "" && textBoxPlayerName.Text.Count() < 20)
                game.addPlayer(textBoxPlayerName.Text);
            else
                MessageBox.Show("Ange ett namn, kompis!");

            textBoxPlayerName.Text = "";
        }

        private void startGame()
        {
            if (game.players.Count() > 1)
                this.DialogResult = DialogResult.Yes;
            else
                MessageBox.Show("Minst två spelare");
        }

        private void textBoxPlayerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                addPlayer();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                e.Handled = true;
                startGame();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }


    }
}
