using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class ClaimForCreationDto
    {
        [Required]
        [MaxLength(10)]
        public string Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }
    }
}
