using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bek_healthProject.Models.DTO
{
    public class AppointmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The appointment date field is required")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "The appointment hour field is required")]
        public TimeSpan AppointmentHour { get; set; }

        [Required(ErrorMessage = "The customer ID field is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "The doctor ID field is required")]
        public int DoctorId { get; set; }

        [MaxLength(255, ErrorMessage = "The appointment description field cannot exceed 255 characters")]
        public string AppointmentDescription { get; set; }

        [MaxLength(20, ErrorMessage = "The state field cannot exceed 20 characters")]
        public string State { get; set; }
    }
}