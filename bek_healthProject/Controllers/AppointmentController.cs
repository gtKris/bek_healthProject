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

        // GET: Appointment/Canceled
        public ActionResult CanceledAppointments(DateTime? fromDate, DateTime? toDate)
        {
            List<AppointmentDTO> canceledAppointments = dao.ReadCanceledAppointments();

            // Aplicar filtro si se proporcionan fechas
            if (fromDate != null && toDate != null)
            {
                canceledAppointments = canceledAppointments
                    .Where(a => a.AppointmentDate >= fromDate && a.AppointmentDate <= toDate)
                    .ToList(); // Convertir a List<AppointmentDTO>
            }

            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(canceledAppointments);
        }


        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            return View(dao.ReadAppointment(id));
        }


        // GET: Appointment/Create
        public ActionResult Create()
        {
            try
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving data.";
                Console.WriteLine("An error occurred while trying to retrieve data: " + ex);
                return RedirectToAction("Error");
            }
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
            catch (MySqlException ex)
            {
                // Manejar excepciones específicas si es necesario
                if (ex.Message.Contains("Sundays"))
                {
                    TempData["ErrorMessage"] = "Appointments cannot be scheduled on Sundays";
                }

                if (ex.Message.Contains("Appointments cannot be scheduled for the current day after 16:30 p.m"))
                {
                    TempData["ErrorMessage"] = "Appointments cannot be scheduled for the current day after 16:30 p.m";
                }

                if (ex.Message.Contains("Appointments cannot be scheduled for days prior to the current one"))
                {
                    TempData["ErrorMessage"] = "Appointments cannot be scheduled for days prior to the current one";
                }

                if (ex.Message.Contains("The appointment must be scheduled in half-hour intervals"))
                {
                    TempData["ErrorMessage"] = "The appointment must be scheduled in half-hour intervals";
                }

                if (ex.Message.Contains("The doctor does not have the required specialty for this appointment"))
                {
                    TempData["ErrorMessage"] = "The doctor does not have the required specialty for this appointment";
                }

                if (ex.Message.Contains("The doctor and the client already have an appointment at that time"))
                {
                    TempData["ErrorMessage"] = "The doctor and the client already have an appointment at that time";
                }

                if (ex.Message.Contains("The doctor already has an appointment at that time"))
                {
                    TempData["ErrorMessage"] = "The doctor already has an appointment at that time";
                }

                if (ex.Message.Contains("The client already has an appointment at that time"))
                {
                    TempData["ErrorMessage"] = "The client already has an appointment at that time";
                }

                // Recuperar datos necesarios para las listas desplegables
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

                // Pasar los datos a la vista
                ViewBag.CustomerIds = customerIds;
                ViewBag.DoctorIds = doctorsIds;

                // Devolver la vista con los datos actualizados y el mensaje de error
                return View(appointmet);
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

                TempData["SuccessMessage"] = "Appointment edited successfully.";
                dao.EditAppointment(id, appointment);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying edit the Appointment";
                Console.WriteLine("An error occurred while trying edit the Appointment" + ex);
                return RedirectToAction("Index");

            }
        }

        // GET: Appointment/Delete/5
        public ActionResult Delete(int id)
        {
            AppointmentDTO appointment = dao.ReadAppointment(id);
            return View(appointment);
        }

        // POST: Appointment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TempData["SuccessMessage"] = "Appointment deleted successfully.";
                dao.DeleteAppointment(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while trying delete the Appointment";
                Console.WriteLine("An error occurred while trying to delete the Appointment" + ex);
                return View(dao.ReadAppointments());
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
