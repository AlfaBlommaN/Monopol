using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class Player
    {
        public string name { get; private set; }
        public int cash;
        public bool prisoner;
        public bool responded = false;
        public int position{get; private set;}
        private bool allowedBuy;
        private Property tmpProp;

        public Player(string name, int cash = 15000, int position = 0)
        {
            this.name = name;
            this.cash = cash;
            this.position = position;
        }

        public void AllowPlayerToBuyProperty(Property prop)
        {
            Debug.WriteLine("Skoja bara!");
            tmpProp = prop;
            allowedBuy = true;
        }
        public void PayOpponent(Player opponent, int money)
        {
            Debug.WriteLine("TESTING TESTING 12 12");
            cash = cash - money;
            opponent.cash += money;
        }
        public void AllowPlayerToBuyProperty(bool no)
        {
            allowedBuy = false;
            Debug.WriteLine("Här får du inte handla!");
        }

        public void BuyProperty()
        {
            if (allowedBuy && cash >= tmpProp.cost)
            {
                cash = cash - tmpProp.cost;
                tmpProp.owner = name;
                allowedBuy = false;

                Debug.WriteLine("Grattis");
            }
            else
                Debug.WriteLine("Ledsen kompis, du får inte köpa");
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
                position = position + steps - 40;
            Debug.WriteLine(position);
        }

        public void GoTo(int position)
        {
            this.position = position;
        }

    }
}
