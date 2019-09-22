using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fetka.Models
{
    public class Student
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int IndexNumber { get; set; }
        public int TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
    }
}