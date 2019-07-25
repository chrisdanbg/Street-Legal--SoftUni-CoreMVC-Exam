using System;
using Microsoft.AspNetCore.Identity;

namespace StreetLegal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public string DriverName { get; set; }
    }
}
