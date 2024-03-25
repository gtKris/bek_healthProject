using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;

namespace bek_healthProject.Models.DAO
{
    public class DoctorDAO
    {
        public string CreateDoctor(DoctorDTO doctor)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string insertQuery = "INSERT INTO bek_doctors (lastname, address, phone_number, email, specialty) VALUES (@pLastName, @pAddress, @pPhoneNumber, @pEmail, @pSpecialty)";
                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pLastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("@pAddress", doctor.Address);
                        cmd.Parameters.AddWithValue("@pPhoneNumber", doctor.PhoneNumber);
                        cmd.Parameters.AddWithValue("@pEmail", doctor.Email);
                        cmd.Parameters.AddWithValue("@pSpecialty", doctor.Specialty);
                        cmd.ExecuteNonQuery();
                    }
                }

                return "Create doctor successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating doctor: " + ex.Message);
            }
        }

        public List<DoctorDTO> ReadDoctors()
        {
            List<DoctorDTO> doctors = new List<DoctorDTO>();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readDoctorsQuery = "SELECT * FROM bek_doctors";
                    using (var cmd = new MySqlCommand(readDoctorsQuery, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DoctorDTO doctor = new DoctorDTO();
                                MapDoctorFromReader(reader, doctor);
                                doctors.Add(doctor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading doctors: " + ex.Message);
            }
            return doctors;
        }

        public DoctorDTO ReadDoctor(int id)
        {
            DoctorDTO doctor = new DoctorDTO();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readDoctorQuery = "SELECT * FROM bek_doctors WHERE id = @pId";
                    using (var cmd = new MySqlCommand(readDoctorQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MapDoctorFromReader(reader, doctor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading doctor: " + ex.Message);
            }
            return doctor;
        }

        public void EditDoctor(int id, DoctorDTO doctor)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String updateQuery = "UPDATE bek_doctors SET lastname = @pLastName, address = @pAddress, phone_number = @pPhoneNumber, email = @pEmail, specialty = @pSpecialty WHERE id = @pId";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.Parameters.AddWithValue("@pLastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("@pAddress", doctor.Address);
                        cmd.Parameters.AddWithValue("@pPhoneNumber", doctor.PhoneNumber);
                        cmd.Parameters.AddWithValue("@pEmail", doctor.Email);
                        cmd.Parameters.AddWithValue("@pSpecialty", doctor.Specialty);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while editing doctor: " + ex.Message);
            }
        }

        public void DeleteDoctor(int id)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM bek_doctors WHERE id = @pId";
                    using (var cmd = new MySqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting doctor: " + ex.Message);
            }
        }

        private void MapDoctorFromReader(MySqlDataReader reader, DoctorDTO doctor)
        {
            doctor.Id = reader.GetInt32("id");
            doctor.LastName = reader.GetString("lastname");
            doctor.Address = reader.GetString("address");
            doctor.PhoneNumber = reader.GetString("phone_number");
            doctor.Email = reader.GetString("email");
            doctor.Specialty = reader.GetString("specialty");
        }
    }
}