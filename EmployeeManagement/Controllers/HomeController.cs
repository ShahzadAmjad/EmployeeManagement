using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //[Route("[controller]")]
    public class HomeController: Controller
    {
        private IEmplyeeRepositry _emplyeeRepositry;
        public HomeController(IEmplyeeRepositry emplyeeRepositry)
        {
            _emplyeeRepositry = emplyeeRepositry;
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
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid) { 
            Employee newEmployee= _emplyeeRepositry.Add(employee);
            //return View();
            return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

    }
}
