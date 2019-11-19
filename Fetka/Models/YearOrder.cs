using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fetka.Models
{
    [Table("yearorder")]
    public class YearOrder
    {
        [Key]
        public int Id { get; set; }
        public virtual Compound Compound { get; set; }
        public int CompoundId { get; set; }
        [Required]
        [Display(Name = "Rok")]
        public int Year { get; set; }
        [Required]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Zrealizowany")]
        public bool IsRealized { get; set; }
        [Required]
        [Display(Name = "Usunięty")]
        public bool IsDeleted { get; set; }
    }
}
