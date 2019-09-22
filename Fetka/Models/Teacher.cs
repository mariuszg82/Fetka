using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fetka.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int Level { get; set; }

        public virtual List<Student> Students { get; set; }

    }
}