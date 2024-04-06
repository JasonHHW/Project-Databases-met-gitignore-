using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenModel;
namespace SomerenDAL
{
    public class DeelnameDao : BaseDao
    {
        public List<Student> GetAllDeelnemersFromActiviteitId(Activiteit activiteit)
        {
            string query = "SELECT [firstName], [lastName], [studentId] FROM [student] JOIN [Participation] ON Student.studentId = Participation.participant WHERE Participation.ActivityId = @ActiviteitId";
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter ("@ActiviteitId", activiteit.ActiviteitId) };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Student> ReadTables(DataTable dataTable)
        {
            List<Student> deelnemers = new List<Student>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Student deelnemer = new Student()
                {
                    Voornaam = (string)dr["firstName"],
                    Achternaam = (string)dr["lastName"],
                    StudentId = (int)dr["studentId"]
                };
                deelnemers.Add(deelnemer);
            }
            return deelnemers;
        }

        public List<Student> GetNonParticipatingStudents(Activiteit act)
        {
            string query = "SELECT [firstName], [lastName], [studentId] FROM [student] WHERE [studentId] NOT IN(SELECT [participant] FROM [Participation] WHERE ActivityId = @ActiviteitId)";
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter ("@ActiviteitId", act.ActiviteitId) };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public void RemoveParticipatingStudent(Activiteit act, Student student)
        {
            string query = "DELETE FROM [Participation] WHERE [participant] = @Student AND [ActivityId] = @Activity";
            SqlParameter[] sqlParameters = new SqlParameter[] {new SqlParameter ("@Student", student.StudentId), new SqlParameter ("@Activity", act.ActiviteitId)};
            ExecuteEditQuery(query, sqlParameters);
        }

        public void AddParticipatingStudent(Activiteit act, Student student)
        {
            string query = "INSERT INTO [Participation] (activityId, participant) VALUES (@activity, @student)";
            SqlParameter[] sqlParameters = new SqlParameter[] {new SqlParameter ("@activity", act.ActiviteitId), new SqlParameter ("@student", student.StudentId)};
            ExecuteEditQuery (query, sqlParameters);
        }
    }
}
