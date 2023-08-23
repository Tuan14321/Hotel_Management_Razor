using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class Room
    {
        public Room()
        {
            Bookings = new HashSet<Booking>();
        }

        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public int? StatusId { get; set; }
        public int FloorId { get; set; }
        public int RoomTypeId { get; set; }

        public virtual Floor Floor { get; set; } = null!;
        public virtual RoomType RoomType { get; set; } = null!;
        public virtual Status? Status { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
