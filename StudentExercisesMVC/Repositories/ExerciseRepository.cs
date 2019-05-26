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

        public static List<Exercise> GetAssignedExercisesByStudent(int studentId)
        {
            List<Exercise> assignedExercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT e.Id, e.Name, e.Language FROM Exercise e JOIN StudentExercise se ON e.Id=se.ExerciseId WHERE se.StudentId=@id";

                    cmd.Parameters.Add(new SqlParameter("@id", studentId));
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        assignedExercises.Add(new Exercise
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Language = reader.GetString(reader.GetOrdinal("Language")),
                        });
                    }
                    reader.Close();
                }
            }
            return assignedExercises;

        }
    }
}
