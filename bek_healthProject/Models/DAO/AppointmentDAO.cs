using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using System.Web.Caching;
using System.Web.Services.Description;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Utilities;

namespace bek_healthProject.Models.DAO
{
    public class AppointmentDAO
    {
        //This method creates a new appointment in the database using a parameterized query to prevent SQL injection.
        //It takes an AppointmentDTO object as a parameter.
        //It opens a connection (MySqlConnection) using a method from SecurityConfig.
        //Inside a using block, it creates a MySqlCommand with the SQL query.
        //It binds parameters with values from the AppointmentDTO.
        //Finally, it executes the query and returns a success message, or catches and logs any MySqlException.
        public string CreateAppointment(AppointmentDTO appointment)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string insertQuery = "INSERT INTO bek_appointments (appointment_date, appointment_hour, customer_id, doctor_id, appointment_description, state , appointment_type) VALUES (@pAppointmentDate, @pAppointmentHour, @pCustomerId, @pDoctorId, @pAppointmentDescription, @pState,@appointment_type)";
                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pAppointmentDate", appointment.AppointmentDate);
                        cmd.Parameters.AddWithValue("@pAppointmentHour", appointment.AppointmentHour);
                        cmd.Parameters.AddWithValue("@pCustomerId", appointment.CustomerId);
                        cmd.Parameters.AddWithValue("@pDoctorId", appointment.DoctorId);
                        cmd.Parameters.AddWithValue("@pAppointmentDescription", appointment.AppointmentDescription);
                        cmd.Parameters.AddWithValue("@pState", appointment.State);
                        cmd.Parameters.AddWithValue("@appointment_type", appointment.appointment_type);
                        cmd.ExecuteNonQuery();
                    }
                }

                return "Create appointment successfully.";
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while creating Appointment: " + ex.Message);
                throw;
            }
        }

        //This method retrieves a list of appointments from the database, along with customer and doctor names.
        //It creates a list of AppointmentDTO objects.
        //Inside a using block, it opens a connection and executes the SQL query.
        //It uses MySqlDataReader to read the results and maps them to AppointmentDTO objects using MapAppointmentFromReader method.
        //The appointments are then added to the list and returned, or any exceptions are caught and logged.
        public List<AppointmentDTO> ReadAppointments()
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readAppointmentsQuery = @"
               SELECT a.id , a.appointment_date, a.appointment_hour, a.customer_id, c.name AS customer_name, 
                 a.doctor_id, d.name AS doctor_name, a.appointment_description, a.state, a.appointment_type
                    FROM bek_appointments a
                    INNER JOIN bek_customers c ON a.customer_id = c.id
                    INNER JOIN bek_doctors d ON a.doctor_id = d.id
                    ";

                    using (var cmd = new MySqlCommand(readAppointmentsQuery, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AppointmentDTO appointment = new AppointmentDTO();
                                appointment.Id = reader.GetInt32("id");
                                appointment.AppointmentDate = reader.GetDateTime("appointment_date");
                                appointment.AppointmentHour = reader.GetTimeSpan("appointment_hour");
                                appointment.CustomerId = reader.GetInt32("customer_id");
                                appointment.CustomerName = reader.GetString("customer_name");
                                appointment.DoctorId = reader.GetInt32("doctor_id");
                                appointment.DoctorName = reader.GetString("doctor_name");
                                appointment.AppointmentDescription = reader.GetString("appointment_description");
                                appointment.State = reader.GetString("state");
                                appointment.appointment_type = reader.GetString("appointment_type");

                                appointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading appointments: " + ex.Message);
            }
            return appointments;
        }


        //This method retrieves a single appointment by its ID from the database.
        //It creates a new AppointmentDTO object.
        //It opens a connection and executes the SQL query to fetch the specific appointment.
        //The appointment data is then mapped to the AppointmentDTO object using MapAppointmentFromReader method.
        //The method returns the appointment or catches and logs any exceptions.
        public AppointmentDTO ReadAppointment(int id)
        {
            AppointmentDTO appointment = new AppointmentDTO();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readAppointmentQuery = @"
                SELECT a.id, a.appointment_date, a.appointment_hour, a.customer_id, c.name AS customer_name, 
                 a.doctor_id, d.name AS doctor_name, a.appointment_description, a.state, a.appointment_type
                FROM bek_appointments a
                INNER JOIN bek_customers c ON a.customer_id = c.id
                INNER JOIN bek_doctors d ON a.doctor_id = d.id
                WHERE a.id = @id
                ";
                    using (var cmd = new MySqlCommand(readAppointmentQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id); // Corregir el nombre del parámetro
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MapAppointmentFromReader(reader, appointment);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading appointment: " + ex.Message);
            }
            return appointment;
        }

        //Updates an existing appointment in the database.
        //This method updates an existing appointment in the database.
        //It takes the appointment ID and an AppointmentDTO object as parameters.
        //It opens a connection and executes an update SQL query with new values from the AppointmentDTO.
        //The method catches and logs any exceptions that occur during the update process.
        public void EditAppointment(int id, AppointmentDTO appointment)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String updateQuery = "UPDATE bek_appointments SET appointment_date = @pAppointmentDate, appointment_hour = @pAppointmentHour,doctor_id = @pDoctorId, appointment_description = @pAppointmentDescription, State = @pState , appointment_type = @appointment_type  WHERE id = @pId";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.Parameters.AddWithValue("@pAppointmentDate", appointment.AppointmentDate);
                        cmd.Parameters.AddWithValue("@pAppointmentHour", appointment.AppointmentHour);
                        cmd.Parameters.AddWithValue("@pDoctorId", appointment.DoctorId);
                        cmd.Parameters.AddWithValue("@pAppointmentDescription", appointment.AppointmentDescription);
                        cmd.Parameters.AddWithValue("@pState", appointment.State);
                        cmd.Parameters.AddWithValue("@appointment_type", appointment.appointment_type);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while editing appointment: " + ex.Message);
            }
        }

        //Deletes an appointment from the database.
        //This method deletes an appointment from the database based on its ID.
        //It opens a connection and executes a delete SQL query.
        //The method catches and logs any exceptions that occur during the deletion process.
        public void DeleteAppointment(int id)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM bek_appointments WHERE id = @pId";
                    using (var cmd = new MySqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while deleting appointment: " + ex.Message);
            }
        }



        //Retrieves the name of a doctor by their ID.
        /*
        This is a private helper method to retrieve a doctor's name by their ID.
        It takes the doctorId as a parameter.
        It opens a connection and executes a SQL query to fetch the doctor's name.
        If the query returns a result, it sets doctorName to that value and returns it.
        The method catches and logs any exceptions.
         */
        private string GetDoctorNameById(int doctorId)
        {
            string doctorName = "";
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string query = "SELECT doctor_name FROM bek_doctors WHERE id = @pDoctorId";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@pDoctorId", doctorId);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            doctorName = result.ToString();
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while getting doctor name: " + ex.Message);
            }
            return doctorName;
        }

        /*
        This method retrieves a list of canceled appointments from the database, along with customer and doctor names.
        It creates a list of AppointmentDTO objects to store the canceled appointments.
        It opens a connection and executes the SQL query to fetch the data.
        The method reads the results using MySqlDataReader and maps them to AppointmentDTO objects.
        The canceled appointments are then added to the list and returned, or any exceptions are caught and logged.
         */
        public List<AppointmentDTO> ReadCanceledAppointments()
        {
            List<AppointmentDTO> canceledAppointments = new List<AppointmentDTO>();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readCanceledAppointmentsQuery = @"
                    SELECT cac.id, cac.appointment_date, cac.appointment_hour, cac.customer_id, c.name AS customer_name, 
                           cac.doctor_id, d.name AS doctor_name, cac.appointment_description, 'Canceled' AS state, cac.appointment_type, cac.canceled_date
                    FROM bek_appointments_canceled cac
                    INNER JOIN bek_customers c ON cac.customer_id = c.id
                    INNER JOIN bek_doctors d ON cac.doctor_id = d.id;
            ";

                    using (var cmd = new MySqlCommand(readCanceledAppointmentsQuery, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AppointmentDTO appointment = new AppointmentDTO();
                                appointment.Id = reader.GetInt32("id");
                                appointment.AppointmentDate = reader.GetDateTime("appointment_date");
                                appointment.AppointmentHour = reader.GetTimeSpan("appointment_hour");
                                appointment.CustomerId = reader.GetInt32("customer_id");
                                appointment.CustomerName = reader.GetString("customer_name");
                                appointment.DoctorId = reader.GetInt32("doctor_id");
                                appointment.DoctorName = reader.GetString("doctor_name");
                                appointment.AppointmentDescription = reader.GetString("appointment_description");
                                appointment.State = reader.GetString("state");
                                appointment.appointment_type = reader.GetString("appointment_type");
                                appointment.CanceledDate = reader.GetDateTime(reader.GetOrdinal("canceled_date"));

                                canceledAppointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading canceled appointments: " + ex.Message);
            }
            return canceledAppointments;
        }


        /*
        This is a private helper method to map appointment data from a MySqlDataReader to an AppointmentDTO object.
        It takes a MySqlDataReader object and an AppointmentDTO object as parameters.
        It reads data from the reader and assigns it to corresponding properties of the AppointmentDTO 
         */
        private void MapAppointmentFromReader(MySqlDataReader reader, AppointmentDTO appointment)
        {
            appointment.Id = reader.GetInt32("id");
            appointment.AppointmentDate = reader.GetDateTime("appointment_date");
            appointment.AppointmentHour = reader.GetTimeSpan("appointment_hour");
            appointment.CustomerId = reader.GetInt32("customer_id");
            appointment.CustomerName = reader.GetString("customer_name");
            appointment.DoctorId = reader.GetInt32("doctor_id");
            appointment.DoctorName = reader.GetString("doctor_name");
            appointment.AppointmentDescription = reader.GetString("appointment_description");
            appointment.State = reader.GetString("state");
            appointment.appointment_type = reader.GetString("appointment_type");
            return;
        }

    }
}