using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
   public class Space
    {

       public Space() { }
 
       public Space (string name_)
        {
            name = name_;
        }
        public string name;
              
    }

    public class Property :  Space 
    {
        public string owner { get; set; }
        public int cost {get; private set;}

        public int rent;

        public Property (string name_, int cost_) : base(name_)
        {
            owner = "";
            cost = cost_;
            rent = Convert.ToInt32(cost * 0.4);
        }

    }
    public class GoToJail : Space
    {

    }
    
    public class Bisys : Space
    {
        
    }

}
