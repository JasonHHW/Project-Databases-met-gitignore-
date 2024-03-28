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
            string query ="INSERT into [Bestelling] (BestellingId, StudentId, BestelDatum) VALUES (@BestellingId, @StudentId, @BestelDatum)";
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
            string query = "INSERT into [OrderItem] (BestellingId, Dranknaam, Aantal, ItemId) VALUES (@BestellingId, @Dranknaam, @Aantal, @ItemId)";
            SqlParameter[] sqlParameters = new SqlParameter[]
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
            string query = "select max(BestellingID) as BestellingID from Bestelling";
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
                    ItemId = (int)dr["ItemId"],
                    BestellingId = (int)dr["BestellingId"],
                    DrankNaam = dr["DrankNaam"].ToString(),
                    Aantal = (int)dr["Aantal"]
                };
                orderItems.Add(orderItem);
            }
            return orderItems;
        }

        public int ReadTotalDrinks(DataTable dataTable)
        {
            int totalDrinks = 0;
            foreach (DataRow dr in dataTable.Rows)
            {
                if (dr["Aantal"] != DBNull.Value)
                {
                    totalDrinks += Convert.ToInt32(dr["Aantal"]);
                }
            }
            return totalDrinks;
        }
        public int ReadTablesforint(DataTable dataTable)
        {
            
                DataRow dr = dataTable.Rows[0];

                return (int)dr["BestellingID"];
                  
         
        }

        public int CountOrderItemsByOrderDate(DateTime start, DateTime end)
        {
            string query = "SELECT SUM([Aantal]) AS [Aantal] FROM [OrderItem] JOIN [Bestelling] ON Bestelling.BestellingId = OrderItem.BestellingId WHERE [BestelDatum] BETWEEN @start AND @eind";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParameters[1] = new SqlParameter("@eind", SqlDbType.DateTime);
            sqlParameters[0].Value = start.Date;
            sqlParameters[1].Value = end.Date.AddDays(1);
            return ReadTotalDrinks(ExecuteSelectQuery(query, sqlParameters));
        }

        
    }
}
