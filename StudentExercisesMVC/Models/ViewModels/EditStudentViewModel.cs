using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Repositories;
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

        // ---- What does the user need to see? -- //
        // All the info about a student
        // --> Get student info from DB and store it in student property
        // All the exercises
        // --> Get all exercises from DB and store in list of select list items
        // Which exercises are assigned
        // --> Get assigned exercises from db and store them on student property (student.Exercises)
        // All the cohorts
        // --> Get all cohorts from DB and store in list of select list items
        // Which cohort is assigned
        // --> We can do this when we get the student out of the DB


        [Display(Name = "Exercises")]
        public List<SelectListItem> allExercises { get; set; } = new List<SelectListItem>();

        public List<int> SelectedExercises { get; set; }
        public List<SelectListItem> Cohorts { get; set; }

        public Student student { get; set; }

      

        public EditStudentViewModel() { }

        public EditStudentViewModel(int studentId)
        {
            
            student =StudentRepository.GetOneStudent(studentId);

            // Get the exercises that are currently assigned to this student
            // The student model already has a list for this! 
            student.Exercises = ExerciseRepository.GetAssignedExercisesByStudent(studentId);

            // Get a list of all the exercises, and mark the ones that are currently assigned as selected
            allExercises = ExerciseRepository.GetAllExercises()
               .Select(singleExercise => new SelectListItem()
               {
                   Text = singleExercise.Name,
                   Value = singleExercise.id.ToString(),
                   Selected = student.Exercises.Find(assignedExercise => assignedExercise.id == singleExercise.id) != null

               })
               .ToList();

           
            Cohorts = CohortRepository.GetAllCohorts()
               .Select(cohort => new SelectListItem()
               {
                   Text = cohort.name,
                   Value = cohort.id.ToString(),
                   Selected = student.CohortId == cohort.id

               })
               .ToList();

        }  

    }
}
