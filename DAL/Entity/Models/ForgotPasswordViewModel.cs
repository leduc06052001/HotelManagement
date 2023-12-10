using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Entity.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email!")]
        [EmailAddress]
        public string Email { get; set; }
    }
}