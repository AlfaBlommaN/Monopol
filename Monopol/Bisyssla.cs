using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
    public class BisysSpace : Space
    {

    }

    public class Bisyssla
    {
        public int value { get; private set; }
        public string message { get; private set; }
        public Bisyssla(string message, int value)
        {
            this.value = value;
            this.message = message;
        }
    }
}
