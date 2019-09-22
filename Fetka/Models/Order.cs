using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fetka.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual Compound OrderedCompound { get; set; }
        public int MyProperty { get; set; }
    }
}