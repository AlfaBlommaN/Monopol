﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class Player
    {
        string name;
        int cash;
        int position;

        public Player(string name, int cash = 1500, int position = 0)
        {
            this.name = name;
            this.cash = cash;
            this.position = position;
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

    }
}