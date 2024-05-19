using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenModel;
namespace SomerenDAL
{
    public class OrderItemDao : BaseDao
    {
        public List<OrderItem> GetAllOrderItems()
        {
            string query = "SELECT * FROM [OrderItem]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

         public void AddBestelling(Bestelling bestelling)
        {
            string query ="INSERT into [Ordering] (orderId, studentId, orderDate) VALUES (@BestellingId, @StudentId, @BestelDatum)";
            SqlParameter[] sqlParameters = new SqlParameter[]
    {
        new SqlParameter("@BestellingId", bestelling.BestellingId),
        new SqlParameter("@StudentId", bestelling.StudentId),
        new SqlParameter("@BestelDatum", bestelling.BestelDatum)
    };


            ExecuteEditQuery(query, sqlParameters);
        }
        public void AddOrderitem(OrderItem item)
        {
            string query = "INSERT into [OrderItem] (orderId, drinkName, quantity, itemId) VALUES (@BestellingId, @Dranknaam, @Aantal, @ItemId)";
            SqlParameter[] sqlParameters = new SqlParameter[4]
            {
        new SqlParameter("@BestellingId", item.BestellingId),
        new SqlParameter("@Dranknaam", item.DrankNaam),
        new SqlParameter("@Aantal", item.Aantal),
        new SqlParameter("@ItemId", item.ItemId)
            };

           ExecuteEditQuery(query, sqlParameters);
        }

        public int GetMaxOrderId()
        {
            string query = "SELECT MAX(orderId) AS [orderId] FROM [Ordering]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTablesforint(ExecuteSelectQuery(query, sqlParameters));
            

        }
       
        
        public List<OrderItem> ReadTables(DataTable dataTable)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (DataRow dr in dataTable.Rows)
            {
                OrderItem orderItem = new OrderItem()
                {
                    ItemId = (int)dr["itemId"],
                    BestellingId = (int)dr["orderId"],
                    DrankNaam = dr["drinkName"].ToString(),
                    Aantal = (int)dr["quantity"],
                    Prijs = (decimal)dr["price"]
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }

        public List<OrderItem> RRReadTables(DataTable dataTable) // RR stands for RevenueReport
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            foreach (DataRow dr in dataTable.Rows)
            {
                OrderItem orderItem = new OrderItem()
                {
                    ItemId = (int)dr["itemId"],
                    DrankNaam = dr["drinkName"].ToString(),
                    Aantal = (int)dr["quantity"],
                    Prijs = (decimal)dr["price"]
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }

        public int ReadTablesforint(DataTable dataTable)
        {
            
            
                DataRow dr = dataTable.Rows[0];

            if (dr["orderID"] == DBNull.Value) { return 0; }
          
            else return (int)dr["orderID"];

        }

        public List<OrderItem> GetOrderItemsByDate(DateTime start, DateTime end)
        {
            // query selects the total and price for each drink and groups it by the name of the drink
            string query = "SELECT SUM([quantity]) AS [quantity], [itemId], [Drink].[price] AS [price], OrderItem.[drinkName] FROM [OrderItem] JOIN [Ordering] ON Ordering.orderId = OrderItem.orderId JOIN [Drink] ON OrderItem.drinkName = Drink.drinkName WHERE [orderDate] BETWEEN @start AND @eind GROUP BY OrderItem.drinkName, Drink.price, quantity, OrderItem.itemId";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParameters[1] = new SqlParameter("@eind", SqlDbType.DateTime);
            sqlParameters[0].Value = start.Date;
            sqlParameters[1].Value = end.Date.AddDays(1);
           return RRReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        
    }
}
