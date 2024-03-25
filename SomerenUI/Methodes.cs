using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SomerenUI
{
   //een static class, aangezien deze class alleen uit helper methodes bestaat om somerenui.cs op te schonen.
   //Er hoeft hier dus geen specifieke object voor aangemaakt te worden elke keer als we de methodes willen gebruiken
    public static partial class Methodes
    {
        //itereert over elke control in de dashboardpanel en checkt of het een panel is, vervolgens verstopt/minimaliseert hij deze panel
        // en showt hij de specifiekep panel + vergroot hij deze naar de grootte van de dashboard panel   

        public static void ShowPanel(Panel panel)
        {
            if (panel.Parent is Panel)
            {
                foreach (Control control in panel.Parent.Controls)
                {
                    if (control is Panel pnl && control != panel)
                    {
                        pnl.Hide();
                    } 
                }
                panel.BringToFront();
                panel.Dock = DockStyle.Fill;
               

            } else 
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is Panel pnl)
                    {
                        pnl.Hide();
                    }
                } }
            panel.Show();
            

        }
    }
}
