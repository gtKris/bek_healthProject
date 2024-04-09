using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Numerics;
using System.Web;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;

namespace bek_healthProject.Models.DAO
{
    //The DoctorDAO class is responsible for interacting with the database to perform CRUD operations on the bek_doctors table.
    public class DoctorDAO
    {
        //Inserts a new doctor record into the database using parameters from the provided DoctorDTO object.
        /*
        This method CreateDoctor inserts a new doctor record into the database using the provided DoctorDTO object.
        It takes a DoctorDTO object as a parameter.
        Inside the try block:
        It opens a connection to the database using SecurityConfig.GetConnection().
        Constructs an SQL query to insert data into the bek_doctors table.
        Creates a MySqlCommand object with the query and sets parameters using cmd.Parameters.AddWithValue().
        Executes the query using cmd.ExecuteNonQuery() to insert the new doctor.
        If successful, it returns a success message.
        If an exception occurs (like a MySqlException), it catches the exception, prints an error message, and re-throws the exception.
         */
        public string CreateDoctor(DoctorDTO doctor)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string insertQuery = "INSERT INTO bek_doctors (name ,lastname, address, phone_number, email, specialty) VALUES (@Name,@pLastName, @pAddress, @pPhoneNumber, @pEmail, @pSpecialty)";
                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Name", doctor.Name);
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
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while creating doctor: " + ex.Message);
                throw;
            }
        }

        //Retrieves a list of all doctors from the database and returns a list of DoctorDTO objects.
        /*
        This method ReadDoctors retrieves a list of all doctors from the database.
        It returns a List<DoctorDTO> containing DoctorDTO objects representing the doctors.
        Inside the try block:
        It opens a connection to the database using SecurityConfig.GetConnection().
        Constructs an SQL query to select all data from the bek_doctors table.
        Creates a MySqlCommand object with the query and executes it using cmd.ExecuteReader().
        It then iterates through the results, creates a DoctorDTO object for each record using MapDoctorFromReader, and adds it to the doctors list.
        If an exception occurs (like a MySqlException), it catches the exception, prints an error message, and returns an empty list.
         */
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
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading doctors: " + ex.Message);
            }
            return doctors;
        }

        //Retrieves a specific doctor from the database based on the provided ID and returns a DoctorDTO object.
        /*
        This method ReadDoctor retrieves a specific doctor from the database based on the provided id.
        It returns a DoctorDTO object representing the doctor.
        Inside the try block:
        It opens a connection to the database using SecurityConfig.GetConnection().
        Constructs an SQL query to select data from the bek_doctors table based on the provided id.
        Creates a MySqlCommand object with the query, sets the id parameter, and executes it using cmd.ExecuteReader().
        It then reads the result, maps the data to the doctor object using MapDoctorFromReader, and returns the doctor.
        If an exception occurs (like a MySqlException), it catches the exception, prints an error message, and returns an empty DoctorDTO object.
         */
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
            catch (MySqlException  ex)
            {
                Console.WriteLine("An error occurred while reading doctor: " + ex.Message);
            }
            return doctor;
        }

        //Updates the details of a doctor in the database based on the provided ID and DoctorDTO object.
        /*
        This method EditDoctor updates the details of a doctor in the database based on the provided id and DoctorDTO object.
        It does not return anything (void).
        Inside the try block:
        It opens a connection to the database using SecurityConfig.GetConnection().
        Constructs an SQL query to update data in the bek_doctors table based on the provided id.
        Creates a MySqlCommand object with the query, sets parameters with the DoctorDTO object, and executes it using cmd.ExecuteNonQuery().
        If an exception occurs (like a MySqlException), it catches the exception, prints an error message, and continues.
         */
        public void EditDoctor(int id, DoctorDTO doctor)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String updateQuery = "UPDATE bek_doctors SET name =@name ,lastname = @pLastName, address = @pAddress, phone_number = @pPhoneNumber, email = @pEmail, specialty = @pSpecialty WHERE id = @pId";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.Parameters.AddWithValue("@name",doctor.Name);
                        cmd.Parameters.AddWithValue("@pLastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("@pAddress", doctor.Address);
                        cmd.Parameters.AddWithValue("@pPhoneNumber", doctor.PhoneNumber);
                        cmd.Parameters.AddWithValue("@pEmail", doctor.Email);
                        cmd.Parameters.AddWithValue("@pSpecialty", doctor.Specialty);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while editing doctor: " + ex.Message);
            }
        }

        //Deletes a doctor record from the database based on the provided ID.
        /*
        This method DeleteDoctor deletes a doctor record from the database based on the provided id.
        It does not return anything (void).
        Inside the try block:
        It opens a connection to the database using SecurityConfig.GetConnection().
        Constructs an SQL query to delete data from the bek_doctors table based on the provided id.
        Creates a MySqlCommand object with the query, sets the id parameter, and executes it using cmd.ExecuteNonQuery().
        If an exception occurs (like a MySqlException), it catches the exception, prints an error message, and continues.
         */
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
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while deleting doctor: " + ex.Message);
            }
        }

        //Helper method to map data from the database reader to a DoctorDTO object.
        /*
        This is a private helper method MapDoctorFromReader used to map data from the database reader to a DoctorDTO object.
        It takes a MySqlDataReader object and a DoctorDTO object as parameters.
        Inside the method:
        It reads values from the reader and assigns them to the corresponding properties of the DoctorDTO object.
        This method helps to avoid duplicating code for mapping data in multiple places.
         */
        private void MapDoctorFromReader(MySqlDataReader reader, DoctorDTO doctor)
        {
            doctor.Id = reader.GetInt32("id");
            doctor.Name = reader.GetString("name");
            doctor.LastName = reader.GetString("lastname");
            doctor.Address = reader.GetString("address");
            doctor.PhoneNumber = reader.GetString("phone_number");
            doctor.Email = reader.GetString("email");
            doctor.Specialty = reader.GetString("specialty");
        }
    }
}