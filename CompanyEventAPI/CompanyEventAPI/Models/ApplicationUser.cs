﻿using Microsoft.AspNetCore.Identity;

namespace CompanyEventAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
