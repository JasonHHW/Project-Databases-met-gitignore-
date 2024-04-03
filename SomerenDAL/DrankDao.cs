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
            string query = "SELECT Drank.Dranknaam, Drank.IsAlcoholisch, Drank.AantalGeconsumeerd, Voorraad.VoorraadAantal, Drank.Prijs " +
                        "FROM [Drank] " +
                        "join Voorraad on Drank.Dranknaam = Voorraad.Dranknaam";
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
                    Aantal_Geconsumeerd = (int)dr["AantalGeconsumeerd"],
                    Voorraad = (int)dr["VoorraadAantal"],
                    DrankNaam = dr["DrankNaam"].ToString(),
                    Prijs = (decimal)dr["Prijs"]

                };
                drankjes.Add(drankje);
            }
            return drankjes;
        }

        public void AddDrink(string drinkName, bool isAlcoholisch, int voorraadAantal, decimal prijs)
        {
            try
            {
                if (DrankExists(drinkName))
                {
                    throw new Exception("Drink with the same name already exists.");
                }   

                // Define your SQL query for adding a drink
                string query = "INSERT INTO Drank (Dranknaam, IsAlcoholisch, AantalGeconsumeerd, Prijs) VALUES (@DrankNaam, @IsAlcoholisch, @AantalGeconsumeerd, @Prijs);" +
                               "INSERT INTO Voorraad (Dranknaam, VoorraadAantal) VALUES (@DrankNaam, @VoorraadAantal)";

                // Define parameters for your query
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DrankNaam", SqlDbType.VarChar) { Value = drinkName },
                    new SqlParameter("@IsAlcoholisch", SqlDbType.Bit) { Value = isAlcoholisch },
                    new SqlParameter("@AantalGeconsumeerd", SqlDbType.Int) { Value = 0 },
                    new SqlParameter("@VoorraadAantal", SqlDbType.Int) { Value = voorraadAantal },
                    new SqlParameter("@Prijs", SqlDbType.Decimal) { Value = prijs }
                };

                // Execute the query
                ExecuteEditQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding drink: " + ex.Message, ex);
            }
        }

        public void UpdateDrank(string oldDrankNaam, string newDrankNaam, bool isAlcoholisch, int voorraadAantal, decimal prijs)
        {
            try
            {
                if (oldDrankNaam != newDrankNaam && DrankExists(newDrankNaam))
                {
                    throw new Exception("Cannot update drink. Drink with the new name already exists.");
                }

                // Update the drink
                string updateQuery = "UPDATE Drank SET Dranknaam = @NewDrankNaam, IsAlcoholisch = @IsAlcoholisch, Prijs = @Prijs WHERE Dranknaam = @OldDrankNaam;" +
                                     "UPDATE Voorraad SET Dranknaam = @NewDrankNaam, VoorraadAantal = @VoorraadAantal WHERE Dranknaam = @OldDrankNaam";

                // Define parameters for the query
                SqlParameter[] updateParameters =
                {
                    new SqlParameter("@NewDrankNaam", SqlDbType.VarChar) { Value = newDrankNaam },
                    new SqlParameter("@IsAlcoholisch", SqlDbType.Bit) { Value = isAlcoholisch },
                    new SqlParameter("@Prijs", SqlDbType.Decimal) { Value = prijs },
                    new SqlParameter("@OldDrankNaam", SqlDbType.VarChar) { Value = oldDrankNaam },
                    new SqlParameter("@VoorraadAantal", SqlDbType.Int) { Value = voorraadAantal }
                };

                // Execute the update query
                ExecuteEditQuery(updateQuery, updateParameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating drink: " + ex.Message, ex);
            }
        }

        public bool DrankExists(string drankNaam)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Drank WHERE Dranknaam = @DrankNaam";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DrankNaam", SqlDbType.VarChar) { Value = drankNaam }
                };
                DataTable result = ExecuteSelectQuery(query, parameters);
                int count = Convert.ToInt32(result.Rows[0][0]);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if drink exists: " + ex.Message, ex);
            }
        }

        public void DeleteDrank(string drinkName)
        {
            try
            {
                // Verwijder eerst gerelateerde records uit de OrderItem tabel
                string deleteOrderItemsQuery = "DELETE FROM OrderItem WHERE Dranknaam = @DrinkName";
                SqlParameter[] orderItemsParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteOrderItemsQuery, orderItemsParameters);

                // Verwijder de voorraadinformatie uit de Voorraad tabel
                string deleteVoorraadQuery = "DELETE FROM Voorraad WHERE DrankNaam = @DrinkName";
                SqlParameter[] voorraadParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteVoorraadQuery, voorraadParameters);

                // Vervolgens de record verwijderen uit de Drank tabel
                string deleteDrinkQuery = "DELETE FROM Drank WHERE DrankNaam = @DrinkName";
                SqlParameter[] drankParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteDrinkQuery, drankParameters);

                // Succesmelding of andere logica na succesvol verwijderen
                Console.WriteLine("Drank en voorraadinformatie succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                // Foutafhandeling
                Console.WriteLine("Er is een fout opgetreden bij het verwijderen van de drank en voorraadinformatie: " + ex.Message);
            }
        }
    }
}
