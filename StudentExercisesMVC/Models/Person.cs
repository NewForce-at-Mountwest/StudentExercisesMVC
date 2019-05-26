using System;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }

        [Required]
        [Display(Name = "Cohort Id")]
        public int CohortId { get; set; }

       
        public Cohort CurrentCohort { get; set; }
    }
}