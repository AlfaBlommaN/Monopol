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

        const int jailpos = 10;
        private int playerturn = 0;
        public List<Player> players = new List<Player>(6);

        public Space[] board = new Space[40];

        private void NextTurn()
        {
            if (playerturn >= players.Count() - 1)
                playerturn = 0;
            else
                ++playerturn;
        }


        public void board_init(ref Space[] board)
        {
            for (uint i = 0; i < board.Length; ++i)
            {
                if (i == jailpos)
                    board[i] = new Space("Granntant Åsa");
                else if (i == 30)
                    board[i] = new GoToJail();
                else board[i] = new Property(i.ToString(), 1000);
            }

        }

        public int[] throw_dice()
        {
            int[] dices = new int[2];
            Random rnd = new Random();
            dices[1] = rnd.Next(1, 7);
            dices[0] = rnd.Next(1, 7);
            players[playerturn].Go(dices[0] + dices[1]);
            Debug.WriteLine(players[playerturn].name + " slår " + (dices[0] + dices[1]).ToString());
            Rules.CheckState(players[playerturn], this);
            NextTurn();
            return dices;
        }

        public void addPlayer(string name)
        {
            this.players.Add(new Player(name));
            Debug.WriteLine(players.Count() + ": " + name);
        }

        public void BustPlayer(Player player)
        {
            Debug.WriteLine(player.name + " fängslas!!!!");
            player.prisoner = true;
            player.GoTo(jailpos);
        }

    }
}
