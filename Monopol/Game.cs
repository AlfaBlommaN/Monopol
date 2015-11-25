using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class Game
    {

        public List<Player> players = new List<Player>(6);

        public Space[] board = new Space[40];


        public void board_init(ref Space[] board)
        {
            for (uint i = 0; i < board.Length; ++i)
            {
                board[i] = new Property(i.ToString(), 1000);
            }

        }

        public int[] throw_dice()
        {
            int[] dices = new int[2];
            Random rnd = new Random();
            dices[1] = rnd.Next(1, 7);
            dices[0] = rnd.Next(1, 7);
            players[0].Go(dices[0] + dices[1]);
            return dices;
        }

        public void addPlayer(string name)
        {
            this.players.Add(new Player(name));
            Debug.WriteLine(players.Count() + ": " + name);
        }
    }
}
