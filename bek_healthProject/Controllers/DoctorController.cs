using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;
using System.Numerics;

namespace bek_healthProject.Controllers
{
    public class DoctorController : Controller
    {
        private DoctorDAO dao = new DoctorDAO();
        // GET: Doctor
        public ActionResult Index()
        {
            var doctor = dao.ReadDoctors();
            return View(doctor);
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int id)
        {
            return View(dao.ReadDoctor(id));
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        [HttpPost]
        public ActionResult Create(DoctorDTO doctor)
        {
            try
            {

                TempData["SuccessMessage"] = "Doctor created successfully.";
                string result = dao.CreateDoctor(doctor);
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
                    TempData["ErrorMessage"] = "An error occurred while trying to create a Doctor.";
                }

                Console.WriteLine("An error occurred while trying to create a Doctor: " + ex);
                return View();
            }
        }

        // GET: Doctor/Edit/5
        public ActionResult Edit(int id)
        {
            return View(dao.ReadDoctor(id));
        }

        // POST: Doctor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, DoctorDTO doctor)
        {
            try
            {

                TempData["SuccessMessage"] = "Doctor edited successfully.";
                dao.EditDoctor(id, doctor);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying edit the Doctor";
                Console.WriteLine("An error occurred while trying edit the user" + ex);
                return RedirectToAction("Index");

            }
        }

        // GET: Doctor/Delete/5
        public ActionResult Delete(int id)
        {
            DoctorDTO doctor = dao.ReadDoctor(id);
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, DoctorDTO doctor)
        {
            try
            {
                TempData["SuccessMessage"] = "Doctor deleted successfully.";
                dao.DeleteDoctor(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying delete the Doctor";
                Console.WriteLine("An error occurred while trying to delete the user" + ex);
                return View(dao.ReadDoctors());
            }
        }
    }
}
