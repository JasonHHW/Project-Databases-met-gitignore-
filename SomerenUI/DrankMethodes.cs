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
            // clear the listview before filling it
            listView.Items.Clear();


            foreach (Student student in students)
            {
                ListViewItem li = new ListViewItem(student.StudentId.ToString());
                li.Tag = student;   // link student object to listview item
                li.SubItems.Add(student.Naam);
                listView.Items.Add(li);

            }
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        

    }
    }
}
