using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using SomerenModel;
using System;

namespace SomerenDAL
{
    public class StudentDao : BaseDao
    {
        public List<Student> GetAllStudents()
        {
            string query = "SELECT [studentId], [firstName], [lastName], [phoneNumber], [class], [room] FROM [Student]";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Student> ReadTables(DataTable dataTable)
        {
            List<Student> students = new List<Student>();

            foreach (DataRow dr in dataTable.Rows)
            {
                Student student = new Student()
                {
                    StudentId = (int)dr["StudentId"],
                    FirstName = dr["Voornaam"].ToString(),
                    LastName = dr["Achternaam"].ToString(),
                    PhoneNumber = dr["Telefoonnummer"].ToString(),
                    Class = dr["Klas"].ToString(),
                    Room = dr["Kamer"].ToString()
                };
                students.Add(student);
            }
            return students;
        }

        public bool StudentExists(int studentId)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Student WHERE studentId = @StudentId";
                SqlParameter[] parameters =
                {
                    new SqlParameter("@StudentId", SqlDbType.VarChar) { Value = studentId }
                };
                DataTable result = ExecuteSelectQuery(query, parameters);
                int count = Convert.ToInt32(result.Rows[0][0]);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking if student exists: " + ex.Message, ex);
            }
        }

        public void AddStudent(int studentId, string studentFirstName, string studentLastName, string studentPhoneNumber, string studentClass, string studentRoom)
        {
            try
            {
                if (StudentExists(studentId))
                {
                    throw new Exception("Student with the same id already exists.");
                }

                // Define your SQL query for adding a student
                string query = "INSERT INTO Student (StudentId, fistName, lastName, phoneNumber, class, room) VALUES (@StudentId, @StudentFirstName, @StudentLastName, @StudentPhoneNumber, @StudentClass, @StudentRoom);";

                // Define parameters for your query
                SqlParameter[] parameters =
                {
                    new SqlParameter("@StudentId", SqlDbType.Int) { Value = studentId },
                    new SqlParameter("@StudentFirstName", SqlDbType.VarChar) { Value = studentFirstName },
                    new SqlParameter("@StudentLastName", SqlDbType.VarChar) { Value = studentLastName },
                    new SqlParameter("@StudentPhoneNumber", SqlDbType.VarChar) { Value = studentPhoneNumber },
                    new SqlParameter("@StudentClass", SqlDbType.VarChar) { Value = studentClass },
                    new SqlParameter("@StudentRoom", SqlDbType.VarChar) { Value = studentRoom }
                };

                // Execute the query
                ExecuteEditQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding student: " + ex.Message, ex);
            }
        }

        public List<string> GetStudentRooms()
        {
            List<string> studentRooms = new List<string>();

            try
            {
                string query = "SELECT roomCode FROM Room WHERE isSingleRoom = 0"; // Fetch student rooms where IsEenPersoons is false
                DataTable result = ExecuteSelectQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    studentRooms.Add(row["roomCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving student rooms: " + ex.Message, ex);
            }

            return studentRooms;
        }

        public void UpdateStudent(int studentId, int newStudentId, string newStudentFirstName, string newStudentLastName, string newStudentPhoneNumber, string newStudentClass, string newStudentRoom)
        {
            try
            {
                if (studentId != newStudentId && StudentExists(newStudentId))
                {
                    throw new Exception("Cannot update student. Student with the new id already exists.");
                }

                // Update the student
                string updateQuery = "UPDATE Student SET studentId = @NewStudentId, firstName = @FirstName, lastName = @LastName, phoneNumber = @PhoneNumber, class = @Class, room = @room " +
                                     "WHERE StudentId = @OldStudentId";

                // Define parameters for the query
                SqlParameter[] updateParameters =
                {
                    new SqlParameter("@OldStudentId", SqlDbType.Int) { Value = studentId },
                    new SqlParameter("@NewStudentId", SqlDbType.Int) { Value = newStudentId },
                    new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = newStudentFirstName },
                    new SqlParameter("@LastName", SqlDbType.VarChar) { Value = newStudentLastName },
                    new SqlParameter("@PhoneNumber", SqlDbType.VarChar) { Value = newStudentPhoneNumber },
                    new SqlParameter("@Class", SqlDbType.VarChar) { Value = newStudentClass },
                    new SqlParameter("@Room", SqlDbType.VarChar) { Value = newStudentRoom }
                };

                // Execute the update query
                ExecuteEditQuery(updateQuery, updateParameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating student: " + ex.Message, ex);
            }
        }
        public void DeleteStudent(int studentId)
        {
            try
            {
                // Delete student where studentId = studentId
                string deleteStudentQuery = "DELETE FROM Student WHERE studentId = @StudentId";
                SqlParameter[] studentParameters = { new SqlParameter("@StudentId", studentId) };
                ExecuteEditQuery(deleteStudentQuery, studentParameters);

                // Succesmelding of andere logica na succesvol verwijderen
                Console.WriteLine("Student + order- and activity information deleted successfully");
            }
            catch (Exception ex)
            {
                // Foutafhandeling
                Console.WriteLine("There was an error while deleting the student and associated information: " + ex.Message);
            }
        }
    }
}