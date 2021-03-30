using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Paup2021_Vjezbe.Models
{
    public class Student
    {
        [Display(Name = "ID studenta")] //Sadržaj HTML helpera Label
        public int Id { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "{0} je obavezno")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        public string Ime { get; set; }

        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "{0} je obavezno")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} mora biti duljine minimalno {2} a maksimalno {1} znakova")]
        public string Prezime { get; set; }

        [Display(Name = "Spol")]
        public char Spol { get; set; }

        [Display(Name = "OIB")]
        public string Oib { get; set; }

        [Display(Name = "Datum rođenja")]
        [Required(ErrorMessage ="{0} je obavezan")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }

        [Display(Name = "Godina studija")]
        public GodinaStudija GodinaStudija { get; set; }

        [Display(Name = "Redovan student")]
        public bool RedovniStudent { get; set; }

        [Display(Name="Broj upisanih kolegija")]
        [Required(ErrorMessage ="{0} je obavezan")]
        [Range(1,8, ErrorMessage = "{0} mora biti između {1} i {2}")]
        public int BrojUpisanihKolegija { get; set; }
    }
}