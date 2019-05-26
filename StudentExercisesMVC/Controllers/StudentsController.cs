using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC.Models;
using StudentExercisesMVC.Models.ViewModels;
using StudentExercisesMVC.Repositories;




namespace StudentExercisesMVC.Controllers
{
    public class StudentsController : Controller
    {


        public StudentsController(IConfiguration config)
        {
            
            StudentRepository.SetConfig(config);
            ExerciseRepository.SetConfig(config);
            CohortRepository.SetConfig(config);
        }

        
        // GET: Students
        public ActionResult Index()
        {
            List<Student> students = StudentRepository.GetStudents();
            return View(students);

        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            Student student = StudentRepository.GetOneStudentWithExercises(id);
            return View(student);

        }

        // GET: Students/Create
        public ActionResult Create()
        {
            CreateStudentViewModel studentViewModel = new CreateStudentViewModel();
            return View(studentViewModel);
        }

        // POST: Students/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateStudentViewModel model)
        {
            try
            {
                StudentRepository.CreateStudent(model);
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View();
            }


        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            EditStudentViewModel viewModel = new EditStudentViewModel(id);
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditStudentViewModel viewModel)
        {
            try
            {
                StudentRepository.UpdateStudent(id, viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return View(viewModel);
            }
        }

        // GET: Students/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Student studentToDelete = StudentRepository.GetOneStudent(id);
            return View(studentToDelete);
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection formCollection)
        {
            try
            {

                StudentRepository.DeleteStudent(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}