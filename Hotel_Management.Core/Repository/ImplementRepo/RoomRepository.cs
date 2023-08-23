using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;
using Hotel_Management.Core.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management.Core.Repository.ImplementRepo
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(HotelManagementContext context) : base(context)
        {
        }

        public int CountRoomByStatus(int StatusId)
        {
            return context.Rooms.Count(i => i.StatusId == StatusId);
        }

        public IEnumerable<Room> FindRoomByFloor(int FloorId)
        {
            return context.Rooms.Where(i => i.FloorId == FloorId).ToList();
        }

        public string GetColorByStatus(int StatusId)
        {
            return context.Statuses.Where(i => i.StatusId == StatusId).ToString();
        }

        public IEnumerable<Room> GetCustomerByRoom()
        {
            var rooms = (from room in context.Rooms
                         join booking in context.Bookings on room.RoomId equals booking.RoomId
                         join customer in context.Customers on booking.CustomerId equals customer.CustomerId
                         where room.StatusId == 2 // Thay đổi điều kiện lọc trạng thái phòng tại đây
                         select room).ToList();
            return rooms;
        }

        public Room GetPrice(int RoomId)
        {
            return context.Rooms.Include(item => item.RoomType).FirstOrDefault(i => i.RoomId == RoomId);
        }
    }
}
