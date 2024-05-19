using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SomerenModel;

namespace SomerenDAL
{
    public class TeacherDao : BaseDao
    {
        public List<Teacher> GetAllTeachers()
        {
            string query = "SELECT [teacherId], [firstName], [lastName], [phoneNumber], [dateOfBirth], [room] FROM [Teacher]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Teacher> GetSupervision(ActivityModel activity )
        {
            string query = "SELECT firstName, lastName, teacherId FROM Supervision JOIN Teacher on teacherId = supervisor WHERE activityId = @ActivityId";
            SqlParameter[] sqlParameters = new SqlParameter[1]
            { new SqlParameter("@ActivityId", activity.ActivityId) };

            return ReadSupervisionTable(ExecuteSelectQuery(query, sqlParameters));     

        }
        public void DeleteSupervision (Teacher teacher, ActivityModel activity)
        {
            string delete = "DELETE FROM Supervision WHERE activityId = @ActivityId and supervisor = @TeacherId";
            SqlParameter[] sqlParameters = new SqlParameter[2]
             { new SqlParameter( "@ActivityId", activity.ActivityId),
                 new SqlParameter( "@TeacherId", teacher.TeacherId)
                 };
            ExecuteEditQuery(delete, sqlParameters);
        }
        public void InsertSupervision (Teacher teacher, ActivityModel activity)
        {
            string insert = "INSERT INTO Supervision (activityId, supervisor) VALUES (@ActivityId, @TeacherId)";
            SqlParameter[] sqlParameters = new SqlParameter[2]
             { new SqlParameter( "@ActivityID", activity.ActivityId),
                 new SqlParameter( "@TeacherId", teacher.TeacherId)
                 };
            ExecuteEditQuery(insert, sqlParameters);


        }


        public List<Teacher>GetFreeTeachers(ActivityModel activity)
        {
            string query = "SELECT distinct firstName, lastName, teacherId FROM Teacher WHERE teacherId NOT IN (SELECT supervisor FROM Supervision WHERE ActivityId = @ActivityId)";
            SqlParameter[] sqlParameters = new SqlParameter[1]
            { new SqlParameter("@ActivityId", activity.ActivityId) };

            return ReadSupervisionTable(ExecuteSelectQuery(query, sqlParameters));

        }

        public List<Teacher> ReadSupervisionTable(DataTable datatable)
        {
            List<Teacher> teachers = new List<Teacher>();

            foreach (DataRow dr in datatable.Rows)
            {
                Teacher teacher = new Teacher()
                {
                    TeacherId = (int)dr["teacherId"],
                    FirstName = dr["firstName"].ToString(),
                    LastName = dr["lastName"].ToString(),
                   // Telefoonnummer = dr["Telefoonnummer"].ToString(),
                   // Geboortedatum = (DateTime)dr["Geboortedatum"],
                   // Kamer = dr["Kamer"].ToString()
                };
                teachers.Add(teacher);
            }
            return teachers;
        }

        public List<Teacher> ReadTables(DataTable datatable)
        {
            List<Teacher> teachers = new List<Teacher>();

            foreach(DataRow dr in datatable.Rows)
            {
                Teacher teacher = new Teacher()
                {
                    TeacherId = (int)dr["teacherId"],
                    FirstName = dr["firstName"].ToString(),
                    LastName = dr["lastName"].ToString(),
                    PhoneNumber = dr["phoneNumber"].ToString(),
                    DateOfBirth = (DateTime)dr["dateOfBirth"],
                    Room = dr["room"].ToString()
                };
                teachers.Add(teacher);
            }
            return teachers;
        }
    }
}
