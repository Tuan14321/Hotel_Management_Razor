using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_Management_Razor.UI.Data;
using Hotel_Management_Razor.UI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Hotel_Management_Razor.UI.Areas.Admin.Pages.Rooms
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext _context;

        public EditModel(Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room =  await _context.Room.FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            Room = room;
           ViewData["FloorId"] = new SelectList(_context.Floor, "FloorId", "FloorId");
           ViewData["RoomTypeId"] = new SelectList(_context.RoomType, "RoomTypeId", "RoomTypeId");
           ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "StatusId", "StatusName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(Room.RoomId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RoomExists(int id)
        {
          return (_context.Room?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
