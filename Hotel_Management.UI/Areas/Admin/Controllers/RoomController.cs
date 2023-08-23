using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management.UI.Areas.Admin.Models;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel_Management.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RoomController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public const int ITEMS_PER_PAGE = 5;

        public RoomController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public IActionResult Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;

            var data = _unitOfWork.RoomRepository.GetAll();
            ViewBag.SearchString = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                data = _unitOfWork.RoomRepository.SearchByName(x => x.RoomName, searchString);

            }
            foreach (var room in data)
            {
                room.RoomType = _unitOfWork.RoomTypeRepository.Find(room.RoomTypeId);
                room.Floor = _unitOfWork.FloorRepository.Find(room.FloorId);
            }
            var totalItems = data.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "Room", new { page = totalPages });
            var departments = data
            .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
            .Take(ITEMS_PER_PAGE)
            .ToList();
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;

            return View(departments);
        }

        // GET: Admin/Department/Details/5
        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var data = _unitOfWork.RoomRepository.Find(id.Value);
                data.RoomType = _unitOfWork.RoomTypeRepository.Find(data.RoomTypeId);
                data.Floor = _unitOfWork.FloorRepository.Find(data.FloorId);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Department/Create
        public IActionResult Create()
        {
            var roomtype = _unitOfWork.RoomTypeRepository.GetAll();
            var floor = _unitOfWork.FloorRepository.GetAll();
            ViewBag.RoomType = new SelectList(roomtype, "RoomTypeId", "RoomTypeName");
            ViewBag.Floor = new SelectList(floor, "FloorId", "FloorName");
            return View();
        }

        // POST: Admin/Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomViewModel room)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.RoomRepository.Add(new Room()
                    {
                        RoomId = room.RoomId,
                        RoomName = room.RoomName,
                        StatusId = 1,
                        FloorId = room.FloorId,
                        RoomTypeId = room.RoomTypeId,
                    });
                    _unitOfWork.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
            return View();
        }

        // GET: Admin/Department/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var room = _unitOfWork.RoomRepository.Find(id.Value);
                if (room != null)
                {
                    var roomtype = _unitOfWork.RoomTypeRepository.GetAll();
                    var floor = _unitOfWork.FloorRepository.GetAll();
                    ViewBag.RoomType = new SelectList(roomtype, "RoomTypeId", "RoomTypeName");
                    ViewBag.Floor = new SelectList(floor, "FloorId", "FloorName");
                    return View(room);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        // POST: Admin/Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? RoomId, RoomViewModel room)
        {
            if (RoomId != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (RoomId.HasValue)
                    {
                        _unitOfWork.RoomRepository.Update(new Room()
                        {
                            RoomId = room.RoomId,
                            RoomName = room.RoomName,
                            StatusId = room.StatusId,
                            FloorId = room.FloorId,
                            RoomTypeId = room.RoomTypeId,
                        });
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

        // GET: Admin/Department/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _unitOfWork.RoomRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
