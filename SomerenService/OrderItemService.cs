using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class OrderItemService
    {
        private OrderItemDao orderItemDao;
        

        public OrderItemService()
        {
            orderItemDao = new OrderItemDao();
        }

        public void AddOrderItem(OrderItem item) {orderItemDao.AddOrderitem(item);}

        public void AddBestelling(Bestelling bestelling) { orderItemDao.AddBestelling(bestelling);}

        public int MaxOrderItem()
        { return orderItemDao.GetMaxOrderId(); }

        public List<OrderItem> GetOrderItems()
        {
            return orderItemDao.GetAllOrderItems();
        }

        private List<OrderItem> GetOrderItemsByDate(DateTime start, DateTime end)
        {
            return orderItemDao.GetOrderItemsByDate(start, end);
        }

        public int RRGetTotalDrinksSold(DateTime start, DateTime end)
        {
            int total = 0;
            foreach (OrderItem item in orderItemDao.GetOrderItemsByDate(start, end))
            {
                total += item.Aantal;
            }
            return total;
        }

        public decimal RRGetTurnover(DateTime start, DateTime end)
        {
            decimal turnover = 0;
            foreach (OrderItem item in orderItemDao.GetOrderItemsByDate(start, end))
            {
                turnover += item.Aantal * item.Prijs;
            }
            return turnover;
        }
    }
}
