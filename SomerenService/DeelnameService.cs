using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class DeelnameService
    {
        private DeelnameDao deelnameDao;

        public DeelnameService()
        {
            deelnameDao = new DeelnameDao();
        }

        public List<Deelname> GetDeelnemers()
        {
            return deelnameDao.GetAllDeelnemers();
        }
    }
}
