using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class BegeleidingService
    {
        private BegeleidingDao begeleidingDao;

        public BegeleidingService()
        {
            begeleidingDao = new BegeleidingDao();
        }

        public List<Begeleiding> GetBegeleiders()
        {
            return begeleidingDao.GetAllBegeleiders();
        }
    }
}
