using System;

namespace SomerenModel
{
    public class Docent
    {
        public int DocentId { get; set; } 
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Telefoonnummer { get; set; } 
        public DateTime Geboortedatum { get; set; }
        public string Kamer {  get; set; }
        public string Naam { get { return Voornaam + " " + Achternaam; } }
        public override string ToString()
        {
            return ($"{DocentId} {Voornaam} {Achternaam}");
        }

    }
}