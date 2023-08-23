using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;

namespace Hotel_Management.Core.Repository.IRepository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Booking GetBookingByRoom(int RoomId);
        Booking GetBooking(int BookingId);
    }
}

