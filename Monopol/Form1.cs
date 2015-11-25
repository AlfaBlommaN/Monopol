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

        Game game = new Game();
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           game.throw_dice();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Menu menu = new Menu(ref game))
            {
                menu.ShowDialog(this);
            }
            game.board_init(ref game.board);
        }

        private void ResposeButton_Click(object sender, EventArgs e)
        {
            game.GetCurrPlayer().BuyProperty();                
        }
    }
}
