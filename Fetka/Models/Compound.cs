using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fetka.Models
{
    [Table("compound")]
    public class Compound
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Brak nazwy odczynnika.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Display(Name = "Numer CAS")]
        //[RegularExpression("[1-9]{1}[0-9]{1,5}-[0-9]{2}-[0-9]{1}", ErrorMessage = "Numer CAS składa się tylko z cyfr w trzech grupach.")]
        public string CAS { get; set; }
        [Display(Name = "Czystość")]
        public string Purity { get; set; }
        [Display(Name = "Świadectwo")]
        public bool Certificate { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Usunięty")]
        public bool IsDeleted { get; set; }
        public virtual ICollection<YearOrder> Orders { get; set; }
        [Display(Name ="Podgląd")]
        public string Image { get; set; }
    }
}
