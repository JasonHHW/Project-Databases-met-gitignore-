using SomerenModel;
using SomerenDAL;
using SomerenService;
namespace SomerenConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Kamer> kamers = new List<Kamer>();
            KamerService kamerService = new KamerService();
            kamers = kamerService.GetKamers();
            if (kamers[0].IsEenPersoons == true)
            {
                Console.WriteLine("True");
            } else
            {
                Console.WriteLine("False");
            }
        }
    }
}