using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace StudentExercisesMVC.Models.ViewModels
{
    public class CreateStudentViewModel
    {

        // This is where our dropdown options will go! SelectListItem is a built in type for dropdown lists
        public List<SelectListItem> Cohorts { get; set; }

        // An individual student. When we render the form (i.e. make a GET request to Students/Create) this will be null. When we submit the form (i.e. make a POST request to Students/Create), this will hold the data from the form.
        public Student student { get; set; }

        // Connection to the database
        protected string _connectionString;

        public CreateStudentViewModel()
        {

            Cohorts = CohortRepository.GetAllCohorts()
                .Select(cohort => new SelectListItem()
                {
                    Text = cohort.name,
                    Value = cohort.id.ToString()

                })
                .ToList();

            // Add an option with instructiosn for how to use the dropdown
            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose a cohort",
                Value = "0"
            });

        }

    }
}