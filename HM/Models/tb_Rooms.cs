//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_Rooms
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_Rooms()
        {
            this.tb_Bookings = new HashSet<tb_Bookings>();
        }
    
        public int RoomID { get; set; }
        public Nullable<int> RoomNumber { get; set; }
        public string RoomType { get; set; }
        public Nullable<int> Bed { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_Bookings> tb_Bookings { get; set; }
    }
}