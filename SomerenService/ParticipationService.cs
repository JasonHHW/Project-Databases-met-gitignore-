using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class ParticipationService
    {
        private ParticipationDao participationDao;

        public ParticipationService()
        {
            participationDao = new ParticipationDao();
        }

        public List<Student> GetParticipantsFromActivityId(ActivityModel act)
        {
            return participationDao.GetAllParticipantsFromActivityId(act);
        }

        public List<Student> GetNonParticipants(ActivityModel act)
        {
            return participationDao.GetNonParticipatingStudents(act);
        }

        public void RemoveParticipatingStudent(ActivityModel act, Student student)
        {
            participationDao.RemoveParticipatingStudent(act, student);
        }

        public void AddParticipatingStudent(ActivityModel act, Student student)
        {
            participationDao.AddParticipatingStudent(act, student);
        }
    }
}
