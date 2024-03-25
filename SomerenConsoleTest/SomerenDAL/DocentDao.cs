using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using SomerenModel;

namespace SomerenDAL
{
    public class DocentDao : BaseDao
    {
        public List<Docent> GetAllDocents()
        {
            string query = "SELECT * FROM [Docent]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Docent> ReadTables(DataTable datatable)
        {
            List<Docent> docenten = new List<Docent>();

            foreach(DataRow dr in datatable.Rows)
            {
                Docent docent = new Docent()
                {
                    DocentId = (int)dr["DocentId"],
                    Voornaam = dr["Voornaam"].ToString(),
                    Achternaam = dr["Achternaam"].ToString(),
                    Telefoonnummer = dr["Telefoonnummer"].ToString(),
                    Geboortedatum = (DateTime)dr["Geboortedatum"],
                    Kamer = dr["Kamer"].ToString()
                };
                docenten.Add(docent);
            }
            return docenten;
        }
    }
}
