using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;


namespace bek_healthProject.Controllers
{
    public class AppointmentController : Controller
    {
        private AppointmentDAO dao = new AppointmentDAO();
        private readonly CustomerDao customerDao = new CustomerDao(); 
        private readonly DoctorDAO doctorDao = new DoctorDAO();
        // GET: Appointment
        public ActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            //var appointments = dao.ReadAppointments();
            //return View(appointments);
            List<AppointmentDTO> appointments = dao.ReadAppointments();

            // Aplicar filtro si se proporcionan fechas
            if (fromDate != null && toDate != null)
            {
                appointments = appointments
                    .Where(a => a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate)
                    .ToList(); // Convertir a List<AppointmentDTO>
            }

            // Guardar las fechas en ViewBag para mantener los valores en el formulario después de enviar
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(appointments);
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            return View(dao.ReadAppointment(id));
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {

            var customerIds = customerDao.ReadCustomers().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Id.ToString()
            }).ToList();

            var doctorsIds = doctorDao.ReadDoctors().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Id.ToString()
            }).ToList();

            ViewBag.CustomerIds = customerIds;
            ViewBag.DoctorIds = doctorsIds;


            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(AppointmentDTO appointmet)
        {


            try
            {
                TempData["SuccessMessage"] = "Appointment created successfully.";
                string result = dao.CreateAppointment(appointmet);
                return RedirectToAction("Index");

               
            }
            catch(MySqlException ex)
            {
                Console.WriteLine("An error occurred while trying to create a user: " + ex);
                return View();
            }
        }

        // GET: Appointment/Edit/5
        public ActionResult Edit(int id)
        {
            var appointment = dao.ReadAppointment(id);

            // Obtener la lista de IDs y nombres de clientes
            var customerIds = customerDao.ReadCustomers().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Id.ToString()
            }).ToList();


            var doctorsIds = doctorDao.ReadDoctors().Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Id.ToString()
            }).ToList();

            var selectedDoctor = doctorDao.ReadDoctor(appointment.DoctorId).Name;

            ViewBag.CustomerIds = customerIds;
            ViewBag.DoctorIds = doctorsIds;
            ViewBag.SelectedDoctor = selectedDoctor;

            return View(appointment);
        }

        // POST: Appointment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, AppointmentDTO appointment)
        {
            try
            {

                TempData["SuccessMessage"] = "User edited successfully.";
                dao.EditAppointment(id, appointment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying edit the user";
                Console.WriteLine("An error occurred while trying edit the user" + ex);
                return RedirectToAction("Index");

            }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Appointment/Delete/5
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

        public ActionResult GetCustomerNameById(int id)
        {
            var customer = customerDao.ReadCustomer(id);
            return Json(customer.Name, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDoctorNameById(int id)
        {
            var doctor = doctorDao.ReadDoctor(id);
            return Json(doctor.Name, JsonRequestBehavior.AllowGet);
        }

    }
}
