using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models.DTO;

namespace bek_healthProject.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
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

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Doctor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
