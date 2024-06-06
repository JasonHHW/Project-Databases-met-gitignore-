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
using System.Diagnostics;

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


        //Student Overview
        private void ShowStudentsPanel()
        {
            //Displays Student Panel
            Methodes.ShowPanel(pnlStudents);

            try
            {
                // Get and display all students
                List<Student> students = GetStudents();
                DisplayStudents(students);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the students: " + e.Message);
            }
        }

        private void studentenOverviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowStudentsPanel();
        }
        private void DisplayStudents(List<Student> students)
        {
            // Clear the listview before filling it
            listViewStudentsOverview.Items.Clear();

            //Puts each student in the listview
            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.StudentId.ToString());

                li.SubItems.Add(student.Name);
                li.SubItems.Add(student.PhoneNumber);
                li.SubItems.Add(student.Class);
                li.SubItems.Add(student.Room);

                listViewStudentsOverview.Items.Add(li);
            }
        }


        // Manage Students
        private void ShowManageStudentsPanel()
        {
            //Displays Manage Student Panel
            Methodes.ShowPanel(pnlManageStudents);

            try
            {
                // Get and display all students
                List<Student> students = GetStudents();
                DisplayStudentsMS(students);
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
        private void DisplayStudentsMS(List<Student> students)
        {
            // Clear the listview before filling it
            listViewStudents.Items.Clear();

            //Puts each student in the listview
            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.StudentId.ToString());

                li.SubItems.Add(student.Name);
                li.SubItems.Add(student.PhoneNumber);
                li.SubItems.Add(student.Class);
                li.SubItems.Add(student.Room);

                listViewStudents.Items.Add(li);
            }
        }
        private void listViewStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewStudents.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedStudent = listViewStudents.SelectedItems[0];

                // Extract data from the selected ListViewItem
                int studentId = int.Parse(selectedStudent.Text);
                string studentName = selectedStudent.SubItems[1].Text;
                string studentPhoneNumber = selectedStudent.SubItems[2].Text;
                string studentClass = selectedStudent.SubItems[3].Text;
                string studentRoom = selectedStudent.SubItems[4].Text;

                //Splits Naam into Voor- en Achternaam
                string[] parts = studentName.Split(new char[] { ' ' }, 2);
                string studentFirstName = parts[0];
                string studentLastName = parts.Length > 1 ? parts[1] : "";

                // Update TextBoxes with selected data
                StudentIdInput.Text = studentId.ToString();
                StudentFirstNameInput.Text = studentFirstName;
                StudentLastNameInput.Text = studentLastName;
                StudentPhoneNumberInput.Text = studentPhoneNumber;
                StudentClassInput.Text = studentClass;

                //Selects Student Kamer in ComboBox
                int index = studentRoomComboBox.FindStringExact(studentRoom);
                if (index != -1)
                {
                    studentRoomComboBox.SelectedIndex = index;
                }
                else
                {
                    studentRoomComboBox.SelectedIndex = -1;
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
                studentRoomComboBox.Items.Clear();

                // Add retrieved student rooms to the ComboBox
                foreach (string room in studentRooms)
                {
                    studentRoomComboBox.Items.Add(room);
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
            StudentFirstNameInput.Text = "";
            StudentLastNameInput.Text = "";
            StudentPhoneNumberInput.Text = "";
            StudentClassInput.Text = "";
            studentRoomComboBox.SelectedIndex = -1;
            listViewStudents.SelectedItems.Clear();
        }
        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowManageStudentsPanel();
        }

        //Student CRUD
        private void AddStudentButton_Click(object sender, EventArgs e)
        {
            //Takes all data drom TextBoxes and ComboBox
            int studentId = int.Parse(StudentIdInput.Text);
            string studentFirstName = StudentFirstNameInput.Text;
            string studentLastName = StudentLastNameInput.Text;
            string studentPhoneNumber = StudentPhoneNumberInput.Text;
            string studentClass = StudentClassInput.Text;
            string studentRoom = studentRoomComboBox.SelectedItem.ToString();

            // Show confirmation message
            DialogResult result = MessageBox.Show($"Are you sure you want to add {studentFirstName} {studentLastName} with StudentId {studentId}, phonenumber {studentPhoneNumber}, in class {studentClass} and room {studentRoom}?",
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
                    studentDao.AddStudent(studentId, studentFirstName, studentLastName, studentPhoneNumber, studentClass, studentRoom);

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
            if (listViewStudents.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedStudent = listViewStudents.SelectedItems[0];

                // Get the old data from the selected item
                // StudentId for Database Querry, everything else for the Comfermation Message
                int studentId = int.Parse(selectedStudent.Text);

                string studentName = selectedStudent.SubItems[1].Text;
                string studentPhoneNumber = selectedStudent.SubItems[2].Text;
                string studentClass = selectedStudent.SubItems[3].Text;
                string studentRoom = selectedStudent.SubItems[4].Text;

                // Get new data from textboxes & ComboBox
                int newStudentId = int.Parse(StudentIdInput.Text);
                string newStudentFirstName = StudentFirstNameInput.Text;
                string newStudentLastName = StudentLastNameInput.Text;
                string newStudentPhoneNumber = StudentPhoneNumberInput.Text;
                string newStudentClass = StudentClassInput.Text;
                string newStudentRoom = studentRoomComboBox.SelectedItem.ToString();

                // Confirmation message
                DialogResult result = MessageBox.Show($"Are you sure you want to edit:\n{studentName} with Id {studentId} with phonenumber {studentPhoneNumber} in class {studentClass} and room {studentRoom}" +
                                                        $"\nto:\n" +
                                                        $"{newStudentFirstName} {newStudentLastName} with Id {newStudentId} with phonenumber {newStudentPhoneNumber} in class {newStudentClass} and room {newStudentRoom}?",
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
                        studentDao.UpdateStudent(studentId, newStudentId, newStudentFirstName, newStudentLastName, newStudentPhoneNumber, newStudentClass, newStudentRoom);

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
            if (listViewStudents.SelectedItems.Count == 1)
            {
                // Ask for Confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to delete this student?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Get studentId from selectedListView
                    int selectedStudentId = int.Parse(listViewStudents.SelectedItems[0].Text);

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

        //Teacher
        private void ShowTeachersPanel()
        {
            //Shows teacher panel
            Methodes.ShowPanel(pnlTeachers);

            try
            {
                // get and display all teachers
                List<Teacher> teachers = GetTeachers();
                DisplayTeachers(teachers);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the teachers: " + e.Message);
            }
        }
        private List<Teacher> GetTeachers()
        {
            TeacherService teacherService = new TeacherService();
            List<Teacher> teachers = teacherService.GetTeachers();
            return teachers;
        }
        private void DisplayTeachers(List<Teacher> teachers)
        {
            //Clears data from listView
            listViewTeachers.Items.Clear();

            // Displays each Teachers
            foreach (Teacher teacher in teachers)
            {
                ListViewItem li = new ListViewItem(teacher.Name);
                li.SubItems.Add(teacher.TeacherId.ToString());
                li.SubItems.Add(teacher.PhoneNumber.ToString());
                li.SubItems.Add(teacher.DateOfBirth.ToString("dd-MM-yyyy"));
                li.Tag = teacher;   // link docent object to listview item
                listViewTeachers.Items.Add(li);
            }
            listViewTeachers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTeachersPanel();
        }

        //Activities
        private void ShowActiviteitenPanel()
        {
            //Shows Activiteiten panel
            Methodes.ShowPanel(pnlActivities);

            try
            {
                // get and display all students
                List<ActivityModel> activiteiten = Methodes.GetActivities();
                DisplayActiviteiten(activiteiten);

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }

        }
        private List<ActivityModel> GetActivities()
        {
            ActivityService activityService = new ActivityService();
            List<ActivityModel> activities = activityService.GetActivities();
            return activities;
        }
        private void DisplayActiviteiten(List<ActivityModel> activiteiten)
        {
            // clear the listview before filling it
            listViewActivities.Items.Clear();

            //Displays each activiteit
            foreach (ActivityModel activiteit in activiteiten)
            {
                ListViewItem li = new ListViewItem(activiteit.ActivityName);
                li.SubItems.Add(activiteit.ActivityId.ToString());
                li.SubItems.Add(activiteit.StartTime.ToString("dd-MM-yyyy HH:mm"));
                li.SubItems.Add(activiteit.EndTime.ToString("dd-MM-yyyy HH:mm"));
                li.Tag = activiteit;   // link student object to listview item

                listViewActivities.Items.Add(li);
            }
            listViewActivities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private void activitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowActiviteitenPanel();
        }
        private void activiteitenOverviewToolStripMenuItem_Click(object sender, EventArgs e)
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
                List<ActivityModel> activities = GetActivities();
                DisplayActiviteitenMAP(activities);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }
        }
        private void DisplayStudentsMAP(ActivityModel activity)
        {
            ParticipationService participationService = new ParticipationService(); // gets all the participating and non participating students from the database
            List<Student> participants = participationService.GetParticipantsFromActivityId(activity);
            List<Student> nonParticipants = participationService.GetNonParticipants(activity);

            listViewMAPParticipatingStudents.Clear();
            listViewMAPNonParticipatingStudents.Clear();

            foreach (Student student in participants) // each foreach fills the participants and non participants listview respectively
            {
                ListViewItem li = new ListViewItem(student.Name);
                li.SubItems.Add(student.StudentId.ToString());
                li.Tag = student;
                listViewMAPParticipatingStudents.Items.Add(li);
            }
            foreach (Student student in nonParticipants)
            {
                ListViewItem li = new ListViewItem(student.Name);
                li.SubItems.Add(student.StudentId.ToString());
                li.Tag = student;
                listViewMAPNonParticipatingStudents.Items.Add(li);
            }
        }
        private void DisplayActiviteitenMAP(List<ActivityModel> activities)
        {
            // clear the listview before filling it
            listViewMAPActivities.Items.Clear();

            foreach (ActivityModel activity in activities)
            {
                ListViewItem li = new ListViewItem(activity.ActivityName);
                li.SubItems.Add(activity.ActivityId.ToString());
                li.SubItems.Add(activity.StartTime.ToString("dd-MM-yyyy HH:mm"));
                li.SubItems.Add(activity.EndTime.ToString("dd-MM-yyyy HH:mm"));
                li.Tag = activity;   // link student object to listview item

                listViewMAPActivities.Items.Add(li);

            }
            listViewActivitiesMAS.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private void deelnemersBeherenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDeelnemersBeherenPanel();
        }
        private void MAPActivitySelected(object sender, EventArgs e)
        {
            if (listViewMAPActivities.SelectedItems.Count! > 0)
            {
                DisplayStudentsMAP((ActivityModel)listViewMAPActivities.SelectedItems[0].Tag);
            }
        }

        private void MAPRemoveStudent(object sender, EventArgs e)
        {
            ParticipationService deelnameService = new ParticipationService();
            if (listViewMAPParticipatingStudents.SelectedItems.Count > 0)
            {
                var ConfirmRemoveStudents = MessageBox.Show("Are you sure you want to delete the student from this activity?", "Remove student from activity", MessageBoxButtons.YesNo);
                if (ConfirmRemoveStudents == DialogResult.Yes)
                {
                    deelnameService.RemoveParticipatingStudent((ActivityModel)listViewMAPActivities.SelectedItems[0].Tag, (Student)listViewMAPParticipatingStudents.SelectedItems[0].Tag);
                    DisplayStudentsMAP((ActivityModel)listViewMAPActivities.SelectedItems[0].Tag);
                }
            }
            else
            {
                MessageBox.Show("Please select a student before proceeding");
            }

        }

        private void MAPAddStudent(object sender, EventArgs e)
        {
            ParticipationService participationService = new ParticipationService();
            if (listViewMAPNonParticipatingStudents.SelectedItems.Count > 0)
            {
                participationService.AddParticipatingStudent((ActivityModel)listViewMAPActivities.SelectedItems[0].Tag, (Student)listViewMAPNonParticipatingStudents.SelectedItems[0].Tag);
                DisplayStudentsMAP((ActivityModel)listViewMAPActivities.SelectedItems[0].Tag);
            }
            else
            {
                MessageBox.Show("Please select a student before proceeding");
            }
        }

        //Supervision

        private void ShowManageSupervisorsPanel()
        {
            //Shows Activiteiten panel
            Methodes.ShowPanel(pnlManageSupervisors);

            try
            {
                // get and display all students
                List<ActivityModel> activities = Methodes.GetActivities();
                DisplayActivitiesMAS(activities);

            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the activities: " + e.Message);
            }

        }
        private void manageSupervisorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowManageSupervisorsPanel();
        }
        private void UpdateBegeleiders()
        {
            if (listViewActivitiesMAS.SelectedItems.Count > 0)
            {
                // Get the selected item
                ActivityModel selectedActivity = (ActivityModel)listViewActivitiesMAS.SelectedItems[0].Tag;

                DisplaySupervision(Methodes.GetSupervisors(selectedActivity));
                DisplayFreeTeachers(Methodes.GetFreeTeachers(selectedActivity));
            }
        }
        private void listViewActiviteiten_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBegeleiders();
        }
        private void listViewVrijeDocenten_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateBegeleiders();
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

        //Display Supervisor things
        private void DisplayActivitiesMAS(List<ActivityModel> activities)
        {
            // clear the listview before filling it
            listViewActivitiesMAS.Items.Clear();

            //Displays each activiteit
            foreach (ActivityModel activity in activities)
            {
                ListViewItem li = new ListViewItem(activity.ActivityName);
                li.SubItems.Add(activity.StartTime.ToString("dd-MM-yyyy HH:mm"));
                li.SubItems.Add(activity.EndTime.ToString("dd-MM-yyyy HH:mm"));

                li.Tag = activity;   // link student object to listview item

                listViewActivitiesMAS.Items.Add(li);
            }
            listViewActivitiesMAS.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        private void DisplaySupervision(List<Teacher> teachers)
        {
            listViewSupervisors.Items.Clear();

             
            foreach (Teacher supervisor in teachers)
            {
                ListViewItem li = new ListViewItem(supervisor.TeacherId.ToString());
                li.SubItems.Add(supervisor.Name);

                li.Tag = supervisor;   // link student object to listview item


                listViewSupervisors.Items.Add(li);

            }

            listViewSupervisors.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }
        private void DisplayFreeTeachers(List<Teacher> teachers)
        {
            listViewFreeTeachers.Items.Clear();
            foreach (Teacher freeTeacher in teachers)
            {
                ListViewItem li = new ListViewItem(freeTeacher.TeacherId.ToString());
                li.SubItems.Add(freeTeacher.Name);

                li.Tag = freeTeacher;   // link student object to listview item


                listViewFreeTeachers.Items.Add(li);
            }
            listViewFreeTeachers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        //Supervision ListViews DragDrops
        private void listViewVrijeDocenten_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem draggedItem = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;


                if (draggedItem.ListView == listViewSupervisors)
                {

                    Teacher teacher = (Teacher)GetSelect(listViewSupervisors).Tag;
                    ActivityModel activity = (ActivityModel)GetSelect(listViewActivitiesMAS).Tag;

                    DialogResult result = MessageBox.Show($"Do you really want to stop {teacher.Name} from supervising {activity.ActivityName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        TeacherService teacherService = new TeacherService();

                        teacherService.MakeFree(teacher, activity);
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
            listViewFreeTeachers.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void listViewBegeleiders_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void listViewBegeleiders_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listViewSupervisors.DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void listViewBegeleiders_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                ListViewItem draggedItem = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;


                if (draggedItem.ListView == listViewFreeTeachers)
                {
                    TeacherService teacherService = new TeacherService();
                    Teacher teacher = (Teacher)GetSelect(listViewFreeTeachers).Tag;
                    ActivityModel activity = (ActivityModel)GetSelect(listViewActivitiesMAS).Tag;

                    teacherService.MakeBusy(teacher, activity);
                    UpdateBegeleiders();
                }
            }
        }

        // Rooms
        private void ShowRoomsPanel()
        {
            Methodes.ShowPanel(pnlKamers);

            try
            {
                // get and display all students
                List<Room> rooms = Methodes.GetRooms();
                DisplayRooms(rooms);


            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the rooms: " + e.Message);
            }

        }
        private void DisplayRooms(List<Room> rooms)
        {
            // clear the listview before filling it
            listViewRooms.Items.Clear();


            foreach (Room room in rooms)
            {
                ListViewItem li = new ListViewItem(room.RoomCodeChar);
                li.Tag = room;   // link student object to listview item
                li.SubItems.Add(room.SleepingPlaces.ToString());
                li.SubItems.Add(room.Type.ToString());


                listViewRooms.Items.Add(li);

            }
            listViewRooms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowRoomsPanel();
        }


        //Drank
        private List<Drink> GetDrinks()
        {
            DrinkService drinkService = new DrinkService();
            List<Drink> drinks = drinkService.GetDrinks();
            return drinks;
        }
        private void DisplayDrankjes(List<Drink> drinks)
        {
            listViewBestellingenDrankjes.Items.Clear();
            foreach (Drink drink in drinks)
            {
                ListViewItem li = new ListViewItem(drink.DrinkName);
                li.Tag = drink;
                li.SubItems.Add($"{drink.Price:C}");
                li.SubItems.Add(drink.Type);
                li.SubItems.Add(drink.Stock.ToString());
                listViewBestellingenDrankjes.Items.Add(li);

            }
            listViewBestellingenDrankjes.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }


        //Voorraad
        private void ShowDrinkStockPanel()
        {
            Methodes.ShowPanel(pnlDrankVoorraad);

            try
            {
                // get and display all students
                List<Drink> drinks = GetDrinks();
                DisplayDrinkStock(drinks);
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong while loading the stock: " + e.Message);
            }
        }
        private void DisplayDrinkStock(List<Drink> stock)
        {
            try
            {
                var orderedDrankList = stock.OrderBy(d => d.Stock).ToList();

                // Clear existing items
                listViewStock.Items.Clear();

                // Populate the ListView with drink stock information
                foreach (Drink item in stock)
                {
                    ListViewItem listViewItem = new ListViewItem(item.DrinkName);
                    listViewItem.SubItems.Add(item.IsAlcoholic ? "Yes" : "No");
                    listViewItem.SubItems.Add(item.Stock.ToString());
                    listViewItem.SubItems.Add(item.TotalConsumed.ToString());
                    listViewItem.SubItems.Add(item.Price.ToString("€0.00"));

                    // Add stock status index as a subitem
                    listViewItem.SubItems.Add((item.Stock < 10) ? "Insufficient" : "Sufficient");

                    listViewStock.Items.Add(listViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading drink stock information: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void drinkStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDrinkStockPanel();
        }
        private void listViewStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewStock.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = listViewStock.SelectedItems[0];

                // Extract data from the selected ListViewItem
                string drinkName = selectedItem.Text;
                string isAlcoholicText = selectedItem.SubItems[1].Text;

                // Convert text representation of "Alcoholisch" to boolean
                bool isAlcoholic = isAlcoholicText.Equals("Yes", StringComparison.OrdinalIgnoreCase);

                // Update controls with selected data
                DrinkNameInput.Text = drinkName;
                AlcoholicCheckBox.Checked = isAlcoholic;

                // Parse VoorraadAantal (stock quantity)
                int stockAmount;
                if (!int.TryParse(selectedItem.SubItems[2].Text, out stockAmount))
                {
                    // Handle parsing error gracefully
                    MessageBox.Show("Invalid stock quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse Prijs (price)
                decimal price;
                if (!decimal.TryParse(selectedItem.SubItems[4].Text.Replace("€ ", ""),
                    NumberStyles.Currency, CultureInfo.CurrentCulture, out price))
                {
                    // Handle parsing error gracefully
                    MessageBox.Show("Invalid price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update controls with selected data
                DrinkNameInput.Text = drinkName;
                AlcoholicCheckBox.Checked = isAlcoholic;
                StockInput.Text = stockAmount.ToString();
                PriceInput.Text = price.ToString();
            }
        }


        //DrankVoorraad CRUD
        private void AddDrankButton_Click(object sender, EventArgs e)
        {
            string drinkName = DrinkNameInput.Text;
            bool isAlcoholic = AlcoholicCheckBox.Checked;
            int stockAmount = Convert.ToInt32(StockInput.Text);
            decimal price = Convert.ToDecimal(PriceInput.Text);

            // Show confirmation message
            DialogResult result = MessageBox.Show($"Are you sure you want to add {stockAmount} {drinkName} with Price €{price:F2}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Instantiate DrankDao
                DrinkDao drankDao = new DrinkDao();

                try
                {
                    // Add the drink to the database
                    drankDao.AddDrink(drinkName, isAlcoholic, stockAmount, price);

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
                ShowDrinkStockPanel();
            }
        }
        private void EditDrankButton_Click(object sender, EventArgs e)
        {
            if (listViewStock.SelectedItems.Count > 0)
            {
                // Get the selected item
                ListViewItem selectedItem = listViewStock.SelectedItems[0];

                // Get the old data from the selected item
                string oldDrankNaam = selectedItem.Text;
                string oldVoorraadAantal = selectedItem.SubItems[2].Text;
                string oldPrijs = selectedItem.SubItems[4].Text.Replace("� ", ""); // Remove currency symbol before parsing

                // Get new data from textboxes
                string newDrankNaam = DrinkNameInput.Text;
                bool isAlcoholisch = AlcoholicCheckBox.Checked;
                int newVoorraadAantal = int.Parse(StockInput.Text);
                decimal newPrijs = decimal.Parse(PriceInput.Text);

                // Confirmation message
                DialogResult result = MessageBox.Show($"Are you sure you want to edit:\n{oldVoorraadAantal} {oldDrankNaam} with price �{oldPrijs:F2}\nto:\n{newVoorraadAantal} {newDrankNaam} with price �{newPrijs:F2}?",
                                           "Confirmation",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Call the DAO method to update the drink
                    DrinkDao drankDao = new DrinkDao();
                    try
                    {
                        drankDao.UpdateDrink(oldDrankNaam, newDrankNaam, isAlcoholisch, newVoorraadAantal, newPrijs);

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
            ShowDrinkStockPanel();
        }
        private void DeleteDrankButton_Click(object sender, EventArgs e)
        {
            if (listViewStock.SelectedItems.Count == 1)
            {
                // Vraag om bevestiging
                DialogResult result = MessageBox.Show("Are you sure you want to delete this drink?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Verkrijg de geselecteerde drank
                    string selectedDrinkName = listViewStock.SelectedItems[0].Text;

                    DrinkDao drankDao = new DrinkDao();

                    // Voer de verwijdering uit
                    drankDao.DeleteDrink(selectedDrinkName);

                    // Wis tekstvakken
                    ClearVoorraadInputFields();

                    // Vernieuw de weergegeven dranken
                    ShowDrinkStockPanel();
                }
            }
            else
            {
                MessageBox.Show("Please select a drink to delete.", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearVoorraadInputFields()
        {
            DrinkNameInput.Text = "";
            AlcoholicCheckBox.Checked = false;
            StockInput.Text = "";
            PriceInput.Text = "";
            listViewStock.SelectedItems.Clear();
        }


        //Bestellingen
        private void ShowDrankBestellingenPanel()
        {
            Methodes.ShowPanel(pnlDrankBestellingen);
            try
            {
                List<Drink> drankjes = GetDrinks();
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
                string voornaam = selectedStudent.FirstName;

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

                        List<Drink> drankjes = Methodes.GetDrinks();
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
            DisplayOmzet();
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

    }
}
