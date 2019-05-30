using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Repositories
{
    public class ExerciseRepository
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



        public static List<Exercise> GetAllExercises()
        {
            List<Exercise> allExercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allExercises.Add(new Exercise
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),
                        });
                    }
                    reader.Close();
                }
            }

            return allExercises;
        }

        public static Exercise GetSingleExercise(int id)
        {
            Exercise exercise = null;

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise WHERE Id=@id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        exercise = new Exercise
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),
                        };
                    }
                    reader.Close();
                }
            }

            return exercise;

        }

        public static List<StudentExercise> GetAssignedExercisesByStudent(int studentId)
        {
            List<StudentExercise> assignedExercises = new List<StudentExercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT se.Id AS 'Student Exercise Id', se.studentId AS 'Student Exercise Student Id', 
                      se.exerciseId AS 'Student Exercise Exercise Id', 
                      se.isComplete AS 'Is Complete', 
                      e.Id AS 'Exercise Id', 
                      e.Name AS 'Exercise Name', 
                      e.Language AS 'Exercise Language' 
                      FROM StudentExercise se 
                      JOIN Exercise e
                      ON e.Id=se.ExerciseId 
                      WHERE se.StudentId=@id";

                    cmd.Parameters.Add(new SqlParameter("@id", studentId));
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        assignedExercises.Add(new StudentExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Student Exercise Id")),
                            studentId = reader.GetInt32(reader.GetOrdinal("Student Exercise Student Id")),
                            exerciseId = reader.GetInt32(reader.GetOrdinal("Student Exercise Exercise Id")),
                            isComplete= reader.GetBoolean(reader.GetOrdinal("Is Complete")),
                            Exercise = new Exercise
                            {
                                id = reader.GetInt32(reader.GetOrdinal("Exercise Id")),
                                Name = reader.GetString(reader.GetOrdinal("Exercise Name")),
                                Language = reader.GetString(reader.GetOrdinal("Exercise Language"))
                            }
                        }
                            );
                    }
                    reader.Close();
                }
            }
            return assignedExercises;

        }

        public static List<StudentExercise> GetAssignedStudentsByExercise(int exerciseId)
        {
            List<StudentExercise> assignedExercises = new List<StudentExercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT se.Id AS 'Student Exercise Id', se.studentId AS 'Student Exercise Student Id', 
                      se.exerciseId AS 'Student Exercise Exercise Id', 
                      se.isComplete AS 'Is Complete', 
                      e.Id AS 'Exercise Id', 
                      e.Name AS 'Exercise Name', 
                      e.Language AS 'Exercise Language', 
                      s.firstName AS 'Student First Name',
                      s.lastName AS 'Student Last Name'
                      FROM StudentExercise se 
                      JOIN Exercise e
                      ON e.Id=se.ExerciseId 
                      JOIN Student s ON se.studentId = s.Id
                      WHERE se.ExerciseId = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", exerciseId));
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        assignedExercises.Add(new StudentExercise
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Student Exercise Id")),
                            studentId = reader.GetInt32(reader.GetOrdinal("Student Exercise Student Id")),
                            exerciseId = reader.GetInt32(reader.GetOrdinal("Student Exercise Exercise Id")),
                            isComplete = reader.GetBoolean(reader.GetOrdinal("Is Complete")),
                            Exercise = new Exercise
                            {
                                id = reader.GetInt32(reader.GetOrdinal("Exercise Id")),
                                Name = reader.GetString(reader.GetOrdinal("Exercise Name")),
                                Language = reader.GetString(reader.GetOrdinal("Exercise Language"))
                            },
                            Student = new Student
                            {
                                FirstName = reader.GetString(reader.GetOrdinal("Student First Name")),
                                LastName = reader.GetString(reader.GetOrdinal("Student Last Name"))
                            }
                        }
                            );
                    }
                    reader.Close();
                }
            }
            return assignedExercises;
        }

        public static List<StudentExercise> GetAllStudentExercises()
        {
            List<StudentExercise> allStudentExercises = new List<StudentExercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT e.Id AS 'Exercise Id', e.Name, e.Language, se.Id AS 'SE Id', se.StudentId, se.isComplete FROM Exercise e LEFT JOIN StudentExercise se ON e.Id = se.exerciseId";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        allStudentExercises.Add(new StudentExercise
                        {
                            Id= reader.GetInt32(reader.GetOrdinal("SE Id")),
                            studentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            exerciseId = reader.GetInt32(reader.GetOrdinal("Exercise Id")),
                            isComplete = reader.GetBoolean(reader.GetOrdinal("isComplete")),
                            Exercise = new Exercise
                            {
                                id = reader.GetInt32(reader.GetOrdinal("Exercise Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Language = reader.GetString(reader.GetOrdinal("Language"))
                            }

                        });
                    }
                    reader.Close();
                }
            }

            return allStudentExercises;

        }
    }
}
