using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.GenericRepo;
using Hotel_Management.Core.Repository.IRepository;

namespace Hotel_Management.Core.Repository.ImplementRepo
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(HotelManagementContext context) : base(context)
        {
        }

        public Status GetColorByStatus(int StatusId)
        {
            return (Status)context.Statuses.Where(i => i.StatusId == StatusId);
        }
    }
}
