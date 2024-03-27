using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Common
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(6, ErrorMessage = "*Mật khẩu tối thiểu 6 ký tự")]
        public string OldPassword {  get; set; }

        [Required]
        [StringLength(6, ErrorMessage = "*Mật khẩu tối thiểu 6 ký tự")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "*Bạn chưa xác nhận mật khẩu")]
        [StringLength(6, ErrorMessage = "*Mật khẩu tối thiểu 6 ký tự")]
        [Compare("NewPassword",ErrorMessage = "*Mật khẩu không khớp")]
        public string ConfirmPassword { get; set;}
    }
}