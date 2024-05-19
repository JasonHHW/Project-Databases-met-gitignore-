using System;

namespace SomerenModel
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Class { get; set; }
        public string Room { get; set; }

        public string Name { get { return FirstName + " " + LastName; } }


        public override string ToString()
        {
            return ($"{StudentId} {LastName} {LastName}");
        }
    }
}