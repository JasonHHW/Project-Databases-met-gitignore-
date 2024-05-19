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
    public class BegeleidingDao : BaseDao
    {
        public List<Begeleiding> GetAllBegeleiders()
        {
            string query = "SELECT * FROM [Begeleiding]";
            SqlParameter[] parameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, parameters));
        }

        public List<Begeleiding> ReadTables(DataTable dataTable)
        {
            List<Begeleiding> begeleiders = new List<Begeleiding>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Begeleiding begeleider = new Begeleiding()
                {
                    ActiviteitId = (int)dr["ActiviteitId"],
                    Begeleider = (int)dr["Begeleider"]
                };
                begeleiders.Add(begeleider);
            }
            return begeleiders;
        }
    }
}
