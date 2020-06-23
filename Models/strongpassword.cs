using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Exam.Models
{
    public class strongpasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("Password is Required");
            }
            var reg = new Regex(@"^(?=.*?\d)(?=.*?[A-Z])(?=.*?[a-z])[A-Za-z\d,!@#$%^&*+=]{8,}$");
            if (!reg.IsMatch((string)value))
                return new ValidationResult("Password must have at least one special and regular character.");
            return ValidationResult.Success;
        }
    }
}