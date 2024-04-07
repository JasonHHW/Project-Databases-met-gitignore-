using SomerenModel;
using SomerenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenUI
{
    public static partial class Methodes 
    {

        public static List<Teacher> GetTeachers()
        {
            TeacherService teacherService = new TeacherService();
            List<Teacher> teachers = teacherService.GetTeachers();
            return teachers;
        }
        public static  List<Student> GetStudents()
        {
            StudentService studentService = new StudentService();
            List<Student> students = studentService.GetStudents();
            return students;
        }
        public static List<Room> GetRooms()
        {
            RoomService RoomService = new RoomService();
            List<Room> rooms = RoomService.GetRooms();
            return rooms;
        }
        public static List<ActivityModel> GetActivities()
        {
            ActivityService activityService = new ActivityService();
            List<ActivityModel> activities = activityService.GetActivities();
            return activities;
        }
        public static List<Drink> GetDrinks()
        {
            DrinkService drinkService = new DrinkService();
            List<Drink> drinks = drinkService.GetDrinks();
            return drinks;
        }

        public static List<Teacher> GetSupervisors(ActivityModel activiteit)
        {
            TeacherService teacherService = new TeacherService();
            List<Teacher> teachers = teacherService.GetSupervisors(activiteit);
            return teachers;
        }
        public static List<Teacher> GetFreeTeachers(ActivityModel activity)
        {
            TeacherService teacherService = new TeacherService();
            List<Teacher> teachers = teacherService.GetFreeTeachers(activity);
            return teachers;
        }
    }
}
