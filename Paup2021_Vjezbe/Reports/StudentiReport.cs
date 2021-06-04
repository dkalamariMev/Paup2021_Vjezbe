using iTextSharp.text;
using iTextSharp.text.pdf;
using Paup2021_Vjezbe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Paup2021_Vjezbe.Reports
{
    public class StudentiReport
    {
        public byte[] Podaci { get; private set; }

        // metoda vraća formatirani izgled ćelije
        private PdfPCell GenerirajCeliju(string sadrzaj, Font font, BaseColor boja, bool wrap)
        {
            PdfPCell c1 = new PdfPCell(new Phrase(sadrzaj, font));
            c1.BackgroundColor = boja;
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.Padding = 5;
            c1.NoWrap = wrap;
            c1.Border = Rectangle.BOTTOM_BORDER;
            c1.BorderColor = BaseColor.LIGHT_GRAY;
            return c1;
        }

        public void ListaStudenata(List<Student> studenti)
        {
            //definiranje fontova
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontTekst = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontTekst, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTablicaZaglavlje = new Font(bfontTekst, 10, Font.BOLD, BaseColor.WHITE);
            Font fontTekst = new Font(bfontTekst, 10, Font.NORMAL, BaseColor.BLACK);

            //plava boja pozadine za header tablice
            BaseColor tPozadinaZaglavlje = new BaseColor(11, 65, 121);
            //bijela boja pozadine za ćelije sa sadržajem
            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;

                    //Otvaranje dokumenta
                    pdfDokument.Open();

                    //Kreiramo tablicu za ispis zaglavlja - 1. kolona logotip, 2. tekst
                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZag = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZag);

                    //učitavamo sliku
                    var logo = iTextSharp.text.Image.GetInstance(HostingEnvironment.MapPath("~/Images/MEV_LOGO.jpg"));
                    logo.Alignment = Element.ALIGN_LEFT;
                    logo.ScaleAbsoluteWidth(50);
                    logo.ScaleAbsoluteHeight(50);

                    //kreiramo ćeliju za logotip
                    PdfPCell cLogo = new PdfPCell(logo);
                    cLogo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cLogo);

                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;
                    //definiramo prored = 1.2f - 1.2 * veličina fonta
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("MEĐIMURSKO VELEUČILIŠTE U ČAKOVCU \n", fontZaglavljeBold));
                    info.Add(new Chunk("Bana Josipa Jelačića 22a \n Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cInfo);

                    //dodajemo tablicu zaglavlja u dokument
                    pdfDokument.Add(tZaglavlje);

                    // naslov
                    Paragraph pNaslov = new Paragraph("POPIS STUDENATA", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);

                    // tablica za studente
                    PdfPTable t = new PdfPTable(5); // 5 kolona
                    t.WidthPercentage = 100; // širina tablice u odnosu na veličinu dokumenta
                    t.SetWidths(new float[] { 1, 3, 2, 1, 4 }); // relativni odnosi širina kolona

                    // definiranje zaglavlja tablice - dodajemo kolone
                    t.AddCell(GenerirajCeliju("R.br.", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Ime i prezime", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("OIB", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Redovni", fontTablicaZaglavlje, tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Smjer", fontTablicaZaglavlje, tPozadinaZaglavlje, true));

                    // dodajemo redove
                    int i = 1; // brojač redova
                    foreach (Student s in studenti)
                    {
                        t.AddCell(GenerirajCeliju(i.ToString(), fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.PrezimeIme, fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.Oib, fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.RedovniStudent ? "DA" : "NE", fontTekst, tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.UpisaniSmjer?.Naziv, fontTekst, tPozadinaSadrzaj, false));
                        i++;
                    }

                    // dodajemo tablicu na dokument
                    pdfDokument.Add(t);

                    // mjesto i vrijeme
                    Paragraph pMjesto = new Paragraph("Čakovec, " + DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pMjesto.Alignment = Element.ALIGN_RIGHT;
                    pMjesto.SpacingBefore = 30;
                    pdfDokument.Add(pMjesto);
                }

                Podaci = mstream.ToArray();

                //otvaramo generirani PDF iz memorije i upisujemo broj stranice u podnožje
                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {PageCount}", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }

        public void Student(Student student, string kreirao)
        {
            //definiranje fontova
            BaseFont bfontZaglavlje = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontText = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont bfontPodnozje = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);
            Font fontZaglavljeBold = new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);
            Font fontNaslov = new Font(bfontText, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);
            Font fontTekstBold = new Font(bfontText, 12, Font.BOLD, BaseColor.BLACK);
            Font fontTekst = new Font(bfontText, 12, Font.NORMAL, BaseColor.BLACK);

            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using (MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;

                    //Otvaranje dokumenta
                    pdfDokument.Open();

                    //Kreiramo tablicu za ispis zaglavlja - 1. kolona logotip, 2. tekst
                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZag = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZag);


                    //učitavamo sliku
                    var logo = iTextSharp.text.Image.GetInstance(HostingEnvironment.MapPath("~/Images/MEV_LOGO.jpg"));
                    // logo.SetAbsolutePosition(440, 800); // apsolutno poziciniranje
                    logo.Alignment = Element.ALIGN_LEFT; // relativno pozicioniranje
                    logo.ScaleAbsoluteWidth(50); // resize
                    logo.ScaleAbsoluteHeight(50);

                    //kreiramo ćeliju za logotip
                    PdfPCell cLogo = new PdfPCell(logo);
                    cLogo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cLogo);

                    // header - tekst generiramo pomocu objekata Chunk, Phrase i Paragraph
                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;
                    //definiramo prored = 1.2f - 1.2 * veličina fonta
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("MEĐIMURSKO VELEUČILIŠTE U ČAKOVCU \n", fontZaglavljeBold));
                    info.Add(new Chunk(
                      "Bana Josipa Jelačića 22a \n" +
                      "Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cInfo);

                    //dodajemo tablicu zaglavlja u dokument
                    pdfDokument.Add(tZaglavlje);

                    // naslov
                    Paragraph pNaslov = new Paragraph("Podaci o studentu", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);


                    // tablica za studente
                    PdfPTable t = new PdfPTable(2); // 5 kolona
                    t.WidthPercentage = 100; // širina tablice u odnosu na veličinu dokumenta
                    t.SetWidths(new float[] { 1, 3 }); // relativni odnosi širina kolona

                    if (!String.IsNullOrWhiteSpace(student.SlikaPutanja))
                    {
                        PdfPCell cell = new PdfPCell();
                        cell.Border = Rectangle.NO_BORDER;
                        t.AddCell(cell);

                        var slika = iTextSharp.text.Image.GetInstance(HostingEnvironment.MapPath(student.SlikaPutanja));
                        // logo.SetAbsolutePosition(440, 800);
                        slika.Alignment = Element.ALIGN_LEFT;
                        slika.ScaleToFit(150, 150);
                        PdfPCell cSlika = new PdfPCell(slika);
                        cSlika.Border = Rectangle.NO_BORDER;
                        t.AddCell(cSlika);
                    }

                    t.AddCell(GenerirajCeliju("ID studenta:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.Id.ToString(), fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Prezime:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.Prezime, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Ime:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.Ime, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Spol:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.Spol, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("OIB:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.Oib, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Datum rođenja:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.DatumRodjenja.ToString("dd.MM.yyyy"), fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Redovni student:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.RedovniStudent ? "Da" : "Ne", fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Smjer:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.UpisaniSmjer?.Naziv, fontTekst, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju("Godina studija:", fontTekstBold, tPozadinaSadrzaj, false));
                    t.AddCell(GenerirajCeliju(student.GodinaStudija.ToString(), fontTekst, tPozadinaSadrzaj, false));

                    // dodajemo tablicu na dokument
                    pdfDokument.Add(t);

                    // mjesto i vrijeme
                    pNaslov = new Paragraph("Čakovec, " + System.DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pNaslov.Alignment = Element.ALIGN_RIGHT;
                    pNaslov.SpacingBefore = 30;
                    pdfDokument.Add(pNaslov);

                    // kreirao
                    pNaslov = new Paragraph("Kreirao: " + kreirao, fontTekst);
                    pNaslov.Alignment = Element.ALIGN_RIGHT;
                    pNaslov.SpacingBefore = 10;
                    pdfDokument.Add(pNaslov);
                }

                Podaci = mstream.ToArray();

                //otvaramo generirani PDF i upisujemo broj stranice u podnožje
                using (var reader = new PdfReader(Podaci))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var stamper = new PdfStamper(reader, ms))
                        {
                            int PageCount = reader.NumberOfPages;
                            for (int i = 1; i <= PageCount; i++)
                            {
                                Rectangle pageSize = reader.GetPageSize(i);
                                PdfContentByte canvas = stamper.GetOverContent(i);

                                canvas.BeginText();
                                canvas.SetFontAndSize(bfontPodnozje, 10);

                                canvas.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, $"Stranica {i} / {PageCount}", pageSize.Right - 50, 30, 0);
                                canvas.EndText();
                            }
                        }
                        Podaci = ms.ToArray();
                    }
                }
            }
        }
    }
}