using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Fetka.Commons;

namespace Fetka.Models
{
    public class SimpleUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Display(Name = "Rola")]
        public Roles Role { get; set; }
        [Display(Name = "Zablokowany")]
        public bool Blocked { get; set; }
    }
}
