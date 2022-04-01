using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Accounts
{
    public class EditAccountInfo
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string MeterNumber { get; set; }
        [Required]
        public string ConsumerType { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
