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
    public class DrankDao : BaseDao
    {
        public List<Drank> GetAllDrankjes()
        {
            string query = "SELECT Drank.Dranknaam, IsAlcoholisch, VoorraadAantal FROM [Drank] join Voorraad on Drank.Dranknaam= Voorraad.Dranknaam";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        //get voorraad

        public List<Drank> ReadTables(DataTable dataTable)
        {
            List<Drank> drankjes = new List<Drank>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Drank drankje = new Drank()
                {
              IsAlcoholisch = (bool)dr["IsAlcoholisch"],

                  //  Aantal_Geconsumeerd = (int)dr["Aantal Geconsumeerd"]
                  Voorraad = (int)dr["VoorraadAantal"],
                    
                    DrankNaam = dr["DrankNaam"].ToString()

                };
                drankjes.Add(drankje);
            }
            return drankjes;
        }
    }
}
