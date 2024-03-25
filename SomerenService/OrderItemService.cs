using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<OrderItem> GetOrderItems()
        {
            return orderItemDao.GetAllOrderItems();
        }
    }
}
