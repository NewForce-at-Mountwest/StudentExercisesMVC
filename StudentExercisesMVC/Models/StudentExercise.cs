using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercisesMVC.Models
{
    public class StudentExercise
    {
        public int Id { get; set; }

        public int studentId { get; set; }

        public int exerciseId { get; set; }

        [Display(Name ="Completed?")]
        public bool isComplete { get; set; }

        public Exercise Exercise { get; set; }
    }
}
