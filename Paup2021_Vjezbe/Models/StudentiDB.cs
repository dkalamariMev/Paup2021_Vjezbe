using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paup2021_Vjezbe.Models
{
    public class StudentiDB
    {
        private static List<Student> lista = new List<Student>();
        private static bool listaInicijalizirana = false;

        //Konstruktor se izvršava kod instanciranja klase, tj kad koristimo naredbu StudentiDB studenti = new StudentiDB();
        public StudentiDB()
        {
            //samo kod prvog instanciranja ove klase će vrijednost varijable listaInicijalizirana
            //biti false jer je varijabla kao i lista static što znači da svi objekti koriste istu
            //vrijednost te varijable i liste
            if (listaInicijalizirana == false)
            {
                //nakon svakog sljedećeg instanciranja će biti true pa se donji blok naredbi
                //više neće izvršavati tj, lista se više neće puniti

                listaInicijalizirana = true;

                lista.Add(new Student()
                {
                    Id = 1,
                    Prezime = "Perić",
                    Ime = "Petar",
                    DatumRodjenja = new DateTime(1995, 10, 15),
                    Spol = 'M',
                    GodinaStudija = GodinaStudija.Druga,
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
                    GodinaStudija = GodinaStudija.Prva,
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
                    GodinaStudija = GodinaStudija.Treca,
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
                    GodinaStudija = GodinaStudija.Prva,
                    Oib = "12344448911",
                    RedovniStudent = true
                }
                );
            }
        }

        //Metoda koja vraća listu studenata popunjenu u konstruktoru
        public List<Student> VratiListu()
        {
            return lista;
        }

        public void AzurirajStudenta(Student student)
        {
            //Pronalazimo lokaciju studenta u listi
            int studentIndex = lista.FindIndex(x => x.Id == student.Id);
            //Na tu lokaciju u listi stavljamo ažurirani objekt s podacima o studentu
            lista[studentIndex] = student;
        }
    }
}