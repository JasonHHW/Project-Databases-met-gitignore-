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

        public List<Bestelling> ReadStudentsOrdered(DataTable dataTable)
        {
            List<Bestelling> studenten = new List<Bestelling>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Bestelling student = new Bestelling()
                {
                    StudentId = (int)dr["StudentId"]
                };
                studenten.Add(student);
            }
            return studenten;
        }

        public List<Bestelling> IndividualStudentsOrdered(DateTime start, DateTime end)
        {
            string query = "SELECT DISTINCT[StudentId] FROM [Bestelling] WHERE [BestelDatum] >= @start AND [BestelDatum] <= @end";
            SqlParameter[] sqlParamaters = new SqlParameter[2];
            sqlParamaters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParamaters[1] = new SqlParameter("@end", SqlDbType.DateTime);
            sqlParamaters[0].Value = start;
            sqlParamaters[1].Value = end.AddHours(23).AddMinutes(59).AddSeconds(59);
            return ReadStudentsOrdered(ExecuteSelectQuery(query, sqlParamaters));
        }
    }
}
