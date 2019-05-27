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
                   Selected = student.Exercises.Find(assignedExercise => assignedExercise.Exercise.id == singleExercise.id) != null

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
