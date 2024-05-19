using System;

namespace SomerenModel
{
    public class Teacher
    {
        public int TeacherId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string Room {  get; set; }
        public string Name { get { return FirstName + " " + LastName; } }
        public override string ToString()
        {
            return ($"{TeacherId} {FirstName} {LastName}");
        }

    }
}