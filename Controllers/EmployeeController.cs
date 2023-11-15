using Microsoft.AspNetCore.Mvc;
using MVC_Sort_Pagination.Context;
using MVC_Sort_Pagination.Models;

namespace MVC_Sort_Pagination.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DatabaseContext _context;
        public EmployeeController(DatabaseContext context) 
        {
            _context = context;
        }
        public IActionResult AddEmployees()
        {

            var employees = new List<Employee> 
            {
                new Employee { Name = "emp1", Email = "emp1@test"},
                new Employee { Name = "emp2", Email = "emp2@test"},
                new Employee { Name = "emp3", Email = "emp3@test"},
                new Employee { Name = "emp4", Email = "emp4@test"},
                new Employee { Name = "emp5", Email = "emp5@test"},
                new Employee { Name = "emp6", Email = "emp6@test"},
                new Employee { Name = "emp7", Email = "emp7@test"},
                new Employee { Name = "emp8", Email = "emp8@test"},
                new Employee { Name = "emp9", Email = "emp9@test"},
                new Employee { Name = "emp10", Email = "emp10@test"},
            };

            _context.AddRange(employees);
            try 
            {
                _context.SaveChanges();
                return Ok("employees added successfully");
            }
            catch(Exception ex) 
            {
                return Content("Add Failed", ex.Message);
            }
        }

        public IActionResult Employees(string term = "", string orderBy = "", int currentPage = 1) 
        {
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();
            var empData = new EmployeeViewModel();
            
            empData.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            empData.EmailSortOrder = orderBy == "email" ? "email_desc" : "email";

            var employees = (from emp in _context.Employee_Table
                             where term == "" || emp.Name.ToLower().StartsWith(term)
                             select new Employee 
                             {
                                Id = emp.Id,
                                Name = emp.Name,
                                Email = emp.Email
                             });

            switch (orderBy) 
            {
                case "name_desc":
                    employees = employees.OrderByDescending(a => a.Name);
                    break;
                case "email_desc":
                    employees = employees.OrderByDescending(a => a.Email);
                    break;
                case "email":
                    employees = employees.OrderBy(a=>a.Name);
                    break;
                default:
                    employees = employees.OrderBy(a => a.Name);
                    break;            
            }

            var totalRecords = employees.Count();
            int pageSize = 5;
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            employees = employees.Skip((currentPage - 1) * pageSize).Take(pageSize);
            empData.EmployeesData = employees;
            empData.CurrentPage = currentPage;
            empData.TotalPages = totalPages;
            empData.PageSize = pageSize;
            empData.Term = term;
            empData.OrderBy = orderBy;

            return View(empData);
        }
    }
}
