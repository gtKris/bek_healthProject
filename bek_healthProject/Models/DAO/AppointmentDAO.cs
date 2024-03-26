using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;

namespace bek_healthProject.Models.DAO
{
    public class AppointmentDAO
    {
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
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating appointment: " + ex.Message);
            }
        }

        public List<AppointmentDTO> ReadAppointments()
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readAppointmentsQuery = "SELECT * FROM bek_appointments";
                    using (var cmd = new MySqlCommand(readAppointmentsQuery, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AppointmentDTO appointment = new AppointmentDTO();
                                MapAppointmentFromReader(reader, appointment);
                                appointments.Add(appointment);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading appointments: " + ex.Message);
            }
            return appointments;
        }

        public AppointmentDTO ReadAppointment(int id)
        {
            AppointmentDTO appointment = new AppointmentDTO();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readAppointmentQuery = "SELECT * FROM bek_appointments WHERE id = @pId";
                    using (var cmd = new MySqlCommand(readAppointmentQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
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
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading appointment: " + ex.Message);
            }
            return appointment;
        }

        public void EditAppointment(int id, AppointmentDTO appointment)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String updateQuery = "UPDATE bek_appointments SET appointment_date = @pAppointmentDate, appointment_hour = @pAppointmentHour, customer_id = @pCustomerId, doctor_id = @pDoctorId, appointment_description = @pAppointmentDescription, state = @pState , appointment_type = @appointment_type  WHERE id = @pId";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while editing appointment: " + ex.Message);
            }
        }

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
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting appointment: " + ex.Message);
            }
        }

        private void MapAppointmentFromReader(MySqlDataReader reader, AppointmentDTO appointment)
        {
            appointment.Id = reader.GetInt32("id");
            appointment.AppointmentDate = reader.GetDateTime("appointment_date");
            appointment.AppointmentHour = reader.GetTimeSpan("appointment_hour");
            appointment.CustomerId = reader.GetInt32("customer_id");
            appointment.DoctorId = reader.GetInt32("doctor_id");
            appointment.AppointmentDescription = reader.GetString("appointment_description");
            appointment.State = reader.GetString("state");
            appointment.appointment_type=reader.GetString("appointment_type");
        }
    }
}