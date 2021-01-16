using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]        
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public Guid IdGuid { get; set; }

        [Required]
        [MaxLength(500)]
        public string Password { get; set; }

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }
}
