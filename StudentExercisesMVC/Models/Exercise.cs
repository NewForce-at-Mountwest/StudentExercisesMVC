/*
Exercise
You must define a type for representing an exercise in code. An exercise can be assigned to many students.

Name of exercise
Language of exercise (JavaScript, Python, CSharp, etc.) */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models{
    public class Exercise{

        [Required]
        public int id {get; set;}

        [Required]
        [Display(Name="Exercise Name")]
        public string Name {get; set;}

        [Required]
        [Display(Name="Exercise Language")]
        public string Language {get; set;}

        public List<Student> assignedStudents { get; set; }  = new List<Student>();
    }
}