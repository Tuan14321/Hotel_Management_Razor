using Hotel_Management.Core.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace Hotel_Management_Razor.UI.Pages
{
    [Authorize]
    public class CheckOutModel : PageModel
    {
        private IUnitOfWork _unitOfWork;
        public CheckOutModel(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }
        public void OnGet(double totalPrice, int bookingId)
        {
            TotalPrice = totalPrice;
            BookingId = bookingId;
        }

        public double TotalPrice { get; private set; }
        public int BookingId { get; private set; }
        public async Task<IActionResult> OnPostCheckOut(int? BookingId, DateTime checkoutTime, Double TotalPrice, int? RoomId)
        {
            var booking = _unitOfWork.BookingRepository.GetBooking(BookingId.Value);
            booking.CheckOutTime = checkoutTime;
            booking.TotalPrice = TotalPrice;

            var room = _unitOfWork.RoomRepository.Find(RoomId.Value);
            room.StatusId = 3;

            _unitOfWork.BookingRepository.Update(booking);
            _unitOfWork.SaveChanges();

            //await Task.Delay(5 * 60 * 1000); // Đợi 5 phút (5 * 60 * 1000 milliseconds)

            //room.StatusId = 1;
            //_unitOfWork.SaveChanges();
            return RedirectToPage("/Index");
        }
    }
}
