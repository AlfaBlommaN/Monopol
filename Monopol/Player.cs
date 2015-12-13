using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class AI : Player
    {
        public AI(string name, int cash = 15000, int position = 0) : base(name, cash, position) { Debug.WriteLine("SKAPAR AI"); }

        public override bool GetDecision(int cost)
        {
            Debug.WriteLine("AI DECISION");
            if (cost <= cash * 0.70)
                return true;
            else
                return false;
        }
    }
    public class Player
    {
        public string name { get; private set; }
        public int cash;
        public bool prisoner = false;
        public bool responded = false;
        public bool active = true;
        public int position{get; private set;}
        private bool allowedBuy = false;
        private Property tmpProp;
        public string icon = "";
        public Color color;

        private bool decision;

        public Queue<BuySellQuery> pendingQueries = new Queue<BuySellQuery>();

        public Player(string name, int cash = 15000, int position = 0)
        {
            this.name = name;
            this.cash = cash;
            this.position = position;
        }

        public void SetDecision(bool dec)
        {
            decision = dec;
        }

        public virtual bool GetDecision(int cost)
        {
            return decision;
        }

        public void AllowPlayerToBuyProperty(Property prop)
        {
            tmpProp = prop;
            allowedBuy = true;
        }
        public void PayOpponent(Player opponent, int money)
        {
            Debug.WriteLine(this.name + " betalar " + money.ToString() + " kr till " + opponent.name);
            cash = cash - money;
            opponent.cash += money;
        }
        public void AllowPlayerToBuyProperty(bool no)
        {
            allowedBuy = false;
        }

        public void BuyProperty()
        {
            if (allowedBuy && cash >= tmpProp.cost)
            {
                cash = cash - tmpProp.cost;
                tmpProp.owner = name;
                allowedBuy = false;
            }
        }

        public void GetOutOfJail()
        {
            this.prisoner = false;
            this.cash -= 2000;
        }


        /// <summary>
        /// Låt spelaren traversa över spelplanen
        /// </summary>
        /// <param name="steps">Antal steg som spelaren skall gå</param>
        public void Go(int steps)
        {
            if (!this.prisoner)
            {
                if (position + steps < 40)
                    position = position + steps;
                else
                {
                    position = position + steps - 40;
                    cash += 5000;
                }
                Debug.WriteLine("Position: " + position);
            }
        }

        public void GoTo(int position)
        {
            this.position = position;
        }


        public void AddQuery(BuySellQuery query)
        {
            pendingQueries.Enqueue(query);
        }

        public bool AcceptQuery(BuySellQuery query, Game g)
        {

            // Om spelaren som skickar queryn säljer och den som tar emot köper
            if (query.type == BuyOrSell.Sell)
            {
                if (cash < query.offer)
                    return false;
                Space prop = (from p in g.board
                                where p.GetType() == typeof(Property)
                                && p.name == query.property
                                select p).Single();

                cash -= query.offer;
                g.findPlayer(query.sender).cash += query.offer;
                ((Property)prop).owner = this.name;
            }

            // Om spelaren som skickar queryn vill köpa och den som tar emot säljer
            if (query.type == BuyOrSell.Buy)
            {
                Space prop = (from p in g.board
                              where p.GetType() == typeof(Property)
                              && p.name == query.property
                              select p).Single();

                cash += query.offer;
                g.findPlayer(query.sender).cash -= query.offer;
                ((Property)prop).owner = query.sender;
            }
            return true;
        }
    }
}
