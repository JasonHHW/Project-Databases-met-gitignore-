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
                    BestellingId = (int)dr["orderId"],
                    StudentId = (int)dr["StudentId"],
                    BestelDatum = (DateTime)dr["orderDate"]
                };
                bestellingen.Add(bestelling);
            }
            return bestellingen;
        }

        public int ReadStudentsOrdered(DataTable dataTable)
        {
            if (dataTable.Rows.Count > 0)
            {
                int studentsOrdered = 0;
                foreach (DataRow dr in dataTable.Rows)
                {
                    if ((int)dr["Students"] != null)
                    {
                        studentsOrdered = (int)dr["Students"];
                    }
                }
                return studentsOrdered;
            }
            else
            {
                throw new Exception();
            }
        }

        public int IndividualStudentsOrdered(DateTime start, DateTime end)
        {
            string query = "SELECT COUNT(DISTINCT[studentId]) AS [Students] FROM [Ordering] WHERE [orderDate] BETWEEN @start AND @end";
            SqlParameter[] sqlParamaters = new SqlParameter[2];
            sqlParamaters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParamaters[1] = new SqlParameter("@end", SqlDbType.DateTime);
            sqlParamaters[0].Value = start.Date;
            sqlParamaters[1].Value = end.Date.AddDays(1);
            return ReadStudentsOrdered(ExecuteSelectQuery(query, sqlParamaters));
        }
    }
}
