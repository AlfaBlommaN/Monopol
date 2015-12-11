using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monopol
{
   public class Space
    {
       public int position;
       public Space() { }
 
       public Space (int position_, string name_)
        {
            name = name_;
            position = position_;
        }

       public Space(int position_)
       {
           this.position = position_;
       }
        public string name;
        private int position_;
              
    }

    public class Property :  Space 
    {
        public string owner { get; set; }
        public int cost {get; private set;}

        public int rent;

        public Property(int position_, string name_, int cost_)
            : base(position_, name_)
        {
            owner = "";
            cost = cost_;
            rent = Convert.ToInt32(cost * 0.4);
        }

    }
    public class GoToJail : Space
    {
        public GoToJail(int position_):base(position_) { }
    }
    

}
