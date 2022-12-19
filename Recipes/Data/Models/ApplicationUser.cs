using Microsoft.AspNetCore.Identity;
using System;

namespace Recipes.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
    }
}
