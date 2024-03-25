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
        public List<Deelname> GetAllDeelnemers()
        {
            string query = "SELECT * FROM [Deelname]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Deelname> ReadTables(DataTable dataTable)
        {
            List<Deelname> deelnemers = new List<Deelname>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Deelname deelnemer = new Deelname()
                {
                    ActiviteitId = (int)dr["ActiviteitId"],
                    Deelnemer = (int)dr["Deelnemer"]
                };
                deelnemers.Add(deelnemer);
            }
            return deelnemers;
        }
    }
}
