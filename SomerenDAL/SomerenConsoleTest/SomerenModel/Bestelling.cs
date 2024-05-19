using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Bestelling
    {
        public int BestellingId { get; set; }
        
        public int StudentId { get; set; }
        public DateTime BestelDatum { get; set; }

        public override string ToString()
        {
            return ($"besellingsid:{BestellingId}, studentid: {StudentId}, besteldatum: {BestelDatum}");
        }
    }
}
