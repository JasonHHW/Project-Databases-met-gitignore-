using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class TeacherService
    {
        private TeacherDao teacherdb;

        public TeacherService()
        {
            teacherdb = new TeacherDao();
        }

        public List<Teacher> GetSupervisors(ActivityModel activity)
        {
            return teacherdb.GetSupervision(activity);
        }
        public List<Teacher> GetFreeTeachers(ActivityModel activity)
        {
            return teacherdb.GetFreeTeachers(activity);
        }

        public void MakeFree(Teacher teacher, ActivityModel activity)
        { teacherdb.DeleteSupervision(teacher, activity); }
        public void MakeBusy(Teacher teacher, ActivityModel activity)
        { teacherdb.InsertSupervision(teacher, activity); }
        public List<Teacher> GetTeachers()
        {
            List<Teacher> teachers = teacherdb.GetAllTeachers();
            return teachers;
        }
    }
}
