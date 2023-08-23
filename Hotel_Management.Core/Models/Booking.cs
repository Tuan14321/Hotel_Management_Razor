using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Products = new HashSet<Product>();
        }

        public int BookingId { get; set; }
        public int? CustomerId { get; set; }
        public int? RoomId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public double? TotalPrice { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Room? Room { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
