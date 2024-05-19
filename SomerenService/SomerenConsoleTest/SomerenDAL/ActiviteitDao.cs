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
    public class ActiviteitDao : BaseDao
    {
        public List<Activiteit> GetAllActiviteiten()
        {
            string query = "SELECT * FROM [Activiteit]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Activiteit> ReadTables(DataTable dataTable)
        {
            List<Activiteit> activiteiten = new List<Activiteit>();

            foreach(DataRow dr in dataTable.Rows)
            {
                Activiteit activiteit = new Activiteit()
                {
                    ActiviteitId = (int)dr["ActiviteitId"],
                    ActiviteitNaam = dr["ActiviteitNaam"].ToString(),
                    BeginTijd = (DateTime)dr["BeginTijd"],
                    EindTijd = (DateTime)dr["EindTijd"]
                };
                activiteiten.Add(activiteit);
            }
            return activiteiten;
        }
    }
}
