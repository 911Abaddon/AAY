using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAY
{
    public class Costumers
    {
        public Costumers() 
        {
            chairs=  new List<int>();
        }
        public string NameCostumer { get; set; }
        public string CostumersPhone { get; set; }

        public List<int>chairs { get; set; }

        public double price
        {
            get
            {
                return chairs.Count * 15; //15 ticket price
            }
        }
        public override string ToString()
        {
            return this .NameCostumer;
        }
    }
}
