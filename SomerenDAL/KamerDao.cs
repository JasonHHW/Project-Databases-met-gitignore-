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
    public class KamerDao : BaseDao
    {
        public List<Kamer> GetAllKamers()
        {
            string query = "SELECT * FROM [Kamer]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Kamer> ReadTables(DataTable dataTable)
        {
            List<Kamer> kamers = new List<Kamer>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Kamer kamer = new Kamer()
                {
                    KamerCode = dr["KamerCode"].ToString(),
                    Gebouw = dr["Gebouw"].ToString(),
                    Verdieping = Convert.ToInt32(dr["Verdieping"]),
                    IsEenPersoons = (bool)dr["IsEenPersoons"]
                };
                kamers.Add(kamer);
            }
            return kamers;
        }
    }
}
