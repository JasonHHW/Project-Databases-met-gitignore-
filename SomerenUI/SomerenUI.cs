using SomerenService;
using SomerenModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Drawing.Text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing;
using System.Text.RegularExpressions;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Diagnostics.Eventing.Reader;
namespace SomerenUI
{
    public partial class SomerenUI : Form
    {
        Kamer kamer { get; set; }
        public SomerenUI()
        {
            InitializeComponent();
        }

        private void ShowDashboardPanel()
        {

            Methodes.ShowPanel(pnlDashboard);

        }

        private void ShowStudentsPanel()
        {

            Methodes.ShowPanel(pnlStudents);

            try
            {
                // get and display all students
                List<Student> students = GetStudents();
                DisplayStudents(students);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the students: " + e.Message);
            }
        }


        private void ShowActiviteitenPanel()
        {
            Methodes.ShowPanel(pnlActviteiten);
            try
            {
                // get and display all students
                List<Activiteit> activiteiten = GetActiviteiten();
                DisplayActiviteiten(activiteiten);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }

        }
        private void ShowKamersPanel()
        {
            Methodes.ShowPanel(pnlKamers);

            try
            {
                // get and display all students
                List<Kamer> kamers = GetKamers();
                DisplayKamers(kamers);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the rooms: " + e.Message);
            }



        }




        private void ShowDrankBestellingenPanel()
        {
            Methodes.ShowPanel(pnlDrankBestellingen);
            try
            {
                List<Drank> drankjes = GetDrankjes();
                List<Student> students = GetStudents();

                DisplayDrankjes(drankjes);
                Methodes.DisplayStudents(students, listViewBestellingenStudenten);
                listViewTotaalBesteld.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the students and drinks: " + e.Message);
            }
        }


        private void ShowTeachersPanel()
        {
            Methodes.ShowPanel(pnlDocenten);
            try
            {
                // get and display all students
                List<Docent> docenten = GetDocenten();
                DisplayTeachers(docenten);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the teachers: " + e.Message);
            }
        }


        private List<Docent> GetDocenten()
        {
            DocentService docentService = new DocentService();
            List<Docent> docenten = docentService.GetDocenten();
            return docenten;
        }
        private List<Student> GetStudents()
        {
            StudentService studentService = new StudentService();
            List<Student> students = studentService.GetStudents();
            return students;
        }
        private List<Kamer> GetKamers()
        {
            KamerService KamerService = new KamerService();
            List<Kamer> kamers = KamerService.GetKamers();
            return kamers;
        }
        private List<Activiteit> GetActiviteiten()
        {
            ActiviteitService activiteitService = new ActiviteitService();
            List<Activiteit> activiteiten = activiteitService.GetActiviteiten();
            return activiteiten;
        }
        private List<Drank> GetDrankjes()
        {
            DrankService drankService = new DrankService();
            List<Drank> drankjes = drankService.GetDrankjes();
            return drankjes;
        }


