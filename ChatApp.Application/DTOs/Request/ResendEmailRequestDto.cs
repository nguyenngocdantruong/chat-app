using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs.Request
{
    public class ResendEmailRequestDto
    {
        [Required(ErrorMessage = "Transaction ID is required")]
        public string? TransactionId { get; set; }
        [Required(ErrorMessage = "Email is required")] 
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        public ActionType ActionType { get; set; }
    }
}
