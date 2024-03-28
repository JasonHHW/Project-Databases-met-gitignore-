using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class ActiviteitService
    {
        
            private ActiviteitDao activiteitdb;

            public ActiviteitService()
            {
                activiteitdb = new ActiviteitDao();
            }

            public List<Activiteit> GetActiviteiten()
            {
                List<Activiteit> activiteiten = activiteitdb.GetAllActiviteiten();
                return activiteiten;
            }
        }
    }
