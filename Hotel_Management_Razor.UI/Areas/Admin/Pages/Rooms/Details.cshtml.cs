﻿using System;
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

namespace Hotel_Management_Razor.UI.Areas.Admin.Pages.Rooms
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext _context;

        public DetailsModel(Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext context)
        {
            _context = context;
        }

      public Room Room { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Room == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            else 
            {
                Room = room;
            }
            return Page();
        }
    }
}
