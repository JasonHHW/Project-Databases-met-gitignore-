using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class SupervisionService
    {
        private SupervisionDao supervisionDao;

        public SupervisionService()
        {
            supervisionDao = new SupervisionDao();
        }

        public List<Supervision> GetSupervisors()
        {
            return supervisionDao.GetAllSupervisors();
        }
    }
}
