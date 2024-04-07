using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class RoomService
    {
        private RoomDao roomdb;

        public RoomService()
        {
            roomdb = new RoomDao();
        }

        public List<Room> GetRooms()
        {
            List<Room> rooms = roomdb.GetAllRooms();
            return rooms;
        }
    }
}
