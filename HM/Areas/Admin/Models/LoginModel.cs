using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nhập tên đăng nhập")]
        public string userName {  get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string password { get; set; }
    }
}