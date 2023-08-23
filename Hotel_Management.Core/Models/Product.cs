using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public double Price { get; set; }
        public int? BookingId { get; set; }
        public string? Image { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
