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
    public class DepartmentController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public const int ITEMS_PER_PAGE = 5;

        public DepartmentController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        public IActionResult Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;
            var data = _unitOfWork.DepartmentRepository.GetAll();
            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                data = _unitOfWork.DepartmentRepository.SearchByName(x => x.DepartmentName, searchString);

            }
            foreach (var department in data)
            {
                department.Company = _unitOfWork.CompanyRepository.Find((int)department.CompanyId);
            }
            var totalItems = data.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "Department", new { page = totalPages });
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
                var data = _unitOfWork.DepartmentRepository.Find(id.Value);
                data.Company = _unitOfWork.CompanyRepository.Find((int)data.CompanyId);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Department/Create
        public IActionResult Create()
        {
            var company = _unitOfWork.CompanyRepository.GetAll();
            ViewBag.Company = new SelectList(company, "CompanyId", "CompanyName");
            return View();
        }

        // POST: Admin/Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentViewModel department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.Add(new Department()
                    {
                        DepartmentName = department.DepartmentName,
                        PhoneNumber = department.PhoneNumber,
                        Fax = department.Fax,
                        Email = department.Email,
                        Address = department.Address,
                        CompanyId = department.CompanyId,
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
                var departments = _unitOfWork.DepartmentRepository.Find(id.Value);
                if (departments != null)
                {
                    var company = _unitOfWork.CompanyRepository.GetAll();
                    ViewBag.Company = new SelectList(company, "CompanyId", "CompanyName");
                    return View(departments);
                }
            }
            return RedirectToAction(nameof(Index));
        }


        // POST: Admin/Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? DepartmentId, DepartmentViewModel department)
        {
            if (DepartmentId != department.DepartmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (DepartmentId.HasValue)
                    {
                        _unitOfWork.DepartmentRepository.Update(new Department()
                        {
                            DepartmentId = department.DepartmentId,
                            DepartmentName = department.DepartmentName,
                            PhoneNumber = department.PhoneNumber,
                            Address = department.Address,
                            Fax = department.Fax,
                            Email = department.Email,
                            CompanyId = department.CompanyId,
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
                _unitOfWork.DepartmentRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
