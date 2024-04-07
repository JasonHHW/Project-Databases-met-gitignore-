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
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.Linq;
using SomerenDAL;

namespace SomerenUI
{
    public partial class SomerenUI : Form
    {

        public SomerenUI()
        {
            InitializeComponent();
        }

        // Dashboard
        private void ShowDashboardPanel()
        {
            Methodes.ShowPanel(pnlDashboard);
        }
        private void dashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDashboardPanel();
        }
        private void dashboardToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            ShowDashboardPanel();
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        //Student
        private void ShowStudentsPanel()
        {
            //Displays Student Panel
            Methodes.ShowPanel(pnlStudents);

            try
            {
                // Get and display all students
                List<Student> students = GetStudents();
                DisplayStudents(students);
                FillStudentKamerComboBox();
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the students: " + e.Message);
            }
        }
        private List<Student> GetStudents()
        {
            StudentService studentService = new StudentService();
            List<Student> students = studentService.GetStudents();
            return students;
        }
        private void DisplayStudents(List<Student> students)
        {
            // Clear the listview before filling it
            listViewStudenten.Items.Clear();

            //Puts each student in the listview
            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.StudentId.ToString());

                li.SubItems.Add(student.Naam);
                li.SubItems.Add(student.Telefoonnummer);
                li.SubItems.Add(student.Klas);
                li.SubItems.Add(student.Kamer);

