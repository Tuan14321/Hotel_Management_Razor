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
using Hotel_Management_Razor.UI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Hotel_Management_Razor.UI.Areas.Admin.Pages.Floors
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext _context;
        private readonly IHubContext<SignalRServer> _signalRHub;


        public CreateModel(Hotel_Management_Razor.UI.Data.Hotel_Management_RazorUIContext context, IHubContext<SignalRServer> signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;

        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Floor Floor { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Floor == null || Floor == null)
            {
                return Page();
            }

            _context.Floor.Add(Floor);
            await _context.SaveChangesAsync();
            await _signalRHub.Clients.All.SendAsync("LoadFloors");

            return RedirectToPage("./Index");
        }
    }
}
