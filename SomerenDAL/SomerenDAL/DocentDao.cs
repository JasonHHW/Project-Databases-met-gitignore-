﻿using System;
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

        public List<Docent> GetBegeleiding(Activiteit activiteit )
        {

            string query = "SELECT Voornaam, Achternaam, DocentId from Begeleiding join Docent on DocentId=Begeleider where ActiviteitId= @ActiviteitID";
            SqlParameter[] sqlParameters = new SqlParameter[1]
            { new SqlParameter("@ActiviteitID", activiteit.ActiviteitId) };

            return ReadBegeleidingtable(ExecuteSelectQuery(query, sqlParameters));     

        }
        public void DeleteDeelname (Docent docent, Activiteit activiteit)
        {
            string delete = "delete from Begeleiding where ActiviteitId=@ActiviteitId and Begeleider = @BegeleiderId";
            SqlParameter[] sqlParameters = new SqlParameter[2]
             { new SqlParameter( "@ActiviteitID", activiteit.ActiviteitId),
                 new SqlParameter( "@BegeleiderId", docent.DocentId)
                 };
            ExecuteEditQuery(delete, sqlParameters);




        }
        public void InsertDeelname(Docent docent, Activiteit activiteit)
        {
            
            string insert = "insert into Begeleiding (ActiviteitId,Begeleider) values (@ActiviteitId, @BegeleiderId)";
            SqlParameter[] sqlParameters = new SqlParameter[2]
             { new SqlParameter( "@ActiviteitID", activiteit.ActiviteitId),
                 new SqlParameter( "@BegeleiderId", docent.DocentId)
                 };
            ExecuteEditQuery(insert, sqlParameters);


        }


        public List<Docent>GetVrijeDocenten(Activiteit activiteit )
        {
            string query = "SELECT distinct Voornaam, Achternaam, DocentId from Begeleiding join Docent on DocentId=Begeleider where Docentid not in (select Begeleider from Begeleiding where ActiviteitId=@ActiviteitID)";
            SqlParameter[] sqlParameters = new SqlParameter[1]
            { new SqlParameter("@ActiviteitID", activiteit.ActiviteitId) };

            return ReadBegeleidingtable(ExecuteSelectQuery(query, sqlParameters));

        }

        public List<Docent> ReadBegeleidingtable(DataTable datatable)
        {
            List<Docent> docenten = new List<Docent>();

            foreach (DataRow dr in datatable.Rows)
            {
                Docent docent = new Docent()
                {
                    DocentId = (int)dr["DocentId"],
                    Voornaam = dr["Voornaam"].ToString(),
                    Achternaam = dr["Achternaam"].ToString(),
                   // Telefoonnummer = dr["Telefoonnummer"].ToString(),
                   // Geboortedatum = (DateTime)dr["Geboortedatum"],
                   // Kamer = dr["Kamer"].ToString()
                };
                docenten.Add(docent);
            }
            return docenten;
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
