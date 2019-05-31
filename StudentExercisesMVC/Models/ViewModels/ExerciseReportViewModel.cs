using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models.ViewModels
{
    public class ExerciseReportViewModel
    {


        [Display(Name = "Number of students completed:")]
        public int numberOfStudentsCompleted { get; set; }



        [Display(Name = "Percentage of students completed:")]
        public double? percentageOfStudentsCompleted { get; set; }


        public Exercise exercise { get; set; }

        public ExerciseReportViewModel(int id)
        {
            exercise = ExerciseRepository.GetSingleExercise(id);
            List<StudentExercise> assignedStudents = ExerciseRepository.GetAssignedStudentsByExercise(id);

            numberOfStudentsCompleted = assignedStudents.Where(s => s.isComplete == true).Count();

            try
            {
                percentageOfStudentsCompleted = ((double)numberOfStudentsCompleted / (double)assignedStudents.Count()) * 100;
            }
            catch (Exception)
            {
                percentageOfStudentsCompleted = null;
            }

          
           
        }
        

    }


}
