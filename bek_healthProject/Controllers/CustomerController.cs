using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;
using Antlr.Runtime.Misc;
using Antlr.Runtime;
using System.Web.UI.WebControls;

namespace bek_healthProject.Controllers
{
  

    public class CustomerController : Controller
    {

        private CustomerDao dao = new CustomerDao();

        // GET: Customer
        // The Index action retrieves all customers using the CustomerDao class and then passes them to the view.
        public ActionResult Index()
        {
            var customers = dao.ReadCustomers();
            return View(customers);
        }


        // GET: Customer/Details/5
        //The Details action takes an id parameter, retrieves the customer with that id using CustomerDao, and then passes the customer to the view.
        public ActionResult Details(int id)
    {
        return View(dao.ReadCustomer(id));
    }

        // GET: Customer/Create
        //The GET action simply returns the Create view.
        public ActionResult Create()
    {
            return View();
        }

        // POST: Customer/Create
        //The POST action takes a CustomerDTO object, tries to create the customer using CustomerDao,
        //and redirects to the Index action upon success. It also handles MySQL exceptions for duplicate email or phone number.
        [HttpPost]
        public ActionResult Create(CustomerDTO customer)
        {
            try
            {
      
                TempData["SuccessMessage"] = "Customer created successfully.";
                string result = dao.CreateCustomer(customer);
                return RedirectToAction("Index");
            }
            catch (MySqlException ex)
            {

                if (ex.Message.Contains("email"))
                {
                    TempData["ErrorMessage"] = "The email is already is not available";
                }
                else if (ex.Message.Contains("phone_number"))
                {
                    TempData["ErrorMessage"] = "The phone number is not available";
                }


                else
                {
                    TempData["ErrorMessage"] = "An error occurred while trying to create a Customer."; 
                }

                Console.WriteLine("An error occurred while trying to create a user: " + ex); 
                return View();
            }
        }


        // GET: Customer/Edit/5
        //The GET action returns the Edit view with the customer data.
        public ActionResult Edit(int id)
        {
            return View(dao.ReadCustomer(id));
        }

        // POST: Customer/Edit/5
        //The POST action takes the id of the customer to edit and a CustomerDTO object,
        //updates the customer using CustomerDao, and redirects to the Index action upon success.
        [HttpPost]
        public ActionResult Edit(int id , CustomerDTO customer)
        {
            try
            {

                TempData["SuccessMessage"] = "Customer edited successfully.";
                dao.EditCustomer(id, customer);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying edit the Customer";
                Console.WriteLine("An error occurred while trying edit the user" + ex);
                return RedirectToAction("Index");
            
            }
        }

        // GET: Customer/Delete/5
        //The GET action retrieves the customer with the given id using CustomerDao and returns the Delete view.
        public ActionResult Delete(int id)
        {
            CustomerDTO customer = dao.ReadCustomer(id);
            return View(customer);
           
        }

        // POST: Customer/Delete/5
        //The POST action takes the id of the customer to delete, deletes the customer using CustomerDao, and redirects to the Index action upon success.
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TempData["SuccessMessage"] = "Customer deleted successfully.";
                dao.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying delete the Customer";
                Console.WriteLine("An error occurred while trying to delete the Customer" + ex);
                return View(dao.ReadCustomers());
            }
        }
    }
}
