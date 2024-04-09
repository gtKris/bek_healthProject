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
        //The CreateCustomer method creates a new customer in the database using the provided CustomerDTO object.
        /*
        This method creates a new customer in the database using the provided CustomerDTO object.
        It constructs and executes an SQL INSERT query with parameters from the customer object.
        Returns a success message if the operation is successful, otherwise, it catches and handles any MySqlException that occurs.
         */
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

        //The ReadCustomers method retrieves all customers from the database and returns a list of CustomerDTO objects.
        /*
        Retrieves all customers from the database and returns a list of CustomerDTO objects.
        Constructs and executes an SQL SELECT query to fetch all customers.
        Each retrieved row is used to create a CustomerDTO object and add it to the customers list.
         */
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

        //The ReadCustomer method retrieves a specific customer by id from the database and returns a CustomerDTO object.
        /*
        Retrieves a specific customer by ID from the database and returns a CustomerDTO object.
        Constructs and executes an SQL SELECT query with a parameterized id.
        Uses the MapCustomerFromReader method to map the data from the database reader to a CustomerDTO object.
         */
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

        //The EditCustomer method updates an existing customer in the database with the provided CustomerDTO object.
        /*
        Updates an existing customer in the database with the provided CustomerDTO object.
        Constructs and executes an SQL UPDATE query with parameters from the customer object and id.
        Modifies the customer record in the database based on the provided id.
         */
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

        //The DeleteCustomer method deletes an existing customer from the database based on the provided id.
        /*
        Deletes an existing customer from the database based on the provided id.
        Constructs and executes an SQL DELETE query with a parameterized id.
         */
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

        //The MapCustomerFromReader method maps data from a MySqlDataReader object to a CustomerDTO object.
        /*
        A private helper method to map data from a MySqlDataReader object to a CustomerDTO object.
        Reads the values from the database reader and assigns them to the corresponding properties of the CustomerDTO.
         */
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