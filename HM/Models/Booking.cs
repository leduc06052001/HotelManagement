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
    
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            this.Invoices = new HashSet<Invoice>();
        }
    
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public int RoomID { get; set; }
        public int ServiceID { get; set; }
        public Nullable<System.DateTime> BookingDate { get; set; }
        public Nullable<System.DateTime> ChekinDate { get; set; }
        public Nullable<System.DateTime> CheckoutDate { get; set; }
        public Nullable<int> Adult { get; set; }
        public Nullable<int> Child { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Room Room { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual Service Service { get; set; }
    }
}