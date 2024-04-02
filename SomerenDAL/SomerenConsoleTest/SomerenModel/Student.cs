using System;

namespace SomerenModel
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Telefoonnummer { get; set; }
        public string Klas { get; set; }
        public string Kamer { get; set; }

        public string Naam { get { return Voornaam + " " + Achternaam; } }


        public override string ToString()
        {
            return ($"{StudentId} {Voornaam} {Achternaam}");
        }
    }
}