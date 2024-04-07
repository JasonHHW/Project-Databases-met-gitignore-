using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Drink
    {
        public string DrinkName { get; set; }
        public bool IsAlcoholic {  get; set; }
        public decimal Price {  get; set; }
        public int Stock { get; set; }
        public string Type
        {
            get
            {
                if (IsAlcoholic)
                { return "Alcoholic"; }
                else
                {
                    return "Non-alcoholic";
                }
            }
        }
        public int TotalConsumed {  get; set; }
    }
   
}
