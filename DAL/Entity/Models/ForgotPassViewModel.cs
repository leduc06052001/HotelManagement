using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Models
{
    public class ForgotPassViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập email!")]
        [EmailAddress]
        public string Email {  get; set; }
    }
}