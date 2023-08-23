using Hotel_Management.Core.Repository.UnitOfWork;
using Hotel_Management.UI.Areas.Admin.Models;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CustomerController : Controller
    {
        public const int ITEMS_PER_PAGE = 5;
        private IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork _unitOfWork)
        {
            this._unitOfWork = _unitOfWork;
        }

        // GET: Admin/Customer
        public async Task<IActionResult> Index([Bind(Prefix = "page")] int pageNumber, string? searchString)
        {
            if (pageNumber == 0)
                pageNumber = 1;
            var Data = _unitOfWork.CustomerRepository.GetAll();
            ViewBag.SearchString = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                Data = _unitOfWork.CustomerRepository.SearchByName(x => x.CustomerName, searchString);
            }
            var totalItems = Data.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);
            if (pageNumber > totalPages)
                return RedirectToAction("Index", "Customer", new { page = totalPages });
            var data = Data
                .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
                .Take(ITEMS_PER_PAGE)
                .ToList();
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;
            return View(data);
        }

        // GET: Admin/Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var data = _unitOfWork.CustomerRepository.Find(id.Value);
                return View(data);
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,CitizenId,PhoneNumber,Email,Address")] CustomerViewModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CustomerRepository.Add(new Customer()
                    {
                        CustomerId = customer.CustomerId,
                        CustomerName = customer.CustomerName,
                        Email = customer.Email,
                        Address = customer.Address,
                        CitizenId = customer.CitizenId,
                        PhoneNumber = customer.PhoneNumber,
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

        // GET: Admin/Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id.HasValue)
            {
                var company = _unitOfWork.CustomerRepository.Find(id.Value);
                if (company != null)
                {
                    return View(company);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? CustomerId, [Bind("CustomerId,CustomerName,CitizenId,PhoneNumber,Email,Address")] CustomerViewModel customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (CustomerId.HasValue)
                    {
                        _unitOfWork.CustomerRepository.Update(new Customer()
                        {
                            CustomerId = customer.CustomerId,
                            CustomerName = customer.CustomerName,
                            Email = customer.Email,
                            Address = customer.Address,
                            CitizenId = customer.CitizenId,
                            PhoneNumber = customer.PhoneNumber,
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

        // GET: Admin/Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                _unitOfWork.CustomerRepository.Delete(id.Value);
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
