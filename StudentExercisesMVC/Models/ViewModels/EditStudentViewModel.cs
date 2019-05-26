using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class EditStudentViewModel : CreateStudentViewModel
    {



        public List<SelectListItem> allExercises {get; set;} = new List<SelectListItem>();

        public List<Exercise> assignedExercises { get; set; } = new List<Exercise>();

        public EditStudentViewModel(int id, string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;

            // Set student property on base class to the current student
            student = GetStudent(id);

            // Get the exercises that are currently assigned to this student


            // Get a list of all the exercises, and mark the ones that are currently assigned as selected
            allExercises = GetAllExercises()
               .Select(e => new SelectListItem()
               {
                   Text = e.Name,
                   Value = e.id.ToString()

               })
               .ToList();

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose a cohort",
                Value = "0"
            });



        }

        private List<Exercise> GetAllExercises()
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


        private Student GetStudent(int id)
        {
            Student studentToEdit = null;

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            Id, firstName, lastName, slackHandle, cohortId
                        FROM Student
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        studentToEdit = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            SlackHandle = reader.GetString(reader.GetOrdinal("slackHandle"))
                        };
                    }
                    reader.Close();

                    
                }
            }
            return studentToEdit;
        }

    }
}
