using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Show
    {
        [Key]
        public int ShowID { get; set; }

        [Required(ErrorMessage="Must Have a Title")]
        public string Title {get; set;}

        [Required(ErrorMessage="Date is Required")]
        [DataType(DataType.Date)]
        [FutureDate]
        public DateTime Date {get; set;}
        [DataType(DataType.Time)]
        public DateTime Time {get; set;}

        [Required]
        public int Duration {get; set;}
        [Required]
        public string DurationType {get; set;}

        [Required(ErrorMessage="There Must be a Description")]
        public string Description {get; set;}
        public int UserID {get; set;}
        public User Planner {get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<rsvp> Attendees { get; set; }
    }
}