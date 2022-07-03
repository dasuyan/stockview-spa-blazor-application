using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace APBDProject.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Subscription> Subscriptions { get; set; }
    }
}
