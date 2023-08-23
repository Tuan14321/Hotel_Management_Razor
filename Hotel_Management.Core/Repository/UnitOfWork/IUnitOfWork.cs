using Hotel_Management.Core.Repository.IRepository;

namespace Hotel_Management.Core.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICompanyRepository CompanyRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IFloorRepository FloorRepository { get; }
        IRoomTypeRepository RoomTypeRepository { get; }
        IBookingRepository BookingRepository { get; }
        IProductRepository ProductRepository { get; }
        IRoomRepository RoomRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IStatusRepository StatusRepository { get; }

        int SaveChanges();
    }
}
