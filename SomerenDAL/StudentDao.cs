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
            string query = "SELECT [StudentId], [Voornaam], [Achternaam], [Telefoonnummer], [Klas], [Kamer] FROM [Student]";
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
                    Voornaam = dr["Voornaam"].ToString(),
                    Achternaam = dr["Achternaam"].ToString(),
                    Telefoonnummer = dr["Telefoonnummer"].ToString(),
                    Klas = dr["Klas"].ToString(),
                    Kamer = dr["Kamer"].ToString()
                };
                students.Add(student);
            }
            return students;
        }

        public bool StudentExists(int studentId)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Student WHERE StudentId = @StudentId";
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

        public void AddStudent(int studentId, string studentVoornaam, string studentAchternaam, string studentTelefoonnummer, string studentKlas, string studentKamer)
        {
            try
            {
                if (StudentExists(studentId))
                {
                    throw new Exception("Student with the same id already exists.");
                }

                // Define your SQL query for adding a student
                string query = "INSERT INTO Student (StudentId, Voornaam, Achternaam, Telefoonnummer, Klas, Kamer) VALUES (@StudentId, @StudentVoornaam, @StudentAchternaam, @StudentTelefoonnummer, @StudentKlas, @StudentKamer);";

                // Define parameters for your query
                SqlParameter[] parameters =
                {
                    new SqlParameter("@StudentId", SqlDbType.Int) { Value = studentId },
                    new SqlParameter("@StudentVoornaam", SqlDbType.VarChar) { Value = studentVoornaam },
                    new SqlParameter("@StudentAchternaam", SqlDbType.VarChar) { Value = studentAchternaam },
                    new SqlParameter("@StudentTelefoonnummer", SqlDbType.VarChar) { Value = studentTelefoonnummer },
                    new SqlParameter("@StudentKlas", SqlDbType.VarChar) { Value = studentKlas },
                    new SqlParameter("@StudentKamer", SqlDbType.VarChar) { Value = studentKamer }
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
                string query = "SELECT KamerCode FROM Kamer WHERE IsEenPersoons = 0"; // Fetch student rooms where IsEenPersoons is false
                DataTable result = ExecuteSelectQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    studentRooms.Add(row["KamerCode"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving student rooms: " + ex.Message, ex);
            }

            return studentRooms;
        }

        public void UpdateStudent(int studentId, int newStudentId, string newStudentVoornaam, string newStudentAchternaam, string newStudentTelefoonnummer, string newStudentKlas, string newStudentKamer)
        {
            try
            {
                if (studentId != newStudentId && StudentExists(newStudentId))
                {
                    throw new Exception("Cannot update student. Student with the new id already exists.");
                }

                // Update the student
                string updateQuery = "UPDATE Student SET StudentId = @NewStudentId, Voornaam = @Voornaam, Achternaam = @Achternaam, Telefoonnummer = @Telefoonnummer, Klas = @Klas, Kamer = @Kamer " +
                                     "WHERE StudentId = @OldStudentId";

                // Define parameters for the query
                SqlParameter[] updateParameters =
                {
                    new SqlParameter("@OldStudentId", SqlDbType.Int) { Value = studentId },
                    new SqlParameter("@NewStudentId", SqlDbType.Int) { Value = newStudentId },
                    new SqlParameter("@Voornaam", SqlDbType.VarChar) { Value = newStudentVoornaam },
                    new SqlParameter("@Achternaam", SqlDbType.VarChar) { Value = newStudentAchternaam },
                    new SqlParameter("@Telefoonnummer", SqlDbType.VarChar) { Value = newStudentTelefoonnummer },
                    new SqlParameter("@Klas", SqlDbType.VarChar) { Value = newStudentKlas },
                    new SqlParameter("@Kamer", SqlDbType.VarChar) { Value = newStudentKamer }
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
                string deleteStudentQuery = "DELETE FROM Student WHERE StudentId = @StudentId";
                SqlParameter[] studentParameters = { new SqlParameter("@StudentId", studentId) };
                ExecuteEditQuery(deleteStudentQuery, studentParameters);

                // Succesmelding of andere logica na succesvol verwijderen
                Console.WriteLine("Student + bestelling- en activiteit informatie succesvol verwijderd.");
            }
            catch (Exception ex)
            {
                // Foutafhandeling
                Console.WriteLine("Er is een fout opgetreden bij het verwijderen van de student en gekoppelde informatie: " + ex.Message);
            }
        }
    }
}