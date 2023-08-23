using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;
using Hotel_Management.Core.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
namespace Hotel_Management.Core.Repository.ImplementRepo
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(HotelManagementContext context) : base(context)
        {
        }

        public Booking GetBooking(int BookingId)
        {
            return context.Bookings.Where(item => item.CheckOutTime == null).Include(b => b.Customer).Include(c => c.Room).Include(c => c.Room.RoomType).FirstOrDefault(i => i.BookingId == BookingId);
        }

        public Booking GetBookingByRoom(int RoomId)
        {
            return context.Bookings.Where(item => item.CheckOutTime == null).Include(b => b.Customer).FirstOrDefault(i => i.RoomId == RoomId);
        }
    }
}
