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
    public partial class SellForm : Form
    {
        

        Game game;
        public SellForm(ref Game game_)
        {
            InitializeComponent();
            this.game = game_;
            foreach (Player player in game.players)
            {
                if (player.name != game.GetCurrPlayer().name)
                    listBoxPlayers.Items.Add(player.name);
            }

            foreach (Space s in game.board)
            {
                if (s.GetType() == typeof(Property))
                {
                    Property p = (Property)s;
                    if (p.owner == game.GetCurrPlayer().name)
                        listBoxProperty.Items.Add(p.name);
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxPrice.Text.All(char.IsDigit) && textBoxPrice.Text != "" && listBoxPlayers.SelectedIndex > -1 && listBoxProperty.SelectedIndex > -1)
            {
                game.findPlayer(listBoxPlayers.SelectedItem.ToString()).AddQuery(new BuySellQuery(game.GetCurrPlayer().name, BuyOrSell.Sell, Int32.Parse(textBoxPrice.Text), listBoxProperty.Text));
                this.Close();
            }

        }
    }
}
