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
    public class DrinkDao : BaseDao
    {
        public List<Drink> GetAllDrinks()
        {
            string query = "SELECT d.drinkName, d.isAlcoholic, s.stockAmount, COALESCE(SUM(oi.quantity), 0) AS TotalConsumed, d.price " +
                            "FROM Drink d " +
                            "JOIN Stock s ON d.drinkName = s.drinkName " +
                            "LEFT JOIN OrderItem oi ON d.drinkName = oi.drinkName " +
                            "GROUP BY d.drinkName, d.isAlcoholic, s.stockAmount, d.price;";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }
        //get voorraad

        public List<Drink> ReadTables(DataTable dataTable)
        {
            List<Drink> drinks = new List<Drink>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Drink drink = new Drink()
                {
                    IsAlcoholic = (bool)dr["IsAlcoholic"],
                    TotalConsumed = (int)dr["totalConsumed"],
                    Stock = (int)dr["stockAmount"],
                    DrinkName = dr["DrinkName"].ToString(),
                    Price = (decimal)dr["price"]

                };
                drinks.Add(drink);
            }
            return drinks;
        }

        public void AddDrink(string drinkName, bool isAlcoholic, int stock, decimal price)
        {
            try
            {
                if (DrinkExists(drinkName))
                {
                    throw new Exception("Drink with the same name already exists.");
                }   

                // Define your SQL query for adding a drink
                string query = "INSERT INTO Drink (drinkName, IsAlcoholic, Price) VALUES (@DrinkName, @IsAlcoholic, @Price);" +
                               "INSERT INTO Stock (drinkName, stockAmount) VALUES (@DrinkName, @Stock)";

                // Define parameters for your query
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DrinkName", SqlDbType.VarChar) { Value = drinkName },
                    new SqlParameter("@IsAlcoholic", SqlDbType.Bit) { Value = isAlcoholic },
                    new SqlParameter("@Stock", SqlDbType.Int) { Value = stock },
                    new SqlParameter("@Price", SqlDbType.Decimal) { Value = price }
                };

                // Execute the query
                ExecuteEditQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding drink: " + ex.Message, ex);
            }
        }

        public void UpdateDrink(string oldDrinkName, string newDrinkName, bool isAlcoholic, int stockAmount, decimal price)
        {
            try
            {
                if (oldDrinkName != newDrinkName && DrinkExists(newDrinkName))
                {
                    throw new Exception("Cannot update drink. Drink with the new name already exists.");
                }

                // Update the drink
                string updateQuery = "UPDATE Drink SET drinkName = @NewDrinkName, isAlcoholic = @IsAlcoholic, price = @Price WHERE drinkName = @OldDrinkName;" +
                                     "UPDATE Stock SET drinkName = @NewDrinkName, stockAmount = @Stock WHERE drinkname = @OldDrinkName";

                // Define parameters for the query
                SqlParameter[] updateParameters =
                {
                    new SqlParameter("@NewDrinkName", SqlDbType.VarChar) { Value = newDrinkName },
                    new SqlParameter("@IsAlcoholic", SqlDbType.Bit) { Value = isAlcoholic },
                    new SqlParameter("@Price", SqlDbType.Decimal) { Value = price },
                    new SqlParameter("@OldDrinkName", SqlDbType.VarChar) { Value = oldDrinkName },
                    new SqlParameter("@Stock", SqlDbType.Int) { Value = stockAmount }
                };

                // Execute the update query
                ExecuteEditQuery(updateQuery, updateParameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating drink: " + ex.Message, ex);
            }
        }

        public bool DrinkExists(string drinkName)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Drink WHERE drinkName = @DrinkName";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@DrinkName", SqlDbType.VarChar) { Value = drinkName }
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

        public void DeleteDrink(string drinkName)
        {
            try
            {
                // First remove related records from the OrderItem table
                string deleteOrderItemsQuery = "DELETE FROM OrderItem WHERE drinkName = @DrinkName";
                SqlParameter[] orderItemsParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteOrderItemsQuery, orderItemsParameters);

                // Delete the inventory information from the Stock table
                string deleteStockQuery = "DELETE FROM Stock WHERE drinkName = @DrinkName";
                SqlParameter[] stockParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteStockQuery, stockParameters);

                // Then delete the record from the Drink table
                string deleteDrinkQuery = "DELETE FROM Drink WHERE drinkName = @DrinkName";
                SqlParameter[] drinkParameters = { new SqlParameter("@DrinkName", drinkName) };
                ExecuteEditQuery(deleteDrinkQuery, drinkParameters);

                // Success message or other logic after successful deletion
                Console.WriteLine("Drink and inventory information successfully deleted.");
            }
            catch (Exception ex)
            {
                // Error
                Console.WriteLine("There was an error while deleting the drink and inventory information:" + ex.Message);
            }
        }
    }
}
