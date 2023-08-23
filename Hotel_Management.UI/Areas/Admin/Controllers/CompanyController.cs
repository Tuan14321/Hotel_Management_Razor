using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CompanyController : Controller
    {
        public const int ITEMS_PER_PAGE = 5;
        private IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }
        public IActionResult Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;
            var CompanyData = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                CompanyData = _unitOfWork.CompanyRepository.SearchByName(x => x.CompanyName, searchString);
            }
            var totalItems = CompanyData.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "Company", new { page = totalPages });
            var data = CompanyData
            .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
            .Take(ITEMS_PER_PAGE)
            .ToList();
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;
            return View(data);
        }

        public IActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                var data = _unitOfWork.CompanyRepository.Find(id.Value);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                var company = _unitOfWork.CompanyRepository.Find(id.Value);
                if (company != null)
                {
                    return View(company);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? CompanyId, CompanyViewModel company)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CompanyId.HasValue)
                    {
                        _unitOfWork.CompanyRepository.Update(new Company()
                        {
                            CompanyId = company.CompanyId,
                            CompanyName = company.CompanyName,
                            PhoneNumber = company.PhoneNumber,
                            Fax = company.Fax,
                            Email = company.Email,
                            Address = company.Address,
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
        public IActionResult Create(CompanyViewModel company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CompanyRepository.Add(new Company()
                    {
                        CompanyName = company.CompanyName,
                        PhoneNumber = company.PhoneNumber,
                        Fax = company.Fax,
                        Email = company.Email,
                        Address = company.Address,
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
                _unitOfWork.CompanyRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
