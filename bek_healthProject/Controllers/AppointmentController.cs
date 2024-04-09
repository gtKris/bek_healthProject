using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using bek_healthProject.Models.DAO;
using bek_healthProject.Models.DTO;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using ZstdSharp.Unsafe;
using static System.Data.Entity.Infrastructure.Design.Executor;


namespace bek_healthProject.Controllers
{
    public class AppointmentController : Controller
    {
        //Create an instance of the AppointmentDAO class. This class contains methods to perform operations related to medical appointments on the database.
        //The dao instance will be used in the controller to call methods such as CreateAppointment, ReadAppointments, EditAppointment, and DeleteAppointment, among others.
        private AppointmentDAO dao = new AppointmentDAO();

        //Create an instance of the CustomerDao class. This class contains methods to perform customer-related operations on the database.
        //The customerDao instance will be used in the controller to call methods like ReadCustomers or other methods that handle customer-related operations.
        private readonly CustomerDao customerDao = new CustomerDao();

        //Create an instance of the DoctorDAO class. This class contains methods to perform operations related to doctors on the database.
        //The doctorDao instance will be used in the controller to call methods like ReadDoctors or other methods that handle doctor-related operations.
        private readonly DoctorDAO doctorDao = new DoctorDAO();
        
        // GET: Appointment
        //Retrieves appointments from the DAO.
        //Filters appointments based on provided date ranges.
        //Passes filtered appointments to the view along with date range values.
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
        //Similar to Index, but retrieves canceled appointments.
        public ActionResult CanceledAppointments(DateTime? fromDate, DateTime? toDate)
        {
            List<AppointmentDTO> canceledAppointments = dao.ReadCanceledAppointments();

            // Apply filter if dates are provided
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
        //Reads details of a specific appointment by ID.
        public ActionResult Details(int id)
        {
            return View(dao.ReadAppointment(id));
        }


        // GET: Appointment/Create
        //Prepares data for creating a new appointment.
        //Retrieves customer and doctor IDs for dropdown lists
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
        //Handles form submission for creating a new appointment.
        //Validates input and saves the appointment.
        //Handles specific exceptions and displays corresponding error messages.
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
                // Handle specific exceptions if necessary
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

                // Retrieve data needed for dropdown lists
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

                // Pass data to view
                ViewBag.CustomerIds = customerIds;
                ViewBag.DoctorIds = doctorsIds;

                // Return the view with updated data and error message
                return View(appointmet);
            }
        }


        // GET: Appointment/Edit/5
        //Prepares data for editing an existing appointment.
        //Retrieves customer and doctor IDs for dropdown lists.
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
        //Handles form submission for editing an appointment.
        //Updates the appointment with new data.
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
        //Retrieves appointment details for confirmation before deletion
        public ActionResult Delete(int id)
        {
            AppointmentDTO appointment = dao.ReadAppointment(id);
            return View(appointment);
        }

        // POST: Appointment/Delete/5
        //Handles deletion of an appointment
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

        //Retrieves customer name by ID using AJAX
        public ActionResult GetCustomerNameById(int id)
        {
            var customer = customerDao.ReadCustomer(id);
            return Json(customer.Name, JsonRequestBehavior.AllowGet);
        }

        //Retrieves doctor name by ID using AJAX.
        public ActionResult GetDoctorNameById(int id)
        {
            var doctor = doctorDao.ReadDoctor(id);
            return Json(doctor.Name, JsonRequestBehavior.AllowGet);
        }

    }
}
