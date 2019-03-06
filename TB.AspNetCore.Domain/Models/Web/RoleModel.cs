using System.Collections.Generic;

namespace TB.AspNetCore.Domain.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Menus { get; set; }
    }
}
