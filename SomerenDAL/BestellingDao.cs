using SomerenModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenDAL
{
    public class BestellingDao : BaseDao
    {
        public List<Bestelling> GetAllBestellingen()
        {
            string query = "SELECT * FROM [Bestelling]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Bestelling> ReadTables(DataTable dataTable)
        {
            List<Bestelling> bestellingen = new List<Bestelling>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Bestelling bestelling = new Bestelling()
                {
                    BestellingId = (int)dr["BestellingId"],
                    OrderItemId = (int)dr["OrderItemId"],
                    StudentId = (int)dr["StudentId"],
                    BestelDatum = (DateTime)dr["BestelDatum"]
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }
    }
}
