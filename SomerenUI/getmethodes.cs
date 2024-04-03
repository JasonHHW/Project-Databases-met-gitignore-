using SomerenModel;
using SomerenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenUI
{
    public static partial class Methodes 
    {

        public static List<Docent> GetDocenten()
        {
            DocentService docentService = new DocentService();
            List<Docent> docenten = docentService.GetDocenten();
            return docenten;
        }
        public static  List<Student> GetStudents()
        {
            StudentService studentService = new StudentService();
            List<Student> students = studentService.GetStudents();
            return students;
        }
        public static List<Kamer> GetKamers()
        {
            KamerService KamerService = new KamerService();
            List<Kamer> kamers = KamerService.GetKamers();
            return kamers;
        }
        public static List<Activiteit> GetActiviteiten()
        {
            ActiviteitService activiteitService = new ActiviteitService();
            List<Activiteit> activiteiten = activiteitService.GetActiviteiten();
            return activiteiten;
        }
        public static List<Drank> GetDrankjes()
        {
            DrankService drankService = new DrankService();
            List<Drank> drankjes = drankService.GetDrankjes();
            return drankjes;
        }

        public static List<Docent> GetBegeleiding(Activiteit activiteit)
        {
            DocentService docentService = new DocentService();
            List<Docent> docenten = docentService.GetBegeleiders(activiteit);
            return docenten;
        }
        public static List<Docent> GetVrijeDocent(Activiteit activiteit)
        {
            DocentService docentService = new DocentService();
            List<Docent> docenten = docentService.GetVrijeDocenten(activiteit);
            return docenten;
        }
    }
}
