namespace Hotel_Management.UI.Areas.Admin.Models
{
    public class RoomTypeViewModel
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public double Price { get; set; }
        public int QuantityPeople { get; set; }
        public int QuantityBed { get; set; }
    }
}
