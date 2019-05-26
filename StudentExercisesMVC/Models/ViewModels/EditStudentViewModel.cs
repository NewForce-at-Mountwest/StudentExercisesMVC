using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class EditStudentViewModel
    {


        [Display(Name = "Exercises")]
        public List<SelectListItem> allExercises { get; set; } = new List<SelectListItem>();

        public List<int> SelectedExercises { get; set; }
        public List<SelectListItem> Cohorts { get; set; }

        public Student student { get; set; }

        private string _connectionString;


        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public EditStudentViewModel() { }

        public EditStudentViewModel(int studentId, string connectionString)
        {
            _connectionString = connectionString;

            // Set student property on base class to the current student
            student = GetStudent(studentId);

            // Get the exercises that are currently assigned to this student
            // The student model already has a list for this! 
            student.Exercises = GetAssignedExercises(studentId);

            // Get a list of all the exercises, and mark the ones that are currently assigned as selected
            allExercises = GetAllExercises()
               .Select(singleExercise => new SelectListItem()
               {
                   Text = singleExercise.Name,
                   Value = singleExercise.id.ToString(),
                   Selected = student.Exercises.Find(assignedExercise => assignedExercise.id == singleExercise.id) != null

               })
               .ToList();

            Cohorts = GetAllCohorts()
               .Select(cohort => new SelectListItem()
               {
                   Text = cohort.name,
                   Value = cohort.id.ToString(),
                   Selected = student.CohortId == cohort.id

               })
               .ToList();

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
                            SlackHandle = reader.GetString(reader.GetOrdinal("slackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("cohortId"))
                        };
                    }
                    reader.Close();


                }
            }
            return studentToEdit;
        }

        private List<Exercise> GetAssignedExercises(int StudentId)
        {
            List<Exercise> assignedExercises = new List<Exercise>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT e.Id, e.Name, e.Language FROM Exercise e JOIN StudentExercise se ON e.Id=se.ExerciseId WHERE se.StudentId=@id";

                    cmd.Parameters.Add(new SqlParameter("@id", StudentId));
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

        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }

    }
}
