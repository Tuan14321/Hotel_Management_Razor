using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
        public double? Price { get; set; }
        public int? QuantityBed { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
