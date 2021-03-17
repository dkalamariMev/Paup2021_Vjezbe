using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paup2021_Vjezbe.Models
{
    public class StudentiDB
    {
        private List<Student> lista = new List<Student>();

        //Konstruktor se izvršava kod instanciranja klase, tj kad koristimo naredbu StudentiDB studenti = new StudentiDB();
        public StudentiDB()
        {
            lista.Add(new Student()
                {
                    Id = 1,
                    Prezime = "Perić",
                    Ime = "Petar",
                    DatumRodjenja = new DateTime(1995, 10, 15),
                    Spol = 'M',
                    GodinaStudija = 2,
                    Oib = "12345678911",
                    RedovniStudent = true
                }
            );

            lista.Add(new Student()
            {
                Id = 2,
                Prezime = "Mat",
                Ime = "Ines",
                DatumRodjenja = new DateTime(1998, 10, 15),
                Spol = 'Z',
                GodinaStudija = 1,
                Oib = "12345678911",
                RedovniStudent = true
            }
            );

            lista.Add(new Student()
            {
                Id = 3,
                Prezime = "Bes",
                Ime = "Marta",
                DatumRodjenja = new DateTime(1988, 04, 01),
                Spol = 'Z',
                GodinaStudija = 3,
                Oib = "12345678911",
                RedovniStudent = false
            }
            );

            lista.Add(new Student()
            {
                Id = 4,
                Prezime = "Preko",
                Ime = "Jura",
                DatumRodjenja = new DateTime(1995, 10, 15),
                Spol = 'M',
                GodinaStudija = 1,
                Oib = "12344448911",
                RedovniStudent = true
            }
            );
        }

        //Metoda koja vraća listu studenata popunjenu u konstruktoru
        public List<Student> VratiListu()
        {
            return lista;
        }
    }
}