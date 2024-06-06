using SomerenModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace SomerenUI
{
    public static partial class Methodes
    {
        public static void DisplayStudents(List<Student> students, ListView listView)
        {
            // verwijdert alles uit de listview voordat hij items toevoegt.
            listView.Items.Clear();


            foreach (Student student in students)
            {  // verbindt  elke student in students met een listview item en weergeeft zijn naam in de listtview
                ListViewItem li = new ListViewItem(student.StudentId.ToString());
                li.Tag = student;  
                li.SubItems.Add(student.Name);
                listView.Items.Add(li);

            }
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        

    }
    }
}
