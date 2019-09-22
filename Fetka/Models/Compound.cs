using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fetka.Models
{
    public class Compound
    {
        public int id_compound { get; set; }
        public string name { get; set; }
        public string cas { get; set; }
        public string purity { get; set; }
        public bool certificate { get; set; }
        public virtual Group group { get; set; }
        public string specification { get; set; }

    }
}
