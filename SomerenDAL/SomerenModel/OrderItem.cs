using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class OrderItem
    {
        public int ItemId { get; set; }
        public int BestellingId { get; set; }
        public string DrankNaam { get; set; }
        public int Aantal {  get; set; }

        public override string ToString()
        {
            return ($"itemid:{ItemId}, bestellingsid:{BestellingId}, dranknaam: {DrankNaam}, aantal: {Aantal}");
        }
    }
}
