using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
   public class Space
    {

        public Space (string name_)
        {
            name = name_;
        }
        public string name;
              
    }

    public class Property :  Space 
    {
        public int cost {get; private set;}

        public Property (string name_, int cost_) : base(name_)
        {
            cost = cost_;
        }




    }
}
