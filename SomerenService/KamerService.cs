using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class KamerService
    {
        private KamerDao kamerdb;

        public KamerService()
        {
            kamerdb = new KamerDao();
        }

        public List<Kamer> GetKamers()
        {
            List<Kamer> kamers = kamerdb.GetAllKamers();
            return kamers;
        }
    }
}
