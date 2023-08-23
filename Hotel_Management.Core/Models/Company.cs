using System;
using System.Collections.Generic;

namespace Hotel_Management_Razor.UI.Models
{
    public partial class Company
    {
        public Company()
        {
            Departments = new HashSet<Department>();
        }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
    }
}
