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
    public class OrderItemDao : BaseDao
    {
        public List<OrderItem> GetAllOrderItems()
        {
            string query = "SELECT * FROM [OrderItem]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
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

        public List<OrderItem> GetOrderItemsByOrderDate(DateTime start, DateTime end)
        {
            string query = "SELECT * FROM [OrderItem] JOIN [Bestelling] ON Bestelling.BestellingId = OrderItem.BestellingId WHERE [BestelDatum] >= @start AND [BestelDatum] <= @eind";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@start", SqlDbType.DateTime);
            sqlParameters[1] = new SqlParameter("@eind", SqlDbType.DateTime);
            sqlParameters[0].Value = start;
            sqlParameters[1].Value = end.AddHours(23).AddMinutes(59).AddSeconds(59);
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        
    }
}
