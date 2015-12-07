using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Monopol
{
    public class Game
    {
        private Color[] colors = new Color[] {Color.Blue, Color.Red, Color.Purple, Color.Orange, Color.DarkGreen, Color.Black};
        const int jailpos = 10;
        private int playerturn = 0;
        public List<Player> players = new List<Player>(6);
        public int[] lastThrow { get; private set; }

        public Space[] board = new Space[40];

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
            board[0] = new Space("Gå");
            board[1] = new Property("SU00", 1000);
            board[2] = new Property("SU01", 1001);
            board[3] = new Property("SU02", 1002);
            board[4] = new Property("SU03", 1003);
            board[5] = new BisysSpace();
            board[6] = new Property("Skäggetorp", 2000);
            board[7] = new BisysSpace();
            board[8] = new Property("Valla", 2000);
            board[9] = new Property("Ryd", 2000);
            board[10] = new Space("Granntant Åsa");

            board[11] = new Property("Spinkiga Stures Sportbar", 4000);
            board[12] = new Property("Joddlande Jörgens Jazzklubb", 2500);
            board[13] = new Property("Panka Pekkas Pantbank", 2500);
            board[14] = new Property("Girige Görans Gym", 4000);
            board[15] = new BisysSpace();
            board[16] = new Property("Rådans Järnvägsspår", 2500);
            board[17] = new BisysSpace();
            board[18] = new Property("De vises kotte", 5);
            board[19] = new Property("Rådans vasbutik", 6000);
            board[20] = new Space("Akademisk kvart");

            board[21] = new Property("Ågatan", 4500);
            board[22] = new Property("Kinda kanal", 5000);
            board[23] = new Property("Lennarts Möbler AB", 4900);
            board[24] = new Property("Axels Tempel", 4000);
            board[25] = new BisysSpace();
            board[26] = new Property("Lambohov", 6000);
            board[27] = new BisysSpace();
            board[28] = new Property("Mor Perbergs Svartklubb", 5000);
            board[29] = new Property("Tandläkare Tures Turistbyrå", 3000);
            board[30] = new GoToJail();

            board[31] = new Property("Professorns kontor", 5000);
            board[32] = new Property("Professorns bibliotek", 7000);
            board[33] = new Property("G-huset", 3000);
            board[34] = new Property("Professorns datorrum", 6000);
            board[35] = new BisysSpace();
            board[36] = new Property("Tant Agdas Konditori", 12000);
            board[37] = new Property("Tant Agdas Vinkällare", 17000);
            board[38] = new BisysSpace();
            board[39] = new Property("Tant Agdas Prinsesstårta", 25000);

            bisysslor.Add(new Bisyssla("tappar sina tänder och slipper köpa tandvårdsprodukter.", 5000));
            bisysslor.Add(new Bisyssla("upptäcker spelet Professorn utan bisysslor och behöver aldrig köpa ett spel igen.", 10000));
            bisysslor.Add(new Bisyssla("har börjat läsa böcker, och köpte Henning Mankells Mordet i diskmatten som är väldigt dyr.", -2000));
            bisysslor.Add(new Bisyssla("har köpt en speldator för att spela sin nya bisyssla, Professorn utan bisysslor", -500));
            bisysslor.Add(new Bisyssla("har köpt en ny BMW Z4", -8000));
            bisysslor.Add(new Bisyssla("sparar pengar genom att köpa en falsk tågbiljett av Rådan", 1000));
            bisysslor.Add(new Bisyssla("attackeras av aggresiva radiotjänstarbetare.", -2216));
            bisysslor.Add(new Bisyssla("köper en pizza.", -200));

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
            if (players.Count() < 6)
            {
                this.players.Add(new Player(name));
                players.Last().color = colors[players.Count - 1];
                Debug.WriteLine(players.Count() + ": " + name);
            }
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
            XDocument xdoc = new XDocument(new XElement("Root"));

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
