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
    public partial class Menu : Form
    {

        Game game;

        public Menu(ref Game game)
        {
            InitializeComponent();
            this.game = game;
        }

        private void buttonAddPlayer_Click(object sender, EventArgs e)
        {
            if (textBoxPlayerName.Text != "" && textBoxPlayerName.Text.Count() < 20)
                game.addPlayer(textBoxPlayerName.Text);
            else
                MessageBox.Show("Ange ett namn, kompis!");

        }

        private void buttonStartGame_Click(object sender, EventArgs e)
        {
            if (game.players.Count() > 1)
                this.Close();
            else
                MessageBox.Show("Minst två spelare");
        }
    }
}
