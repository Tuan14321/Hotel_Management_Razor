using Hotel_Management.UI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FA.JustBlog.UI.Areas.Identity.Pages.Roles
{
    public class UserModel : PageModel
    {
        const int USER_PER_PAGE = 10;

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<Hotel_ManagementUIUser> _userManager;

        public UserModel(RoleManager<IdentityRole> roleManager,
            UserManager<Hotel_ManagementUIUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }
        public class UserInRole : Hotel_ManagementUIUser
        {
            public string ListRoles { get; set; }
        }

        public List<UserInRole> users;
        public int TotalPage { get; set; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public int pageNumber { set; get; }

        public void OnPost() => NotFound("Not found!");
        public async Task<IActionResult> OnGet()
        {
            if (pageNumber == 0) pageNumber = 1;

            var listUsers = _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u =>
                new UserInRole()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                });

            int totalUsers = await listUsers.CountAsync();

            TotalPage = (int)Math.Ceiling(totalUsers * 1.0 / USER_PER_PAGE);

            users = await listUsers
                .Skip(USER_PER_PAGE * (pageNumber - 1))
                .Take(USER_PER_PAGE)
                .ToListAsync();

            //getRoles for each user
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.ListRoles = string.Join(",", roles.ToList());
            }

            return Page();
        }
    }
}
