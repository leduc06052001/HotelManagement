using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "*Bạn chưa nhập email!")]
        [EmailAddress(ErrorMessage = "*Email không hợp lệ")]
        public string Email { get; set; }
    }
}