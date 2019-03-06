using System;
using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class SystemRoles
    {
        public SystemRoles()
        {
            BackstageUser = new HashSet<BackstageUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<BackstageUser> BackstageUser { get; set; }
    }
}
