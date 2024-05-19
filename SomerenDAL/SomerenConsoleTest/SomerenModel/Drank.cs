using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Drank
    {
        
       
       public string DrankNaam { get; set; }
        public bool IsAlcoholisch {  get; set; }
        public int Prijs = 2;
        public int Voorraad { get; set; }
        public string Type
        {
            get
            {
                if (IsAlcoholisch)
                { return "Alcoholisch"; }
                else
                {
                    return "Non-alcoholisch";
                }
            }
        }
        public int Aantal_Geconsumeerd {  get; set; }
    }
   
}
