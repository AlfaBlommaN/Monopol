using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
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

        public Player(string name, int cash = 15000, int position = 0)
        {
            this.name = name;
            this.cash = cash;
            this.position = position;
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


        /// <summary>
        /// Låt spelaren traversa över spelplanen
        /// </summary>
        /// <param name="steps">Antal steg som spelaren skall gå</param>
        public void Go(int steps)
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

        public void GoTo(int position)
        {
            this.position = position;
        }

    }
}
