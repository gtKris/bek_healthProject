using System;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;


namespace bek_healthProject.Controllers
{
    public class DoctorController : Controller
    {
        private DoctorDAO dao = new DoctorDAO();
        // GET: Doctor
        //Returns a view with a list of all doctors retrieved from the DAO.
        public ActionResult Index()
        {
            var doctor = dao.ReadDoctors();
            return View(doctor);
        }

        // GET: Doctor/Details/5
        //Returns a view displaying details of a specific doctor with the given ID.
        public ActionResult Details(int id)
        {
            return View(dao.ReadDoctor(id));
        }

        // GET: Doctor/Create
        //Returns a view to create a new doctor.
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctor/Create
        //Handles the POST request to create a new doctor. It calls the CreateDoctor method in the DAO.
        //If an error occurs, it catches MySQL exceptions for duplicate entries (like email or phone number) and sets appropriate error messages.
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
        //Returns a view to edit details of the doctor with the given ID.
        public ActionResult Edit(int id)
        {
            return View(dao.ReadDoctor(id));
        }

        // POST: Doctor/Edit/5
        //Handles the POST request to update details of a doctor with the given ID. It calls the EditDoctor method in the DAO.
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
        //Returns a view to confirm the deletion of the doctor with the given ID.
        public ActionResult Delete(int id)
        {
            DoctorDTO doctor = dao.ReadDoctor(id);
            return View(doctor);
        }

        // POST: Doctor/Delete/5
        //Handles the POST request to delete the doctor with the given ID. It calls the DeleteDoctor method in the DAO.
        //If an error occurs, it catches exceptions and redirects to the index view.
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
