//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Manager
    {
        public int ManagerID { get; set; }
        [Required(ErrorMessage = "Required information field")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required information field")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required information field")]
        [EmailAddress(ErrorMessage = "Email format is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required information field")]
        [StringLength(50, ErrorMessage = "Name don't exceed 50 character")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Required information field")]
        [StringLength(10, ErrorMessage = "Phone number don't exceed 10 character")]
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
    }
}
