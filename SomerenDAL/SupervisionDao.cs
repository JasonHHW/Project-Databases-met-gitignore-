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
    public class SupervisionDao : BaseDao
    {
        public List<Supervision> GetAllSupervisors()
        {
            string query = "SELECT [activityId], [supervisor] FROM [Supervision]";
            SqlParameter[] parameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, parameters));
        }

        public List<Supervision> ReadTables(DataTable dataTable)
        {
            List<Supervision> supervisors = new List<Supervision>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Supervision supervisor = new Supervision()
                {
                    ActivityId = (int)dr["activityId"],
                    Supervisor = (int)dr["supervisor"]
                };
                supervisors.Add(supervisor);
            }
            return supervisors;
        }
    }
}
