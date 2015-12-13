using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Monopol
{
    public enum BuyOrSell { Buy, Sell }
    public class Game
    {
        private Color[] colors = new Color[] { Color.Blue, Color.Red, Color.Purple, Color.Orange, Color.DarkGreen, Color.Black };
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

        public int[] ThrowDice()
        {
            NextTurn();
            Debug.WriteLine("\n" + players[playerturn].name + "\nCash: " + players[playerturn].cash.ToString());
            int[] dice = new int[2];
            Random rnd = new Random();
            dice[1] = rnd.Next(1, 7);
            dice[0] = rnd.Next(1, 7);
            players[playerturn].Go(dice[0] + dice[1]);
            Debug.WriteLine("Tärning: " + (dice[0] + dice[1]).ToString());
            Rules.CheckState(players[playerturn], this);
            lastThrow = dice;
            return dice;
        }

        public Player GetCurrPlayer()
        {
            return players[playerturn];
        }

        public Space getCurrSpace()
        {
            return board[GetCurrPlayer().position];
        }

        public void addPlayer(bool isAI, string name)
        {
            if (players.Count() < 6)
            {
                if (isAI)
                    this.players.Add(new AI(name));
                else
                    this.players.Add(new Player(name));
                players.Last().color = colors[players.Count - 1];
                Debug.WriteLine(players.Count() + ": " + name);
            }
        }

        public void addPlayer(bool isAI, string name, int cash, int position, bool prisoner)
        {
            if (players.Count() < 6)
            {
                if (isAI)
                    this.players.Add(new AI(name, cash, position));
                else
                    this.players.Add(new Player(name, cash, position));
                players.Last().color = colors[players.Count - 1];
                players.Last().prisoner = prisoner;

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

        public void SendQueryToPlayer(string name, BuySellQuery query)
        {
            findPlayer(name).AddQuery(query);
        }

        public void Save(FileSystemWatcher watcher)
        {
            watcher.EnableRaisingEvents = false;
            XDocument xdoc = new XDocument(new XElement("Root"));

            XElement playersState = new XElement("Players",

            from player in players
            where player.active == true
            select new XElement("Player",
                new XAttribute("AI", player is AI),
                new XAttribute("Name", player.name),
                new XAttribute("Cash", player.cash),
                new XAttribute("Prisoner", player.prisoner),
                new XAttribute("Position", player.position)
                ));

            XElement boardState = new XElement("Board",

            from space in board
            where (space.GetType() == typeof(Property) && ((Property)space).owner != "")
            select new XElement("Property",
                new XAttribute("Name", ((Property)space).name),
                new XAttribute("Owner", ((Property)space).owner))

            );

            xdoc.Root.Add(new XComment("Information om spelarna"), playersState);
            xdoc.Root.Add(new XComment("Ändringar i spelbrädet"), boardState);
            xdoc.Save("Data/State.xml");
            watcher.EnableRaisingEvents = true;
        }

        public void LoadState(FileSystemWatcher watcher)
        {
            System.Threading.Thread.Sleep(500);
            watcher.EnableRaisingEvents = false;
            try
            {
                var playerState = from members in XDocument.Load("Data/State.xml").Root.Elements().Descendants("Player")
                                  select members;

                var propertyState = from members in XDocument.Load("Data/State.xml").Root.Elements().Descendants("Property")
                                    select members;

                players = new List<Player>();

                foreach (var player in playerState)
                {
                    addPlayer(
                        Boolean.Parse(player.Attribute("AI").Value),
                                    player.Attribute("Name").Value, 
                        Int32.Parse(player.Attribute("Cash").Value), 
                        Int32.Parse(player.Attribute("Position").Value), 
                        Boolean.Parse(player.Attribute("Prisoner").Value));
                }


                foreach (Space p in board)
                {

                    if (p is Property)
                    {
                        ((Property)p).owner = "";
                        foreach (var x in propertyState)
                        {
                            if (((Property)p).name == x.Attribute("Name").Value)
                                ((Property)p).owner = x.Attribute("Owner").Value;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            watcher.EnableRaisingEvents = true;
        }

        public void LoadInitData()
        {
            var newSpaces = from members in XElement.Load("Data/Board.xml").Elements()
                            select members;
            var newBisys = from members in XElement.Load("Data/Bisysslor.xml").Elements()
                           select members;

            foreach (XElement x in newSpaces)
            {
                int position = Int32.Parse(x.Attribute("Position").Value);

                switch (x.Name.ToString())
                {
                    case "Space":
                        string spacename = x.Attribute("Name").Value;
                        board[position] = new Space(position, spacename);
                        break;
                    case "Bisysspace":
                        board[position] = new BisysSpace(position);
                        break;
                    case "GoToJail":
                        board[position] = new GoToJail(position);
                        break;
                    case "Property":
                        string propname = x.Attribute("Name").Value;
                        int cost = Int32.Parse(x.Attribute("Cost").Value);
                        board[position] = new Property(position, propname, cost);
                        break;
                }

            }
            foreach (XElement x in newBisys)
            {
                bisysslor.Add(new Bisyssla(x.Attribute("Message").Value, Int32.Parse(x.Attribute("Value").Value)));
            }


        }

    }
}
