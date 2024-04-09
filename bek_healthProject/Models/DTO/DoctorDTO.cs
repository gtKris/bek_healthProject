using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bek_healthProject.Models.DTO
{
    public class DoctorDTO
    {
        // The DoctorDTO class defines the structure of a doctor object used for data transfer.
        //It includes properties for the ID, name, last name, address, phone number, email, and specialty of a doctor.
        //The properties have data annotations for validation, such as required fields, maximum lengths, and regular expressions for valid formats.

        public int Id { get; set; }

        [Required(ErrorMessage = "The name field is required")]
        [MaxLength(50, ErrorMessage = "The name field cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Only letters, accents, and spaces are allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The last name field is required")]
        [MaxLength(50, ErrorMessage = "The last name field cannot exceed 50 characters")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "Only letters, accents, and spaces are allowed")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "The address field is required")]
        [MaxLength(100, ErrorMessage = "The address field cannot exceed 100 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The phone number field is required")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "The phone number must be exactly 8 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "The email field is required")]
        [MaxLength(100, ErrorMessage = "The email field cannot exceed 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The specialty field is required")]
        [MaxLength(100, ErrorMessage = "The specialty field cannot exceed 100 characters")]
        public string Specialty { get; set; }
    }
}