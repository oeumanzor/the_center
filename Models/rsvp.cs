using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class rsvp
    {
        [Key]
        public int rsvpID {get; set;}
        public int UserID {get; set;}
        public int ShowID {get; set;}
        public User Guest {get; set;}
        public Show Event {get; set;}

    }
}