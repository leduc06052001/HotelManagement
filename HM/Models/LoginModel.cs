﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nhập tên email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu")]
        public string password { get; set; }
        public string rememberMe { get; set; }
    }
}