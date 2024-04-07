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
    public class ActivityDao : BaseDao
    {
        public List<ActivityModel> GetAllActivities()
        {
            string query = "SELECT * FROM [Activity]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        public List<ActivityModel> ReadTables(DataTable dataTable)
        {
            List<ActivityModel> activities = new List<ActivityModel>();

            foreach(DataRow dr in dataTable.Rows)
            {
                ActivityModel activity = new ActivityModel()
                {
                    ActivityId = (int)dr["activityId"],
                    ActivityName = dr["activityName"].ToString(),
                    StartTime = (DateTime)dr["startTime"],
                    EndTime = (DateTime)dr["endTime"]
                };
                activities.Add(activity);
            }
            return activities;
        }
    }
}
