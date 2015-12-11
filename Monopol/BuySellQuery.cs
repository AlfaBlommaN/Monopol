using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class BuySellQuery
    {
        public BuyOrSell type { get; private set; }
        public int offer { get; private set; }
        public string property { get; private set; }
        public string sender { get; private set; }

        public BuySellQuery(string sender, BuyOrSell type, int offer, string property)
        {
            this.sender = sender;
            this.type = type;
            this.offer = offer;
            this.property = property;
        }
    }
}
