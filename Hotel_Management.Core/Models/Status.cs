using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class Status
    {
        public Status()
        {
            Rooms = new HashSet<Room>();
        }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;
        public string? Color { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
