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
    public class ParticipationDao : BaseDao
    {
        public List<Student> GetAllParticipantsFromActivityId(SomerenModel.ActivityModel activity)
        {
            string query = "SELECT [firstName], [lastName], [studentId] FROM [student] JOIN [Participation] ON Student.studentId = Participation.participant WHERE Participation.ActivityId = @ActiviteitId";
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter ("@ActiviteitId", activity.ActivityId) };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Student> ReadTables(DataTable dataTable)
        {
            List<Student> participants = new List<Student>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Student participant = new Student()
                {
                    FirstName = (string)dr["firstName"],
                    LastName = (string)dr["lastName"],
                    StudentId = (int)dr["studentId"]
                };
                participants.Add(participant);
            }
            return participants;
        }

        public List<Student> GetNonParticipatingStudents(SomerenModel.ActivityModel act)
        {
            string query = "SELECT [firstName], [lastName], [studentId] FROM [student] WHERE [studentId] NOT IN(SELECT [participant] FROM [Participation] WHERE ActivityId = @ActiviteitId)";
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter ("@ActiviteitId", act.ActivityId) };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public void RemoveParticipatingStudent(SomerenModel.ActivityModel act, Student student)
        {
            string query = "DELETE FROM [Participation] WHERE [participant] = @Student AND [ActivityId] = @Activity";
            SqlParameter[] sqlParameters = new SqlParameter[] {new SqlParameter ("@Student", student.StudentId), new SqlParameter ("@Activity", act.ActivityId)};
            ExecuteEditQuery(query, sqlParameters);
        }

        public void AddParticipatingStudent(SomerenModel.ActivityModel act, Student student)
        {
            string query = "INSERT INTO [Participation] (activityId, participant) VALUES (@activity, @student)";
            SqlParameter[] sqlParameters = new SqlParameter[] {new SqlParameter ("@activity", act.ActivityId), new SqlParameter ("@student", student.StudentId)};
            ExecuteEditQuery (query, sqlParameters);
        }
    }
}
