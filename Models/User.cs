using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage= "Please Enter a FULL Name")]
        [MinLength (2)]
        public string Name { get; set; }

        [Required(ErrorMessage= "Please Enter a Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage="Please Enter a Password")]
        [MinLength (8, ErrorMessage="Password Has To Be Atleast 8 Characters Long")]
        [DataType ("Password")]
        [strongpassword]
        public string Password { get; set; }

        [Required]
        [Compare ("Password")]
        [DataType ("Password")]
        [NotMapped]
        public string Confirm { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Show> MyShows { get; set; }
        public List<rsvp> Attendees { get; set; }
    }
}