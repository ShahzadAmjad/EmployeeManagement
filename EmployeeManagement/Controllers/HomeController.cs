using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //[Route("[controller]")]
    public class HomeController: Controller
    {
        private IEmplyeeRepositry _emplyeeRepositry;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmplyeeRepositry emplyeeRepositry, IHostingEnvironment hostingEnvironment)
        {
            _emplyeeRepositry = emplyeeRepositry;
            this.hostingEnvironment = hostingEnvironment;
        }
        //[Route ("")]
        //[Route("[action]")]
        //[Route("~/")]
        public ViewResult Index() 
        {
            var model= _emplyeeRepositry.GetAllEmployee();
            return View(model);
        }

        //[Route("[action]/{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel() 
            {
                Employee = _emplyeeRepositry.GetEmployee(id??1), 
                PageTitle= "Employee Details" 
            };
           
            //Employee model= _emplyeeRepositry.GetEmployee(1);
            //ViewBag.PageTitle = "Employee Details";
            //ViewBag.Employee = model;
            
            return View(homeDetailsViewModel);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid) {

                string uniqueFileName = null;
                if(model.Photo!=null)
                {
                    string uploadsfolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName= Guid.NewGuid().ToString() + "" + model.Photo.FileName;
                    string filePath= Path.Combine(uploadsfolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }


                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = (Dept)model.Department,
                    PhotoPath = uniqueFileName

                };

                _emplyeeRepositry.Add(newEmployee);
            //return View();
            return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

    }
}
