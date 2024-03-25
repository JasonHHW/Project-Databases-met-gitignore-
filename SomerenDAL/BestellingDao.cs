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
                    StudentId = (int)dr["StudentId"],
                    BestelDatum = (DateTime)dr["BestelDatum"]
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }

        public List<Bestelling> ReadStudentId(DataTable dataTable)
        {
            List<Bestelling> bestellingen = new List<Bestelling>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Bestelling bestelling = new Bestelling()
                {
                    StudentId = (int)dr["StudentId"],
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }

        public List<Bestelling> IndividualStudentsOrdered(DateTime start, DateTime end)
        {
            string query = "SELECT DISTINCT[StudentId] FROM [Bestelling] WHERE [BestelDatum] >= @start AND [BestelDatum] <= @eind";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParameters[1] = new SqlParameter("@eind", SqlDbType.DateTime);
            sqlParameters[0].Value = start;
            sqlParameters[1].Value = end.AddHours(23).AddMinutes(59).AddSeconds(59);
            return ReadStudentId(ExecuteSelectQuery(query, sqlParameters));
        }
    }
}
