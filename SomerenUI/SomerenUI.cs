using SomerenService;
using SomerenModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
namespace SomerenUI
{
    public partial class SomerenUI : Form
    {
        Kamer kamer { get; set; }
        public SomerenUI()
        {
            InitializeComponent();
        }

        private void HideAllPanels() // Deze methode zet de visibility van alle pannels in de UI op false
        {
            pnlStudents.Hide();
            pnlDocenten.Hide();
            pnlActviteiten.Hide();
            pnlKamers.Hide();
            pnlDrankBestellingen.Hide();
            pnlDrankOmzet.Hide();
            pnlDrankVAT.Hide();
            pnlDrankVoorrraad.Hide();
        }
        private void ShowDashboardPanel()
        {
            // hide all other panels
            HideAllPanels();

            // show dashboard
            pnlDashboard.Show();
        }

        private void ShowStudentsPanel()
        {
            // hide all other panels
            //pnlDashboard.Hide();

            HideAllPanels();

            // show students
            pnlStudents.BringToFront();
            pnlStudents.Dock = DockStyle.Fill;

            pnlStudents.Show();

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
            // hide all other panels
            HideAllPanels();
            pnlActviteiten.BringToFront();
            pnlActviteiten.Dock = DockStyle.Fill;

            pnlActviteiten.Show();


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
            HideAllPanels();
            pnlKamers.BringToFront();
            pnlKamers.Dock = DockStyle.Fill;

            pnlKamers.Show();


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
        private void ShowTeachersPanel()
        {
            // hide all other panels
            HideAllPanels();
            pnlDocenten.BringToFront();

            pnlDocenten.Dock = DockStyle.Fill;

            pnlDocenten.Show();


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

        public void ShowOmzetPanel()
        {
            HideAllPanels();
            pnlDrankOmzet.BringToFront();

            pnlDrankOmzet.Dock = DockStyle.Fill;

            pnlDrankOmzet.Show();
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

        private List<OrderItem> GetOmzetItems()
        {
            OrderItemService orderItemService = new OrderItemService();
            List<OrderItem> orderItems = orderItemService.GetOrderItemsByDate(dtpDrankOmzetStart.Value, dtpDrankOmzetEind.Value);

            return orderItems;
        }

        public int GetAmountOfStudentsWithOrders()
        {
            BestellingService bestellingService = new BestellingService();
            return bestellingService.GetAmountOfStudentsOmzet(dtpDrankOmzetStart.Value, dtpDrankOmzetEind.Value).Count;
        }


        private void DisplayActiviteiten(List<Activiteit> activiteiten)
        {
            // clear the listview before filling it
            listViewActiviteiten.Items.Clear();

            foreach (Activiteit activiteit in activiteiten)
            {
                ListViewItem li = new ListViewItem(activiteit.ActiviteitNaam);
                li.SubItems.Add(activiteit.BeginTijd.ToString("yyyy-MM-dd HH:mm"));
                li.SubItems.Add(activiteit.EindTijd.ToString("yyyy-MM-dd HH:mm"));

                li.Tag = activiteit;   // link student object to listview item

                listViewActiviteiten.Items.Add(li);

            }
            listViewActiviteiten.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
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
            listViewStudents.Items.Clear();


            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.Naam);


                li.Tag = student;   // link student object to listview item
                listViewStudents.Items.Add(li);

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

        private void DisplayOmzet()
        {
            listViewDrankOmzet.Items.Clear();

            List<OrderItem> orderItems = GetOmzetItems();
            int studentsOrdered = GetAmountOfStudentsWithOrders();
            double price = 2.00;
            int totalDrinksSold = 0;
            double turnover = 0.00;
            foreach (OrderItem orderItem in orderItems)
            {
                totalDrinksSold += orderItem.Aantal;
                turnover += orderItem.Aantal * price;
            }
            ListViewItem li = new ListViewItem(Convert.ToString(totalDrinksSold));
            li.SubItems.Add("€ " + String.Format("{0,00}", turnover));
            li.SubItems.Add(Convert.ToString(studentsOrdered));
            listViewDrankOmzet.Items.Add(li);
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

        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTeachersPanel();
        }

        private void pnlTeachers_Paint(object sender, PaintEventArgs e)
        {

        }

        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowKamersPanel();
        }

        private void listViewKamers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void activitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowActiviteitenPanel();
        }

        private void drankToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pnlStudents_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlDrankOmzet_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblDrankOmzetEind_Click(object sender, EventArgs e)
        {

        }

        private void dtpDrankOmzetEind_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDrankOmzetStart.Value > dtpDrankOmzetEind.Value)
            {
                MessageBox.Show("End date can not be set before the start date");
                dtpDrankOmzetEind.Value = dtpDrankOmzetStart.Value;
            }
            else if (dtpDrankOmzetEind.Value > DateTime.Now)
            {
                MessageBox.Show("End date can not be set after the current date");
                dtpDrankOmzetEind.Value = DateTime.Now;
            }
            else
            {
                DisplayOmzet();
            }

        }

        private void omzetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOmzetPanel();
        }

        private void dtpDrankOmzetStart_ValueChanged(object sender, EventArgs e)
        {
            if (dtpDrankOmzetStart.Value > DateTime.Now)
            {
                MessageBox.Show("Start date can not be set after the current date");
                dtpDrankOmzetStart.Value = DateTime.Now;
            } else if (dtpDrankOmzetStart.Value > dtpDrankOmzetEind.Value)
            {
                MessageBox.Show("Start date can not be set after the end date");
                dtpDrankOmzetStart.Value = dtpDrankOmzetEind.Value;
            }
            else
            {
                DisplayOmzet();
            }
        }

        private void listViewDrankOmzet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}