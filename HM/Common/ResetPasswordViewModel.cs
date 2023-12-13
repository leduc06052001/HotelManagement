using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "*Bạn chưa nhập mật khẩu")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "*Mật khẩu phải lớn hơn 6 ký tự")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*Bạn chưa nhập mật khẩu xác thực")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage ="*Mật khẩu không khớp, hãy thử lại")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string ResetPasswordCode { set; get; }
    }
}