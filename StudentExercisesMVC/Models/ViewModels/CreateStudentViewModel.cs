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
        public List<SelectListItem> Cohorts { get; set; }
        public Student student { get; set; }

      

        public CreateStudentViewModel()
        {
        

            Cohorts = CohortRepository.GetAllCohorts()
                .Select(cohort => new SelectListItem()
                {
                    Text = cohort.name,
                    Value = cohort.id.ToString()

                })
                .ToList();

            Cohorts.Insert(0, new SelectListItem
            {
                Text = "Choose a cohort",
                Value = "0"
            });

        }

       
    }
}