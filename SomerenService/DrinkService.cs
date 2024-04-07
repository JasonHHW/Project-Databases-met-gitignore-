using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class DrinkService
    {private DrinkDao drinkdb;

        public DrinkService() {
            drinkdb = new DrinkDao(); }

        public List<Drink> GetDrinks() { return drinkdb.GetAllDrinks();
        }


    }
}
