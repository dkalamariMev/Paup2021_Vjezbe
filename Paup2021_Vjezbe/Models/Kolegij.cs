using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paup2021_Vjezbe.Models
{
    [Table("kolegiji")]
    public class Kolegij
    {
        [Key]
        [Display(Name = "ID kolegija")]
        public long Id { get; set; }

        [Display(Name = "Naziv")]
        [Required(ErrorMessage = "{0} je obavezno")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        public string Naziv { get; set; }

        [Column("nositelj_kolegija")]
        [Display(Name = "Nositelj kolegija")]
        [Required(ErrorMessage = "{0} je obavezno")]
        public string NositeljKolegija { get; set; }

        [Display(Name = "Semestar")]
        [Required(ErrorMessage = "{0} je obavezan")]
        [Range(1, 6, ErrorMessage = "Vrijednost {0} mora biti između {1} i {2}")]
        public int Semestar { get; set; }

        [Display(Name = "ECTS bodovi")]
        [Required(ErrorMessage = "{0} su obavezni")]
        public int Ects { get; set; }
    }
}