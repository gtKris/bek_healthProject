﻿using System;
using System.ComponentModel.DataAnnotations;


namespace bek_healthProject.Models.DTO
{
    //This model represents the Appointment entity, defining its properties and validation rules.
    public class AppointmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The appointment date field is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "The appointment hour field is required")]
        public TimeSpan AppointmentHour { get; set; }

        [Required(ErrorMessage = "The customer ID field is required")]
        public int CustomerId { get; set; }

        public string CustomerName { get; set; } 

        [Required(ErrorMessage = "The doctor ID field is required")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        [MaxLength(255, ErrorMessage = "The appointment description field cannot exceed 255 characters")]
        [Required(ErrorMessage = "The AppointmentDescription  field is required")]
        public string AppointmentDescription { get; set; }

        [Required(ErrorMessage = "The state field is required")]
        [MaxLength(20, ErrorMessage = "The state field cannot exceed 20 characters")]
        public string State { get; set; }

        [MaxLength(20, ErrorMessage = "The state field cannot exceed 20 characters")]

        [Required(ErrorMessage = "The appointment_type field is required")]
        public string appointment_type { get; set; }
        [Required(ErrorMessage = "The appointment_type field is required")]
        public DateTime? CanceledDate { get; set; }
    }
}