                listViewStudenten.Items.Add(li);
            }
        }
        private void listViewStudenten_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewStudenten.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedStudent = listViewStudenten.SelectedItems[0];

                // Extract data from the selected ListViewItem
                int studentId = int.Parse(selectedStudent.Text);
                string studentNaam = selectedStudent.SubItems[1].Text;
                string studentTelefoonnummer = selectedStudent.SubItems[2].Text;
                string studentKlas = selectedStudent.SubItems[3].Text;
                string studentKamer = selectedStudent.SubItems[4].Text;

                //Splits Naam into Voor- en Achternaam
                string[] parts = studentNaam.Split(new char[] { ' ' }, 2);
                string studentVoornaam = parts[0];
                string studentAchternaam = parts.Length > 1 ? parts[1] : "";

                // Update TextBoxes with selected data
                StudentIdInput.Text = studentId.ToString();
                StudentVoornaamInput.Text = studentVoornaam;
                StudentAchternaamInput.Text = studentAchternaam;
                StudentTelefoonnummerInput.Text = studentTelefoonnummer;
                StudentKlasInput.Text = studentKlas;

                //Selects Student Kamer in ComboBox
                int index = studentKamerComboBox.FindStringExact(studentKamer);
                if (index != -1)
                {
                    studentKamerComboBox.SelectedIndex = index;
                }
                else
                {
                    studentKamerComboBox.SelectedIndex = -1;
                }
            }
        }
        private void FillStudentKamerComboBox()
        {
            // Instantiate StudentDao
            StudentDao studentDao = new StudentDao();

            try
            {
                // Get all student rooms from the database
                List<string> studentRooms = studentDao.GetStudentRooms();

                // Clear existing items in the ComboBox
                studentKamerComboBox.Items.Clear();

                // Add retrieved student rooms to the ComboBox
                foreach (string room in studentRooms)
                {
                    studentKamerComboBox.Items.Add(room);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                MessageBox.Show("An error occurred while fetching student rooms: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearStudentsInputFields()
        {
            //Clears all data from the Student Textboxes And Unselects the ComboBox
            StudentIdInput.Text = "";
            StudentVoornaamInput.Text = "";
            StudentAchternaamInput.Text = "";
            StudentTelefoonnummerInput.Text = "";
            StudentKlasInput.Text = "";
            studentKamerComboBox.SelectedIndex = -1;
            listViewStudenten.SelectedItems.Clear();
        }
        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowStudentsPanel();
        }

        //Student CRUD
        private void AddStudentButton_Click(object sender, EventArgs e)
        {
            //Takes all data drom TextBoxes and ComboBox
            int studentId = int.Parse(StudentIdInput.Text);
            string studentVoornaam = StudentVoornaamInput.Text;
            string studentAchternaam = StudentAchternaamInput.Text;
            string studentTelefoonnummer = StudentTelefoonnummerInput.Text;
            string studentKlas = StudentKlasInput.Text;
            string studentKamer = studentKamerComboBox.SelectedItem.ToString();

            // Show confirmation message
            DialogResult result = MessageBox.Show($"Are you sure you want to add {studentVoornaam} {studentAchternaam} with StudentId {studentId}, phonenumber {studentTelefoonnummer}, in class {studentKlas} and room {studentKamer}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Instantiate StudentDao
                StudentDao studentDao = new StudentDao();

                try
                {
                    // Add the student to the database
                    studentDao.AddStudent(studentId, studentVoornaam, studentAchternaam, studentTelefoonnummer, studentKlas, studentKamer);

                    // Display success message
                    MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    ClearStudentsInputFields();
                }
                catch (Exception ex)
                {
                    // Display error message
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Refreshes Student Panel & ListView
                ShowStudentsPanel();
            }
        }
        private void EditStudentButton_Click(object sender, EventArgs e)
        {
            if (listViewStudenten.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedStudent = listViewStudenten.SelectedItems[0];

                // Get the old data from the selected item
                // StudentId for Database Querry, everything else for the Comfermation Message
                int studentId = int.Parse(selectedStudent.Text);

                string studentNaam = selectedStudent.SubItems[1].Text;
                string studentTelefoonnummer = selectedStudent.SubItems[2].Text;
                string studentKlas = selectedStudent.SubItems[3].Text;
                string studentKamer = selectedStudent.SubItems[4].Text;

                // Get new data from textboxes & ComboBox
                int newStudentId = int.Parse(StudentIdInput.Text);
                string newStudentVoornaam = StudentVoornaamInput.Text;
                string newStudentAchternaam = StudentAchternaamInput.Text;
                string newStudentTelefoonnummer = StudentTelefoonnummerInput.Text;
                string newStudentKlas = StudentKlasInput.Text;
                string newStudentKamer = studentKamerComboBox.SelectedItem.ToString();

                // Confirmation message
                DialogResult result = MessageBox.Show($"Are you sure you want to edit:\n{studentNaam} with Id {studentId} with phonenumber {studentTelefoonnummer} in class {studentKlas} and room {studentKamer}" +
                                                        $"\nto:\n" +
                                                        $"{newStudentVoornaam} {newStudentAchternaam} with Id {newStudentId} with phonenumber {newStudentTelefoonnummer} in class {newStudentKlas} and room {newStudentKamer}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Call the DAO method to update the student
                    StudentDao studentDao = new StudentDao();
                    try
                    {
                        //Sends old studentId and updated data to Database
                        studentDao.UpdateStudent(studentId, newStudentId, newStudentVoornaam, newStudentAchternaam, newStudentTelefoonnummer, newStudentKlas, newStudentKamer);

                        // Display a success message to the user
                        MessageBox.Show("Student updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear input fields
                        ClearStudentsInputFields();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions appropriately
                        MessageBox.Show("Error updating Student: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Inform the user to select a row in the ListView
                MessageBox.Show("Please select a student to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Refreshes the Student Panel and ListView
            ShowStudentsPanel();
        }
        private void DeleteStudentButton_Click(object sender, EventArgs e)
        {
            if (listViewStudenten.SelectedItems.Count == 1)
            {
                // Ask for Confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to delete this student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Get studentId from selectedListView
                    int selectedStudentId = int.Parse(listViewStudenten.SelectedItems[0].Text);

                    // Instantiate StudentDao
                    StudentDao studentDao = new StudentDao();

                    // Delete Student from Database
                    studentDao.DeleteStudent(selectedStudentId);

                    // Clear Textboxes and ComboBox
                    ClearStudentsInputFields();

                    // Refresh studentpanel and listView
                    ShowStudentsPanel();
                }
            }
            else
            {
                // No Student was selected
                MessageBox.Show("Please select a student to delete.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Docent
        private void ShowTeachersPanel()
        {
            //Shows teacher panel
            Methodes.ShowPanel(pnlDocenten);

            try
            {
                // get and display all teachers
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
        private void DisplayTeachers(List<Docent> docenten)
        {
            //Clears data from listView
            listViewDocenten.Items.Clear();

            // Displays each Docent
            foreach (Docent docent in docenten)
            {
                ListViewItem li = new ListViewItem(docent.Naam);

                li.Tag = docent;   // link docent object to listview item
                listViewDocenten.Items.Add(li);
            }
        }
        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTeachersPanel();
        }

        //Activiteiten
        private void ShowActiviteitenPanel()
        {
            //Shows Activiteiten panel
            Methodes.ShowPanel(pnlActviteiten);

            try
            {
                // get and display all students
                List<Activiteit> activiteiten = Methodes.GetActiviteiten();
                DisplayActiviteiten(activiteiten);

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }

        }
        private List<Activiteit> GetActiviteiten()
        {
            ActiviteitService activiteitService = new ActiviteitService();
            List<Activiteit> activiteiten = activiteitService.GetActiviteiten();
            return activiteiten;
        }
        private void DisplayActiviteiten(List<Activiteit> activiteiten)
        {
            // clear the listview before filling it
            listViewActiviteiten.Items.Clear();

            //Displays each activiteit
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
        private void activitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowActiviteitenPanel();
        }


        //Deelname
        public void ShowDeelnemersBeherenPanel()
        {
            // Shows Deelnemer panel
            Methodes.ShowPanel(pnlManageActivityParticipants);

            try
            {
                List<Activiteit> activiteiten = GetActiviteiten();
                DisplayActiviteitenMAP(activiteiten);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }
        }
        private void DisplayStudentsMAP(Activiteit activiteit)
        {
            DeelnameService deelnameService = new DeelnameService(); // gets all the participating and non participating students from the database
            List<Student> participants = deelnameService.GetDeelnemersFromActiviteitId(activiteit);
            List<Student> nonParticipants = deelnameService.GetNietDeelnemers(activiteit);

            listViewMAPParticipatingStudents.Clear();
            listViewMAPNonParticipatingStudents.Clear();

            foreach (Student student in participants) // each foreach fills the participants and non participants listview respectively
            {
                ListViewItem li = new ListViewItem(student.Naam);
                li.SubItems.Add(student.StudentId.ToString());
                li.Tag = student;
                listViewMAPParticipatingStudents.Items.Add(li);
            }
            foreach (Student student in nonParticipants)
            {
                ListViewItem li = new ListViewItem(student.Naam);
                li.SubItems.Add(student.StudentId.ToString());
                li.Tag = student;
                listViewMAPNonParticipatingStudents.Items.Add(li);
            }
        }
        private void DisplayActiviteitenMAP(List<Activiteit> activiteiten)
        {
            // clear the listview before filling it
            listViewMAPActivities.Items.Clear();

            foreach (Activiteit activiteit in activiteiten)
            {
                ListViewItem li = new ListViewItem(activiteit.ActiviteitNaam);
                li.SubItems.Add(activiteit.ActiviteitId.ToString());
                li.SubItems.Add(activiteit.BeginTijd.ToString("dd-MM-yyyy HH:mm"));
                li.SubItems.Add(activiteit.EindTijd.ToString("dd-MM-yyyy HH:mm"));
                li.Tag = activiteit;   // link student object to listview item

                listViewMAPActivities.Items.Add(li);

            }
            listViewActiviteiten.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private void deelnemersBeherenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDeelnemersBeherenPanel();
        }
        private void MAPActivitySelected(object sender, EventArgs e)
        {
            if (listViewMAPActivities.SelectedItems.Count! > 0)
            {
                DisplayStudentsMAP((Activiteit)listViewMAPActivities.SelectedItems[0].Tag);
            }
        }

        private void MAPRemoveStudent(object sender, EventArgs e)
        {
            DeelnameService deelnameService = new DeelnameService();
            if (listViewMAPParticipatingStudents.SelectedItems.Count > 0)
            {
                var ConfirmRemoveStudents = MessageBox.Show("Are you sure you want to delete the student from this activity?", "Remove student from activity", MessageBoxButtons.YesNo);
                if (ConfirmRemoveStudents == DialogResult.Yes)
                {
                    deelnameService.RemoveParticipatingStudent((Activiteit)listViewMAPActivities.SelectedItems[0].Tag, (Student)listViewMAPParticipatingStudents.SelectedItems[0].Tag);
                    DisplayStudentsMAP((Activiteit)listViewMAPActivities.SelectedItems[0].Tag);
                }
            }
            else
            {
                MessageBox.Show("Please select a student before proceeding");
            }

        }

        private void MAPAddStudent(object sender, EventArgs e)
        {
            DeelnameService deelnameService = new DeelnameService();
            if (listViewMAPNonParticipatingStudents.SelectedItems.Count > 0)
            {
                deelnameService.AddParticipatingStudent((Activiteit)listViewMAPActivities.SelectedItems[0].Tag, (Student)listViewMAPNonParticipatingStudents.SelectedItems[0].Tag);
                DisplayStudentsMAP((Activiteit)listViewMAPActivities.SelectedItems[0].Tag);
            }
            else
            {
                MessageBox.Show("Please select a student before proceeding");
            }
        }

        //Kamers
        private void ShowKamersPanel()
        {
            Methodes.ShowPanel(pnlKamers);

            try
            {
                // get and display all students
                List<Kamer> kamers = Methodes.GetKamers();
                DisplayKamers(kamers);


            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the rooms: " + e.Message);
            }

        }

        private void DisplayBegeleiding(List<Docent> docenten)
        {
            listViewBegeleiders.Items.Clear();


            foreach (Docent begeleider in docenten)
            {
                ListViewItem li = new ListViewItem(begeleider.DocentId.ToString());
                li.SubItems.Add(begeleider.Naam);

                li.Tag = begeleider;   // link student object to listview item


                listViewBegeleiders.Items.Add(li);

            }

            listViewBegeleiders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }

        private void DisplayVrijeDocenten(List<Docent> docenten)
        {
            listViewVrijeDocenten.Items.Clear();
            foreach (Docent vrijedocent in docenten)
            {
                ListViewItem li = new ListViewItem(vrijedocent.DocentId.ToString());
                li.SubItems.Add(vrijedocent.Naam);

                li.Tag = vrijedocent;   // link student object to listview item


                listViewVrijeDocenten.Items.Add(li);

            }
            listViewVrijeDocenten.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private List<Kamer> GetKamers()
        {
            KamerService KamerService = new KamerService();
            List<Kamer> kamers = KamerService.GetKamers();
            return kamers;
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
        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowKamersPanel();
        }


        //Drank
        private List<Drank> GetDrankjes()
        {
            DrankService drankService = new DrankService();
            List<Drank> drankjes = drankService.GetDrankjes();
            return drankjes;
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
        private void drankToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        //Voorraad
        private void ShowDrankVoorraadPanel()
        {
            Methodes.ShowPanel(pnlDrankVoorraad);

            try
            {
                // get and display all students
                List<Drank> drankjes = GetDrankjes();
                DisplayDrankVoorraad(drankjes);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the stock: " + e.Message);
            }
        }
        private void DisplayDrankVoorraad(List<Drank> voorraad)
        {
            try
            {
                var orderedDrankList = voorraad.OrderBy(d => d.Voorraad).ToList();

                // Clear existing items
                listViewVoorraad.Items.Clear();

                // Populate the ListView with drink stock information
                foreach (Drank item in orderedDrankList)
                {
                    ListViewItem listViewItem = new ListViewItem(item.DrankNaam);
                    listViewItem.SubItems.Add(item.IsAlcoholisch ? "Yes" : "No");
                    listViewItem.SubItems.Add(item.Voorraad.ToString());
                    listViewItem.SubItems.Add(item.Aantal_Geconsumeerd.ToString());
                    listViewItem.SubItems.Add(item.Prijs.ToString("€0.00"));

                    // Add stock status index as a subitem
                    listViewItem.SubItems.Add((item.Voorraad < 10) ? "Insufficient" : "Sufficient");

                    listViewVoorraad.Items.Add(listViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading drink stock information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void drankVoorraadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDrankVoorraadPanel();
        }
        private void listViewVoorraad_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewVoorraad.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = listViewVoorraad.SelectedItems[0];

                // Extract data from the selected ListViewItem
                string drankNaam = selectedItem.Text;
                string isAlcoholischText = selectedItem.SubItems[1].Text;

                // Convert text representation of "Alcoholisch" to boolean
                bool isAlcoholisch = isAlcoholischText.Equals("Yes", StringComparison.OrdinalIgnoreCase);

                // Update controls with selected data
                DrankNaamInput.Text = drankNaam;
                AlcoholischCheckBox.Checked = isAlcoholisch;

                // Parse VoorraadAantal (stock quantity)
                int voorraadAantal;
                if (!int.TryParse(selectedItem.SubItems[2].Text, out voorraadAantal))
                {
                    // Handle parsing error gracefully
                    MessageBox.Show("Invalid stock quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse Prijs (price)
                decimal prijs;
                if (!decimal.TryParse(selectedItem.SubItems[4].Text.Replace("� ", ""),
                    NumberStyles.Currency, CultureInfo.CurrentCulture, out prijs))
                {
                    // Handle parsing error gracefully
                    MessageBox.Show("Invalid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update controls with selected data
                DrankNaamInput.Text = drankNaam;
                AlcoholischCheckBox.Checked = isAlcoholisch;
                VoorraadInput.Text = voorraadAantal.ToString();
                PrijsInput.Text = prijs.ToString();
            }
        }


        //DrankVoorraad CRUD
        private void AddDrankButton_Click(object sender, EventArgs e)
        {
            string drinkName = DrankNaamInput.Text;
            bool isAlcoholisch = AlcoholischCheckBox.Checked;
            int voorraadAantal = Convert.ToInt32(VoorraadInput.Text);
            decimal prijs = Convert.ToDecimal(PrijsInput.Text);

            // Show confirmation message
            DialogResult result = MessageBox.Show($"Are you sure you want to add {voorraadAantal} {drinkName} with Price �{prijs:F2}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Instantiate DrankDao
                DrankDao drankDao = new DrankDao();

                try
                {
                    // Add the drink to the database
                    drankDao.AddDrink(drinkName, isAlcoholisch, voorraadAantal, prijs);

                    // Display success message
                    MessageBox.Show("Drink added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear input fields
                    ClearVoorraadInputFields();
                }
                catch (Exception ex)
                {
                    // Display error message
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Optionally, perform any additional actions
                ShowDrankVoorraadPanel();
            }
        }
        private void EditDrankButton_Click(object sender, EventArgs e)
        {
            if (listViewVoorraad.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = listViewVoorraad.SelectedItems[0];

                // Get the old data from the selected item
                string oldDrankNaam = selectedItem.Text;
                string oldVoorraadAantal = selectedItem.SubItems[2].Text;
                string oldPrijs = selectedItem.SubItems[4].Text.Replace("� ", ""); // Remove currency symbol before parsing

                // Get new data from textboxes
                string newDrankNaam = DrankNaamInput.Text;
                bool isAlcoholisch = AlcoholischCheckBox.Checked;
                int newVoorraadAantal = int.Parse(VoorraadInput.Text);
                decimal newPrijs = decimal.Parse(PrijsInput.Text);

                // Confirmation message
                DialogResult result = MessageBox.Show($"Are you sure you want to edit:\n{oldVoorraadAantal} {oldDrankNaam} with price �{oldPrijs:F2}\nto:\n{newVoorraadAantal} {newDrankNaam} with price �{newPrijs:F2}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Call the DAO method to update the drink
                    DrankDao drankDao = new DrankDao();
                    try
                    {
                        drankDao.UpdateDrank(oldDrankNaam, newDrankNaam, isAlcoholisch, newVoorraadAantal, newPrijs);

                        // Display a success message to the user
                        MessageBox.Show("Drink updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear input fields
                        ClearVoorraadInputFields();
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions appropriately
                        MessageBox.Show("Error updating drink: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Inform the user to select a row in the ListView
                MessageBox.Show("Please select a drink to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Optionally, perform any additional actions
            ShowDrankVoorraadPanel();
        }
        private void DeleteDrankButton_Click(object sender, EventArgs e)
        {
            if (listViewVoorraad.SelectedItems.Count == 1)
            {
                // Vraag om bevestiging
                DialogResult result = MessageBox.Show("Are you sure you want to delete this drink?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Verkrijg de geselecteerde drank
                    string selectedDrinkName = listViewVoorraad.SelectedItems[0].Text;

                    DrankDao drankDao = new DrankDao();

                    // Voer de verwijdering uit
                    drankDao.DeleteDrank(selectedDrinkName);

                    // Wis tekstvakken
                    ClearVoorraadInputFields();

                    // Vernieuw de weergegeven dranken
                    ShowDrankVoorraadPanel();
                }
            }
            else
            {
                MessageBox.Show("Please select a drink to delete.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearVoorraadInputFields()
        {
            DrankNaamInput.Text = "";
            AlcoholischCheckBox.Checked = false;
            VoorraadInput.Text = "";
            PrijsInput.Text = "";
            listViewVoorraad.SelectedItems.Clear();
        }


        //Bestellingen
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
                        orderItem.Aantal = totaalaantalDrank;

                        listItem.SubItems[1].Text = totaalaantalDrank.ToString();
                        listItem.SubItems[2].Text = (decimal.Parse(Regex.Replace(listItem.SubItems[2].Text, @"[^\d,]", "")) + prijs).ToString("C");
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
            labelPrijs.Text = 0.00.ToString("C", new CultureInfo("nl-NL"));
        }

        private void drankBestellingenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDrankBestellingenPanel();
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

        private void bttnPlaatsBestelling_Click(object sender, EventArgs e)
        {
            if (listViewBestellingenStudenten.SelectedItems.Count == 1 && listViewTotaalBesteld.Items.Count != 0)
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

                        List<Drank> drankjes = Methodes.GetDrankjes();
                        DisplayDrankjes(drankjes);


                    }
                    else
                    {

                    }

                }
                else { }

            }
            else { MessageBox.Show("Selecteer alstublieft op wiens naam de bestelling is en voeg een drankje toe"); }

        }
        private void bttnOrder_Click(object sender, EventArgs e)
        {
            DisplayTotaalBesteld();
        }

        private void listViewBestellingenDrankjes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //Omzet
        public void ShowOmzetPanel()
        {
            Methodes.ShowPanel(pnlDrankOmzet);
        }
        public int GetAmountOfStudentsWithOrders()
        {
            BestellingService bestellingService = new BestellingService();
            return bestellingService.GetAmountOfStudentsOmzet(dtpDrankOmzetStart.Value, dtpDrankOmzetEind.Value);
        }
        private void DisplayOmzet()
        {
            listViewDrankOmzet.Items.Clear();
            int studentsOrdered = GetAmountOfStudentsWithOrders();
            OrderItemService orderItemService = new OrderItemService();
            int totalDrinksSold = orderItemService.RRGetTotalDrinksSold(dtpDrankOmzetStart.Value, dtpDrankOmzetEind.Value);
            decimal turnover = orderItemService.RRGetTurnover(dtpDrankOmzetStart.Value, dtpDrankOmzetEind.Value);
            ListViewItem li = new ListViewItem(Convert.ToString(totalDrinksSold));
            li.SubItems.Add(turnover.ToString("C", new CultureInfo("nl-NL")));
            li.SubItems.Add(Convert.ToString(studentsOrdered));
            listViewDrankOmzet.Items.Add(li);
            listViewDrankOmzet.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
            }
            else if (dtpDrankOmzetStart.Value > dtpDrankOmzetEind.Value)
            {
                MessageBox.Show("Start date can not be set after the end date");
                dtpDrankOmzetStart.Value = dtpDrankOmzetEind.Value;
            }
            else
            {
                DisplayOmzet();
            }
        }


        //Overig?
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

        private void activiteitenOverviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowActiviteitenPanel();
        }

        private void UpdateBegeleiders()
        {
            if (listViewActiviteiten.SelectedItems.Count > 0)
            {
                // Get the selected item
                Activiteit selectedactiviteit = (Activiteit)listViewActiviteiten.SelectedItems[0].Tag;

                DisplayBegeleiding(Methodes.GetBegeleiding(selectedactiviteit));
                DisplayVrijeDocenten(Methodes.GetVrijeDocent(selectedactiviteit));
            }
        }

        private void listViewActiviteiten_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBegeleiders();

        }

        private void listViewVrijeDocenten_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private ListViewItem GetSelect(System.Windows.Forms.ListView listView)
        {
            if (listView.SelectedItems.Count > 0)
            {
                // Get the selected item
                return listView.SelectedItems[0];
            }
            return null;
        }


        private void listViewVrijeDocenten_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem draggedItem = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;


                if (draggedItem.ListView == listViewBegeleiders)
                {

                    Docent docent = (Docent)GetSelect(listViewBegeleiders).Tag;
                    Activiteit activiteit = (Activiteit)GetSelect(listViewActiviteiten).Tag;

                    DialogResult result = MessageBox.Show($"Weet je zeker dat je {docent.Naam} {activiteit.ActiviteitNaam} niet meer wil laten begeleiden? ", "Bevestig", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        DocentService docentService = new DocentService();


                        docentService.MakeFree(docent, activiteit);
                        UpdateBegeleiders();
                    }
                }
            }

        }

        private void listViewVrijeDocenten_DragEnter(object sender, DragEventArgs e)
        {

            e.Effect = DragDropEffects.Move;



        }

        private void listViewVrijeDocenten_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listViewVrijeDocenten.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void listViewBegeleiders_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;


        }
        private void listViewBegeleiders_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listViewBegeleiders.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void listViewBegeleiders_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem draggedItem = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;


                if (draggedItem.ListView == listViewVrijeDocenten)
                {
                    DocentService docentService = new DocentService();
                    Docent docent = (Docent)GetSelect(listViewVrijeDocenten).Tag;
                    Activiteit activiteit = (Activiteit)GetSelect(listViewActiviteiten).Tag;

                    docentService.MakeBusy(docent, activiteit);
                    UpdateBegeleiders();
                }
            }



        }
    }
}
