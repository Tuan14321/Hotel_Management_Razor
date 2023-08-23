using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management.UI.Areas.Admin.Models;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RoomTypeController : Controller
    {
        public const int ITEMS_PER_PAGE = 5;
        private IUnitOfWork _unitOfWork;

        public RoomTypeController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        // GET: Admin/RoomType
        public async Task<IActionResult> Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;
            var Data = _unitOfWork.RoomTypeRepository.GetAll();
            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Data = _unitOfWork.RoomTypeRepository.SearchByName(x => x.RoomTypeName, searchString);
            }
            var totalItems = Data.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "RoomType", new { page = totalPages });
            var data = Data
            .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
            .Take(ITEMS_PER_PAGE)
            .ToList();
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;
            return View(data);
        }

        // GET: Admin/RoomType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var data = _unitOfWork.RoomTypeRepository.Find(id.Value);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/RoomType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RoomType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomTypeId,RoomTypeName,Price,QuantityPeople,QuantityBed")] RoomTypeViewModel roomType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.RoomTypeRepository.Add(new RoomType()
                    {
                        RoomTypeId = roomType.RoomTypeId,
                        RoomTypeName = roomType.RoomTypeName,
                        Price = roomType.Price,
                        QuantityBed = roomType.QuantityBed,
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

        // GET: Admin/RoomType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var company = _unitOfWork.RoomTypeRepository.Find(id.Value);
                if (company != null)
                {
                    return View(company);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/RoomType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? RoomTypeId, [Bind("RoomTypeId,RoomTypeName,Price,QuantityPeople,QuantityBed")] RoomTypeViewModel roomType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (RoomTypeId.HasValue)
                    {
                        _unitOfWork.RoomTypeRepository.Update(new RoomType()
                        {
                            RoomTypeId = roomType.RoomTypeId,
                            RoomTypeName = roomType.RoomTypeName,
                            Price = roomType.Price,
                            QuantityBed = roomType.QuantityBed,
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

        // GET: Admin/RoomType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                _unitOfWork.RoomTypeRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
