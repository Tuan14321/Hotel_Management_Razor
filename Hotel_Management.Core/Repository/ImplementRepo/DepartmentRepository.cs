using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;
using Hotel_Management.Core.Repository.IRepository;

namespace Hotel_Management.Core.Repository.ImplementRepo
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(HotelManagementContext context) : base(context)
        {
        }
    }
}
