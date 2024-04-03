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

        public List<Docent> GetBegeleiders(Activiteit activiteit)
        {
            return docentdb.GetBegeleiding(activiteit);
        }
        public List<Docent> GetVrijeDocenten(Activiteit activiteit)
        {
            return docentdb.GetVrijeDocenten(activiteit);
        }

        public void MakeFree(Docent docent, Activiteit activiteit)
        { docentdb.DeleteDeelname(docent, activiteit); }
        public void MakeBusy(Docent docent, Activiteit activiteit)
        { docentdb.InsertDeelname(docent, activiteit); }
        public List<Docent> GetDocenten()
        {
            List<Docent> docenten = docentdb.GetAllDocents();
            return docenten;
        }
    }
}
