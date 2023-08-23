using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;
using Hotel_Management.Core.Repository.IRepository;

namespace Hotel_Management.Core.Repository.ImplementRepo
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(HotelManagementContext context) : base(context)
        {
        }
    }
}
