using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Models
{
    public class AuthModel
    {
        [Required]
        [MaxLength(50)]
        public string eMail { get; set; }

        [Required]
        [MaxLength(8)]
        public string Password { get; set; }
    }

    public class ValidateModel
    {
        [Required]
        public string Token { get; set; }
    }

}
