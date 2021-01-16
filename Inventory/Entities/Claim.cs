using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Entities
{
    public class Claim
    {
        [Key]
        public Guid Id{ get; set; }

        [Required]
        [MaxLength(10)]
        public string Type { get; set; }

        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public string UserId { get; set; }
    }
}
