using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "*Vui lòng nhập tên")]
        [StringLength(50, ErrorMessage = "*Tên không vượt quá 50 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "*Email không được để trống")]
        [EmailAddress(ErrorMessage = "*Email không hợp lệ")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "*Nhập mật khẩu của bạn")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "*Mật khẩu tối thiểu 6 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Hãy xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "*Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }

    }
}