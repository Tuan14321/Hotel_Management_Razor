using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management.UI.Areas.Admin.Models;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class FloorController : Controller
    {
        public const int ITEMS_PER_PAGE = 5;
        private IUnitOfWork _unitOfWork;

        public FloorController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public IActionResult Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;
            var Data = _unitOfWork.FloorRepository.GetAll();
            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Data = _unitOfWork.FloorRepository.SearchByName(x => x.FloorName, searchString);
            }
            var totalItems = Data.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "Floor", new { page = totalPages });
            var data = Data
            .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
            .Take(ITEMS_PER_PAGE)
            .ToList();
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;
            return View(data);
        }

        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                var data = _unitOfWork.FloorRepository.Find(id.Value);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var company = _unitOfWork.FloorRepository.Find(id.Value);
                if (company != null)
                {
                    return View(company);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? FloorId, FloorViewModel floor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (FloorId.HasValue)
                    {
                        _unitOfWork.FloorRepository.Update(new Floor()
                        {
                            FloorId = floor.FloorId,
                            FloorName = floor.FloorName,
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FloorViewModel floor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.FloorRepository.Add(new Floor()
                    {
                        FloorId = floor.FloorId,
                        FloorName = floor.FloorName,
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

        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _unitOfWork.FloorRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
