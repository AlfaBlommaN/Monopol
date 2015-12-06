using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monopol
{
    public class Game
    {

        const int jailpos = 10;
        private int playerturn = 0;
        public List<Player> players = new List<Player>(6);
        public int[] lastThrow { get; private set; }

        public Space[] board = new Space[40];
        XDocument xdoc = new XDocument(new XElement("Root"));

        public Bisyssla currentBisys = new Bisyssla("", 0);
        private List<Bisyssla> bisysslor = new List<Bisyssla>();


        private void NextTurn()
        {
            if (playerturn >= players.Count() - 1)
                playerturn = 0;
            else
                ++playerturn;

            if (players[playerturn].active == false)
                NextTurn();
        }


        public void board_init(ref Space[] board)
        {
            for (uint i = 0; i < board.Length; ++i)
            {
                if (i == 0)
                    board[i] = new Space("Gå");
                else if (i == jailpos)
                    board[i] = new Space("Granntant Åsa");
                else if (i == 20)
                    board[i] = new Space("Fri Parkering");
                else if (i == 30)
                    board[i] = new GoToJail();
                else if (i == 2 || i == 17 || i == 33 || i == 7 || i == 22 || i == 36 || i == 38 || i == 16)
                    board[i] = new BisysSpace();
                else board[i] = new Property("Testgatan " + i.ToString(), 3000);
            }

            for (int i = 0; i < 15; i++)
            {
                Random rnd = new Random();
                bisysslor.Add(new Bisyssla("Testmeddelande " + i.ToString(), rnd.Next(-4000, 0)));
            }

        }

        public int[] throw_dice()
        {
            NextTurn();
            Debug.WriteLine("\n" + players[playerturn].name);
            Debug.WriteLine("Cash: " + players[playerturn].cash.ToString());
            int[] dices = new int[2];
            Random rnd = new Random();
            dices[1] = rnd.Next(1, 7);
            dices[0] = rnd.Next(1, 7);
            players[playerturn].Go(dices[0] + dices[1]);
            Debug.WriteLine("Tärning: " + (dices[0] + dices[1]).ToString());
            Rules.CheckState(players[playerturn], this);
            lastThrow = dices;
            Save();
            return dices;
        }

        public Player GetCurrPlayer()
        {
            return players[playerturn];
        }

        public Space getCurrSpace()
        {
            return board[GetCurrPlayer().position];
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

        public Player findPlayer(string name)
        {
            return players.Find(x => x.name == name);
        }

        public bool checkIfGameOver()
        {
            uint counter = 0;
            foreach (Player p in players)
            {
                if (p.active)
                    counter++;
            }
            return (counter <= 1);
        }

        public void newBisys()
        {
            Random rnd = new Random();
            currentBisys = bisysslor.ElementAt(rnd.Next(0, bisysslor.Count() - 1));
        }

        public void resetBisys()
        {
            currentBisys = new Bisyssla("", 0);
        }

        public void Save()
        {

            XElement playersState = new XElement("Players",

            from player in players
            where player.active == true 
            select new XElement("Player",
                new XAttribute("Name", player.name),
                new XAttribute("Cash", player.cash),
                new XAttribute("Prisoner", player.prisoner),
                new XAttribute("Position", player.position),
                new XAttribute("Icon", player.icon)));

            XElement boardState = new XElement("Board",

            from space in board
            where (space.GetType() == typeof(Property) && ((Property)space).owner != "")  
            select new XElement("Property",
                new XAttribute("Name", ((Property)space).name),
                new XAttribute("Owner", ((Property)space).owner))

            );

            xdoc.Root.Add(new XComment("Information om spelarna"), playersState); 
            xdoc.Root.Add(new XComment("Ändringar i spelbrädet"), boardState);
            xdoc.Save("State.xml");
        }

    }
}
