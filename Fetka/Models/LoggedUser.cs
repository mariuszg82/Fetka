using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fetka.Commons;

namespace Fetka.Models
{
    public class LoggedUser
    {
        public string UserId { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Roles Role { get; set; }
    }
}