        private void DisplayActiviteiten(List<Activiteit> activiteiten)
        {
            // clear the listview before filling it
            listViewActiviteiten.Items.Clear();

            foreach (Activiteit activiteit in activiteiten)
            {
                ListViewItem li = new ListViewItem(activiteit.ActiviteitNaam);
                li.SubItems.Add(activiteit.BeginTijd.ToString("dd-MM-yyyy HH:mm"));
                li.SubItems.Add(activiteit.EindTijd.ToString("dd-MM-yyyy HH:mm"));

                li.Tag = activiteit;   // link student object to listview item

                listViewActiviteiten.Items.Add(li);

            }
            listViewActiviteiten.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void DisplayDrankjes(List<Drank> drankjes)
        {
            listViewBestellingenDrankjes.Items.Clear();
            foreach (Drank drank in drankjes)
            {
                ListViewItem li = new ListViewItem(drank.DrankNaam);
                li.Tag = drank;
                li.SubItems.Add($"{drank.Prijs:C}");
                li.SubItems.Add(drank.Type);
                li.SubItems.Add(drank.Voorraad.ToString());
                listViewBestellingenDrankjes.Items.Add(li);

            }
            listViewBestellingenDrankjes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }
        private void DisplayKamers(List<Kamer> kamers)
        {
            // clear the listview before filling it
            listViewKamers.Items.Clear();


            foreach (Kamer kamer in kamers)
            {
                ListViewItem li = new ListViewItem(kamer.RoomCode);
                li.Tag = kamer;   // link student object to listview item
                li.SubItems.Add(kamer.Slaapplekken.ToString());
                li.SubItems.Add(kamer.Type.ToString());


                listViewKamers.Items.Add(li);

            }
            listViewKamers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void DisplayStudents(List<Student> students)
        {
            // clear the listview before filling it
            listViewStudenten.Items.Clear();


            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.Naam);


                li.Tag = student;   // link student object to listview item
                listViewStudenten.Items.Add(li);

            }
            //   listViewStudents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private void DisplayTeachers(List<Docent> docenten)
        {
            listViewDocenten.Items.Clear();

            foreach (Docent docent in docenten)
            {
                ListViewItem li = new ListViewItem(docent.Naam);


                li.Tag = docent;   // link student object to listview item
                listViewDocenten.Items.Add(li);

            }
        }

        private void UpdateBestelling(ListViewItem selectedDrink)
        {
            bool newdrink = true;
            

            if (int.TryParse(textBoxHoeveelheidDrank.Text, out int aantalDrank) && aantalDrank > 0)
            { 
                
                decimal prijs = aantalDrank * decimal.Parse(Regex.Replace(selectedDrink.SubItems[1].Text, @"[^\d,]", ""));
                decimal totaal = decimal.Parse(Regex.Replace(labelPrijs.Text, @"[^\d,]", "")) + prijs;
                int totaalaantalDrank = 0;
                foreach (ListViewItem listItem in listViewTotaalBesteld.Items)
                {
                    if (listItem.Text == selectedDrink.Text)

                    {
                        OrderItem orderItem = (OrderItem)listItem.Tag;
                        

                        newdrink = false;
                        totaalaantalDrank = int.Parse(listItem.SubItems[1].Text) + aantalDrank;
                        orderItem.Aantal=totaalaantalDrank;

                        listItem.SubItems[1].Text = totaalaantalDrank.ToString();
                        listItem.SubItems[2].Text = (decimal.Parse(Regex.Replace(listItem.SubItems[2].Text, @"[^\d,]", "")) +  prijs).ToString("C");
                    }
                }




                if (newdrink)
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.DrankNaam = selectedDrink.SubItems[0].Text;
                    orderItem.Aantal = aantalDrank;
                    //orderItem.prijs = decimal.Parse(Regex.Replace(selectedDrink.SubItems[1].Text, @"[^\d,]", ""));
                    //orderItem.ItemId =
                   // orderItem.BestellingId = maxbestellingid in database + 1

                    ListViewItem li = new ListViewItem(selectedDrink.SubItems[0].Text);
                 
                    li.SubItems.Add(aantalDrank.ToString());
                    li.SubItems.Add(prijs.ToString("C"));
                    li.Tag = orderItem;
                  
                    listViewTotaalBesteld.Items.Add(li);
                }


                labelPrijs.Text = totaal.ToString("C");


            }
            else { MessageBox.Show("Gebruik alstublieft een valide getal"); }
        }
        private void DisplayTotaalBesteld()
        {

           

            ListViewItem selectedStudent = new ListViewItem();
            ListViewItem selectedDrink = new ListViewItem();


            if (listViewBestellingenStudenten.SelectedItems.Count == 1 && listViewBestellingenDrankjes.SelectedItems.Count == 1)
            {
                selectedStudent = listViewBestellingenStudenten.SelectedItems[0];
                selectedDrink = listViewBestellingenDrankjes.SelectedItems[0];

                UpdateBestelling(selectedDrink);


            }
            else { MessageBox.Show("Selecteer alstublieft 1 student en 1 drankje"); }
        }
        private void ResetBestelling()
        {
            listViewTotaalBesteld.Items.Clear();
            textBoxHoeveelheidDrank.Text = "";
            labelPrijs.Text = "$0,00";

        }

