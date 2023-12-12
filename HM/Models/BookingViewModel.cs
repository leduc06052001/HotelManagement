using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HM.Models
{
    public class BookingViewModel
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số CCCD")]
        public string IdentifyNo { get; set; }

        [Required(ErrorMessage = "Hãy chọn ngày check in")]
        public Nullable<System.DateTime> CheckinDate { get; set; }

        [Required(ErrorMessage = "Hãy chọn ngày check out")]
        public Nullable<System.DateTime> CheckoutDate { get; set; }

        public Nullable<int> Adult { get; set; }
        public Nullable<int> Child { get; set; }
        public string PromotionCode { get; set; }
        public string Note { get; set; }
    }
}