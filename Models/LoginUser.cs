using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class LoginUser
        {
            [Required]
            public string LoginEmail{get; set;}

            [Required(ErrorMessage="Please enter your password")]
            [MinLength(4, ErrorMessage="Password needs to be longer")]
            [DataType(DataType.Password)]
            public string LoginPassword{get; set;}
        }

    }
