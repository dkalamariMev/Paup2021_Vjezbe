using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Paup2021_Vjezbe.Controllers;
using Paup2021_Vjezbe.Models;

namespace Paup2021Vjezbe_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
       private int Zbroji(int a, int b)
        {
            return a + b;
        }

        [TestMethod]
        public void TestZbrajanje()
        {
            //Arrange
            int x = 10;
            int y = 5;

            //Act
            int zbroj = Zbroji(x, y);

            //Assert
            Assert.AreEqual(15, zbroj);
        }

        [TestMethod]
        public void OIB_ValidacijaDuzina11Znakova()
        {
            Student s = new Student()
            {
                Oib = new string('a', 10)
            };

            var context = new ValidationContext(s)
                { MemberName = nameof(Student.Oib) };

            var results = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(s.Oib, context, results);

            Assert.IsFalse(valid);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("OIB mora biti duljine 11 znakova", results[0].ErrorMessage);
        }

        [TestMethod]
        public void OIB_ValidacijaInvalid()
        {
            string oib = "1246";
            bool valid = OIB.CheckOIB(oib);
            Assert.IsFalse(valid);
        }

        [TestMethod]
        public void OIB_ValidacijaValid()
        {
            string oib = "50050041051";
            bool valid = OIB.CheckOIB(oib);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void TestStudentiIndexTitle()
        {
            //Arrange
            StudentiController controller =
                new StudentiController();

            //Act
            ViewResult result = controller.Index() as ViewResult;

            //Assert
            Assert.AreEqual("Početna stranica o studentima", result.ViewBag.Title);
        }

        [TestMethod]
        public void Kolegij_NazivNotNull()
        {
            // Arrange
            Kolegij testKolegij = new Kolegij
            {
                Naziv = null
            };

            // Act
            var ctx = new ValidationContext(testKolegij) { MemberName = nameof(testKolegij.Naziv) };
            var result = new List<ValidationResult>();
            var valid = Validator.TryValidateProperty(testKolegij.Naziv, ctx, result);

            // Assert
            Assert.IsFalse(valid);
            Assert.AreEqual(result.Count, 1);
            Assert.AreEqual("Naziv je obavezno", result[0].ErrorMessage);
        }

        [TestMethod]
        public void Kolegij_KreiranjeBrisanje()
        {
            BazaDbContext db = new BazaDbContext();

            Kolegij kolegij = new Kolegij()
            {
                Id = 0,
                Naziv = "Kolegij",
                Ects = 4,
                NositeljKolegija = "Mirko",
                Semestar = 4
            };

            db.PopisKolegija.Add(kolegij);
            db.SaveChanges();

            db.PopisKolegija.Remove(kolegij);
            int obrisano = db.SaveChanges();

            Assert.AreEqual(1, obrisano);
        }

        [TestMethod]
        public void Smjerovi_DuplikatException()
        {
            SmjeroviController contr =
                new SmjeroviController();

            Smjer testSmjer = new Smjer()
            {
                Sifra = "MS",
                Naziv = "Menadžment sporta"
            };

            Assert.ThrowsException<DbUpdateException>(() => contr.Create(testSmjer));
        }
    }
}
