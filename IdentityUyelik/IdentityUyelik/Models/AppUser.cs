﻿using Microsoft.AspNetCore.Identity;

namespace IdentityUyelik.Models
{
    public class AppUser : IdentityUser
    {
        public string City { get; set; }

        public string Picture { get; set; }

        public DateTime BirthDay { get; set; }

        public Gender Gender { get; set; }


    }
}
