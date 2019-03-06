using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TB.AspNetCore.Domain.Entitys
{
    public partial class SystemRoles
    {
        [NotMapped]
        public virtual List<string> Menus { get; set; }
    }
}
