using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class DrankService
    {private DrankDao drankdb;

        public DrankService() {
            drankdb = new DrankDao(); }

        public List<Drank> GetDrankjes() { return drankdb.GetAllDrankjes();
        }


    }
}
