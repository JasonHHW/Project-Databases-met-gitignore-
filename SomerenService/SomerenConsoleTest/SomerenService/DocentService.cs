using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenService
{
    public class DocentService
    {
        private DocentDao docentdb;

        public DocentService()
        {
            docentdb = new DocentDao();
        }

        public List<Docent> GetDocenten()
        {
            List<Docent> docenten = docentdb.GetAllDocents();
            return docenten;
        }
    }
}
