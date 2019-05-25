using System;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
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

        [Display(Name = "CohortId")]
        public int? CohortId { get; set; }

       
        public Cohort CurrentCohort { get; set; }
    }
}