using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management_Razor.UI.Hubs;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Net;


namespace Hotel_Management_Razor.UI.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IUnitOfWork _unitOfWork;
        private readonly IHubContext<SignalRServer> _signalRHub;



        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork _unitOfWork, IHubContext<SignalRServer> signalRHub)
        {
            _logger = logger;
            this._unitOfWork = _unitOfWork;
            _signalRHub = signalRHub;

        }

        public void OnGet()
        {

        }
        public IActionResult OnPostChangeRoom(int? RoomIdPresent, int? RoomIdChange)
        {
            // Gọi hàm ChangeRoom với các tham số
            ChangeRoom(RoomIdPresent, RoomIdChange);

            // Chuyển hướng đến trang Index
            return RedirectToPage("/Index");
        }
        public IActionResult OnPostCheckIn(int? RoomId, string? CustomerName, string? CitizenId, string? PhoneNumber, string? Email, string? Address)
        {
            // Gọi hàm ChangeRoom với các tham số
            CheckIn(RoomId, CustomerName, CitizenId, PhoneNumber, Email, Address);

            // Chuyển hướng đến trang Index
            return RedirectToPage("/Index");
        }
        public async Task<IActionResult> ChangeRoom(int? RoomIdPresent, int? RoomIdChange)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoomIdPresent.HasValue && RoomIdChange.HasValue)
                    {
                        var booking = _unitOfWork.BookingRepository.GetBookingByRoom(RoomIdPresent.Value);
                        booking.RoomId = RoomIdChange;
                        _unitOfWork.BookingRepository.Update(booking);

                        var roomPresent = _unitOfWork.RoomRepository.Find(RoomIdPresent.Value);
                        roomPresent.StatusId = 1;

                        var RoomChange = _unitOfWork.RoomRepository.Find(RoomIdChange.Value);
                        RoomChange.StatusId = 2;

                        _unitOfWork.SaveChanges();
                        await _signalRHub.Clients.All.SendAsync("Load");

                    }

                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> CheckIn(int? RoomId, string? CustomerName, string? CitizenId, string? PhoneNumber, string? Email, string? Address)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoomId.HasValue)
                    {
                        var customer = new Customer();
                        customer.CustomerName = CustomerName;
                        customer.PhoneNumber = PhoneNumber;
                        customer.Email = Email;
                        customer.Address = Address;
                        customer.CitizenId = CitizenId;

                        //_unitOfWork.CustomerRepository.Add(customer);


                        var booking = new Booking();
                        booking.RoomId = RoomId;
                        booking.CheckInTime = DateTime.Now;
                        booking.Customer = customer;

                        _unitOfWork.BookingRepository.Add(booking);



                        var room = _unitOfWork.RoomRepository.Find(RoomId.Value);
                        room.StatusId = 2;

                        _unitOfWork.SaveChanges();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public double TotalPrice { get; set; }
        public int BookingID { get; set; }
        public IActionResult OnGetCheckOut(int? BookingId)
        {
            var booking = _unitOfWork.BookingRepository.GetBooking(BookingId.Value);

            // Tính số giờ chênh lệch giữa Ngày A và Ngày B
            TimeSpan timeDifference = (TimeSpan)(DateTime.Now - booking.CheckInTime);

            // Lấy giá trị tuyệt đối của số giờ chênh lệch
            TimeSpan absoluteDifference = timeDifference.Duration();

            double totalPrice = Math.Ceiling((double)(absoluteDifference.TotalHours * booking.Room.RoomType.Price));

            return RedirectToPage("/CheckOut", new { TotalPrice = totalPrice, BookingId = BookingId.Value });
        }

        public IActionResult OnGetClearRoom(int? RoomId)
        {
            if (ModelState.IsValid)
            {
                if (RoomId.HasValue)
                {
                    var room = _unitOfWork.RoomRepository.Find(RoomId.Value);
                    room.StatusId = 1;
                    _unitOfWork.RoomRepository.Update(room);
                    _unitOfWork.SaveChanges();
                }
            }
            return RedirectToPage("/Index");
        }


    }
}