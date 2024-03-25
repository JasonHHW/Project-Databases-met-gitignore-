using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class BestellingService
    {
        private BestellingDao bestellingDao;

        public BestellingService()
        {
            bestellingDao = new BestellingDao();
        }

        public List<Bestelling> GetBestellingen()
        {
            return bestellingDao.GetAllBestellingen();
        }
    }
}