        private void dashboardToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            ShowDashboardPanel();

        }

        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowStudentsPanel();
        }



        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTeachersPanel();
        }



        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowKamersPanel();
        }



        private void activitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowActiviteitenPanel();
        }






        private void drankBestellingenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDrankBestellingenPanel();
        }





        private void button1_Click(object sender, EventArgs e)
        {
            DisplayTotaalBesteld();
        }

        private void listViewBestellingenStudenten_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            ResetBestelling();
            if (listViewBestellingenStudenten.SelectedItems.Count == 1)
            {

                lblNaamBesteller.Text = listViewBestellingenStudenten.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void bttnResetBestelling_Click(object sender, EventArgs e)
        {
            ResetBestelling();

        }
        private void tagging()
        {
         //   foreach (OrderItem item in listViewTotaalBesteld) ;
        
        }
        private bool checkstock()
        { 
            bool erisgenoeg = true;

            foreach (ListViewItem orderItem in listViewTotaalBesteld.Items)
            {
                string dranknaam = orderItem.SubItems[0].Text;
                int hoeveelheidbesteld = int.Parse(orderItem.SubItems[1].Text);
                
                foreach (ListViewItem drinkItem in listViewBestellingenDrankjes.Items)
                {
                    if (drinkItem.SubItems[0].Text == dranknaam)
                    {
                        int aantaldrankover = int.Parse(drinkItem.SubItems[3].Text);


                        if (hoeveelheidbesteld > aantaldrankover)
                        {
                            MessageBox.Show($"Er is niet genoeg {dranknaam} over, we hebben nog maar {aantaldrankover} ");
                            erisgenoeg = false;
                        }
                       
                        break;
                        
                    }




                }


            }
            return erisgenoeg;
        }

        private void bttnPlaatsBestelling_Click(object sender, EventArgs e)
        {
            if (listViewBestellingenStudenten.SelectedItems.Count == 1 && listViewTotaalBesteld.Items.Count!=0)
            {
                Student selectedStudent = (Student)listViewBestellingenStudenten.SelectedItems[0].Tag;
                string voornaam = selectedStudent.Voornaam;
                
                DateTime tijdvanbestelling = DateTime.Now;
                if (checkstock())
                {
                    DialogResult result = MessageBox.Show($"{voornaam}, Weet je zeker dat je voor {labelPrijs.Text} aan drankjes wil bestellen? \n {tijdvanbestelling:g} ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        OrderItemService orderItemService = new OrderItemService();
                        int newBestellingId = orderItemService.MaxOrderItem() + 1;
                        Bestelling bestelling = new Bestelling();
                        bestelling.BestellingId = newBestellingId;
                        bestelling.StudentId = selectedStudent.StudentId;
                        bestelling.BestelDatum = tijdvanbestelling;

                        MessageBox.Show(bestelling.ToString());
                        orderItemService.AddBestelling(bestelling);
                        List<OrderItem> orderItems = new List<OrderItem>();
                        int id = 0;
                        foreach (ListViewItem listviewItem in listViewTotaalBesteld.Items)

                        {

                            OrderItem orderitem = (OrderItem)listviewItem.Tag;
                            id += 1;
                            orderitem.ItemId = id;
                            orderitem.BestellingId = newBestellingId;
                            orderItems.Add(orderitem);
                            MessageBox.Show(orderitem.ToString());
                           orderItemService.AddOrderItem(orderitem);
                        }
                        ResetBestelling();

                        listViewBestellingenStudenten.SelectedItems[0].Selected = false;
                        listViewBestellingenDrankjes.SelectedItems[0].Selected = false;
                        ShowDrankBestellingenPanel();
                        
                    }
                    else
                    {

                    }

                } else { }

            }
            else { MessageBox.Show("Selecteer alstublieft op wiens naam de bestelling is en voeg een drankje toe"); }
           
        }
        }
    }
 