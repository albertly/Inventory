using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class UserDto
    {
        
        public string Id { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public Guid IdGuid { get; set; }

    }
}
