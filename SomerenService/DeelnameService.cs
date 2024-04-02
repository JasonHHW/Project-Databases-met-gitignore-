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

        public List<Student> GetDeelnemersFromActiviteitId(Activiteit act)
        {
            return deelnameDao.GetAllDeelnemersFromActiviteitId(act);
        }

        public List<Student> GetNietDeelnemers(Activiteit act)
        {
            return deelnameDao.GetNonParticipatingStudents(act);
        }

    }
}
