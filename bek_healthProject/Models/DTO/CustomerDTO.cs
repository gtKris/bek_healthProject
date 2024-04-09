﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace bek_healthProject.Models.DTO
{
    //The CustomerDTO class represents the data transfer object for a customer.
    //It includes properties for Id, Name, LastName, Address, PhoneNumber, and Email.
    //Each property has validation attributes for required fields, maximum lengths, and regular expressions for proper formats.
    public class CustomerDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(50, ErrorMessage = "The name field cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Only letters, accents, and spaces are allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The lastname field is required")]
        [MaxLength(50, ErrorMessage = "The last name field cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Only letters, accents, and spaces are allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The address field is required")]
        [MaxLength(100, ErrorMessage = "The address field cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The phone number field is required")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "The phone number must be 8 digits long")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "The email field is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
    }
}