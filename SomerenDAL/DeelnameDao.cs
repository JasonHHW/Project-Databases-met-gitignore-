using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            string query = "SELECT [Voornaam], [Achternaam], [StudentId] FROM [Student] JOIN [Deelname] ON Student.StudentId = Deelname.StudentId WHERE Deelname.ActiviteitId = @ActiviteitId";
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
                    Voornaam = (string)dr["Voornaam"],
                    Achternaam = (string)dr["Deelnemer"],
                    StudentId = (int)dr["StudentId"]
                };
                deelnemers.Add(deelnemer);
            }
            return deelnemers;
        }

        public List<Student> GetNonParticipatingStudents(Activiteit act)
        {
            string query = "SELECT [Student.Voornaam], [Student.Achternaam], [Student.StudentId] FROM [Student] WHERE StudentId NOT IN(SELECT [StudentId] FROM [Deelname] WHERE ActiviteitId = @ActiviteitId)";
            SqlParameter[] sqlParameters = new SqlParameter[] { new SqlParameter ("@ActiviteitId", act.ActiviteitId) };
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
    }
}
