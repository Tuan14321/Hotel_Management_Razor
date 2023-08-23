using Hotel_Management_Razor.UI.Models;
using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hotel_Management.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork _unitOfWork)
        {
            _logger = logger;
            this._unitOfWork = _unitOfWork;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ChangeRoom(int? RoomIdPresent, int? RoomIdChange)
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
                    }

                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult CheckIn(int? RoomId, string? CustomerName, string? CitizenId, string? PhoneNumber, string? Email, string? Address)
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


                        Booking booking = new Booking();
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
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult CheckOut(int? BookingId)
        {
            var booking = _unitOfWork.BookingRepository.GetBooking(BookingId.Value);

            // Tính số giờ chênh lệch giữa Ngày A và Ngày B
            TimeSpan timeDifference = (TimeSpan)(DateTime.Now - booking.CheckInTime);

            // Lấy giá trị tuyệt đối của số giờ chênh lệch
            TimeSpan absoluteDifference = timeDifference.Duration();

            double totalPrice = Math.Ceiling((double)(absoluteDifference.TotalHours * booking.Room.RoomType.Price));

            ViewBag.TotalPrice = totalPrice;
            ViewBag.BookingId = BookingId;
            return View();
        }

        public IActionResult ClearRoom(int? RoomId)
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
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(int? BookingId, DateTime checkoutTime, Double TotalPrice, int? RoomId)
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
            return RedirectToAction(nameof(Index));
        }
    }
}