using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;

namespace bek_healthProject.Controllers
{
  

    public class CustomerController : Controller
    {

        private CustomerDao dao = new CustomerDao();

        // GET: Customer
        public ActionResult Index()
        {
            var customers = dao.ReadCustomers();
            return View(customers);
        }


        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View(dao.ReadCustomer(id));
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(CustomerDTO customer)
        {
            try
            {
      
                TempData["SuccessMessage"] = "User created successfully.";
                string result = dao.CreateCustomer(customer);
                return RedirectToAction("Index");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) 
                {
                    if (ex.Message.Contains("email")) 
                    {
                        TempData["ErrorMessage"] = "The email is already is not available"; 
                    }
                    else if (ex.Message.Contains("phone_number"))
                    {
                        TempData["ErrorMessage"] = "The phone number is not available"; 
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while trying to create a user."; 
                }

                Console.WriteLine("An error occurred while trying to create a user: " + ex); 
                return View();
            }
        }


        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View(dao.ReadCustomer(id));
        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id , CustomerDTO customer)
        {
            try
            {

                TempData["SuccessMessage"] = "User edited successfully.";
                dao.EditCustomer(id, customer);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying edit the user";
                Console.WriteLine("An error occurred while trying edit the user" + ex);
                return RedirectToAction("Index");
            
            }
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int id)
        {
            CustomerDTO customer = dao.ReadCustomer(id);
            return View(customer);
           
        }

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
                dao.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying delete the user";
                Console.WriteLine("An error occurred while trying to delete the user" + ex);
                return View(dao.ReadCustomers());
            }
        }
    }
}
