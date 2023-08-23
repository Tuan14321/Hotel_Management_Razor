using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hotel_Management_Razor.UI.Data;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hotel_Management_Razor.UI.Areas.Admin.Pages.Floors
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext _context;

        public IndexModel(Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext context)
        {
            _context = context;
        }

        public IList<Floor> Floor { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Floor != null)
            {
                Floor = await _context.Floor.ToListAsync();
            }
        }
    }
}
