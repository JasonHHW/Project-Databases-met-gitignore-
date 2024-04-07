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
    public class RoomDao : BaseDao
    {
        public List<Room> GetAllRooms()
        {
            string query = "SELECT [roomCode], [building], [Verdieping], [isSingleRoom] FROM [Room]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<Room> ReadTables(DataTable dataTable)
        {
            List<Room> rooms = new List<Room>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Room room = new Room()
                {
                    RoomCode = dr["roomCode"].ToString(),
                    Building = dr["building"].ToString(),
                    Floor = Convert.ToInt32(dr["Verdieping"]),
                    IsSingleRoom = (bool)dr["isSingleRoom"]
                };
                rooms.Add(room);
            }
            return rooms;
        }
    }
}
