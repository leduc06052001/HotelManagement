using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "*Bạn chưa nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ, hãy kiểm tra lại")]
        [StringLength(64,ErrorMessage ="*Email không vượt quá 64 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Hãy nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}