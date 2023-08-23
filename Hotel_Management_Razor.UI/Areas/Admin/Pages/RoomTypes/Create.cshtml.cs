using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Hotel_Management_Razor.UI.Data;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hotel_Management_Razor.UI.Areas.Admin.Pages.RoomTypes
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext _context;

        public CreateModel(Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RoomType RoomType { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.RoomType == null || RoomType == null)
            {
                return Page();
            }

            _context.RoomType.Add(RoomType);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
