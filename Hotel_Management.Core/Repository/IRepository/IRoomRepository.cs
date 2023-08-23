using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;

namespace Hotel_Management.Core.Repository.IRepository
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        IEnumerable<Room> FindRoomByFloor(int FloorId);
        String GetColorByStatus(int StatusId);
        IEnumerable<Room> GetCustomerByRoom();
        Room GetPrice(int RoomId);
        int CountRoomByStatus(int StatusId);
    }
}
