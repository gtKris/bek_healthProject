using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using bek_healthProject.Models.DTO;
using MySql.Data.MySqlClient;


namespace bek_healthProject.Models.DAO
{
    public class CustomerDao
    {
        public string CreateCustomer(CustomerDTO customer)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string insertQuery = "INSERT INTO bek_customers (name, lastname, address, phone_number, email) VALUES (@pName, @pLastName, @pAddress, @pPhoneNumber, @pEmail)";
                    using (var cmd = new MySqlCommand(insertQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pName", customer.Name);
                        cmd.Parameters.AddWithValue("@pLastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@pAddress", customer.Address);
                        cmd.Parameters.AddWithValue("@pPhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@pEmail", customer.Email);
                        cmd.ExecuteNonQuery();
                    }
                }

                return "Create customer successfully.";
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while creating customer: " + ex.Message);
                throw; 
            }
        }


        public List<CustomerDTO> ReadCustomers()
        {
            List<CustomerDTO> customers = new List<CustomerDTO>();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readCustomersQuery = "SELECT * FROM bek_customers";
                    using (var cmd = new MySqlCommand(readCustomersQuery, con))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerDTO customer = new CustomerDTO();
                                MapCustomerFromReader(reader, customer);
                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading customers: " + ex.Message);
            }
            return customers;
        }

        public CustomerDTO ReadCustomer(int id)
        {
            CustomerDTO customer = new CustomerDTO();
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String readCustomerQuery = "SELECT * FROM bek_customers WHERE id = @pId";
                    using (var cmd = new MySqlCommand(readCustomerQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MapCustomerFromReader(reader, customer);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while reading customer: " + ex.Message);
            }
            return customer;
        }

        public void EditCustomer(int id, CustomerDTO customer)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    String updateQuery = "UPDATE bek_customers SET name = @pName, lastname = @pLastName, address = @pAddress, phone_number = @pPhoneNumber, email = @pEmail WHERE id = @pId";
                    using (var cmd = new MySqlCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.Parameters.AddWithValue("@pName", customer.Name);
                        cmd.Parameters.AddWithValue("@pLastName", customer.LastName);
                        cmd.Parameters.AddWithValue("@pAddress", customer.Address);
                        cmd.Parameters.AddWithValue("@pPhoneNumber", customer.PhoneNumber);
                        cmd.Parameters.AddWithValue("@pEmail", customer.Email);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while editing customer: " + ex.Message);
            }
        }

        public void DeleteCustomer(int id)
        {
            try
            {
                using (MySqlConnection con = SecurityConfig.GetConnection())
                {
                    con.Open();
                    string deleteQuery = "DELETE FROM bek_customers WHERE id = @pId";
                    using (var cmd = new MySqlCommand(deleteQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@pId", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("An error occurred while deleting customer: " + ex.Message);
            }
        }

        private void MapCustomerFromReader(MySqlDataReader reader, CustomerDTO customer)
        {
            customer.Id = reader.GetInt32("id");
            customer.Name = reader.GetString("name");
            customer.LastName = reader.GetString("lastname");
            customer.Address = reader.GetString("address");
            customer.PhoneNumber = reader.GetString("phone_number");
            customer.Email = reader.GetString("email");
        }
    }
}