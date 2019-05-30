using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;

namespace StudentExercisesMVC.Repositories
{
    public class StudentRepository
    {

        private static IConfiguration _config;

        public static void SetConfig(IConfiguration configuration)
        {
            _config = configuration;
        }

        public static SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public static List<Student> GetStudents()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            s.Id, s.firstName, s.lastName, s.slackHandle, s.cohortId,
                            c.Name AS 'Cohort Name'
                        FROM Student s
                        JOIN Cohort c ON s.CohortId = c.Id
                        ORDER BY s.lastName, s.firstName";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                            CurrentCohort = new Cohort
                            {
                                name = reader.GetString(reader.GetOrdinal("Cohort Name"))
                            }
                        };

                        students.Add(student);
                    }

                    reader.Close();

                    return students;
                }
            }

        }

        public static Student GetOneStudent(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            s.Id, s.firstName, s.lastName, s.slackHandle, s.cohortId,
                            c.Name AS 'Cohort Name'
                        FROM Student s
                        JOIN Cohort c ON s.CohortId = c.Id
                        WHERE s.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student Student = null;

                    if (reader.Read())
                    {
                        Student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("slackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("cohortId")),
                            CurrentCohort = new Cohort
                            {
                                name = reader.GetString(reader.GetOrdinal("Cohort Name"))
                            }
                        };
                    }
                    reader.Close();

                    return Student;
                }
            }

        }

        public static Student GetOneStudentWithExercises(int id)
        {
            Student student = GetOneStudent(id);
            student.Exercises = ExerciseRepository.GetAssignedExercisesByStudent(id);
            return student;
        }

        public static void CreateStudent(CreateStudentViewModel model)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student
                ( FirstName, LastName, SlackHandle, CohortId )
                VALUES
                ( @firstName, @lastName, @slackHandle, @cohortId )";
                    cmd.Parameters.Add(new SqlParameter("@firstName", model.student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", model.student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", model.student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", model.student.CohortId));
                    cmd.ExecuteNonQuery();

                    
                }
            }

        }

        public static void UpdateStudent(int id, EditStudentViewModel viewModel)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string command = @"UPDATE Student
                                            SET firstName=@firstName, 
                                            lastName=@lastName, 
                                            slackHandle=@slackHandle, 
                                            cohortId=@cohortId
                                            WHERE Id = @id
                                            DELETE FROM StudentExercise WHERE studentId =@id";

                    viewModel.SelectedExercises.ForEach(exerciseId =>
                    {
                        command += $" INSERT INTO StudentExercise (studentId, exerciseId) VALUES (@id, {exerciseId})";

                    });
                    cmd.CommandText = command;
                    cmd.Parameters.Add(new SqlParameter("@firstName", viewModel.student.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", viewModel.student.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", viewModel.student.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", viewModel.student.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();

                }
                
            }

        }

        public static void DeleteStudent(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM StudentExercise WHERE studentId = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();

                }
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    int rowsAffected = cmd.ExecuteNonQuery();

                }

            }

        }
    }
}
