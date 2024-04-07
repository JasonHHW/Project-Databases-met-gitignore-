using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class ActivityService
    {
        
            private ActivityDao activitydb;

            public ActivityService()
            {
                activitydb = new ActivityDao();
            }

            public List<ActivityModel> GetActivities()
            {
                List<ActivityModel> activities = activitydb.GetAllActivities();
                return activities;
            }
        }
    }
