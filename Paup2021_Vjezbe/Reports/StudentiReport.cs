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

        private PdfPCell GenerirajCeliju(string sadrzaj,
            Font font, BaseColor boja, bool wrap)
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
            BaseFont bfontZaglavlje =
                BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, false);
            BaseFont bfontTekst =
                BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfontPodnozje =
                BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);

            Font fontZaglavlje = 
                new Font(bfontZaglavlje, 12, Font.NORMAL, BaseColor.DARK_GRAY);

            Font fontZaglavljeBold =
                new Font(bfontZaglavlje, 12, Font.BOLD, BaseColor.DARK_GRAY);

            Font fontNaslov =
                new Font(bfontTekst, 14, Font.BOLDITALIC, BaseColor.DARK_GRAY);

            Font fontTablicaZaglavlje =
                new Font(bfontTekst, 10, Font.BOLD, BaseColor.WHITE);

            Font fontTekst =
                new Font(bfontTekst, 10, Font.NORMAL, BaseColor.BLACK);

            BaseColor tPozadinaZaglavlje = new BaseColor(11, 65, 121);
            BaseColor tPozadinaSadrzaj = BaseColor.WHITE;

            using(MemoryStream mstream = new MemoryStream())
            {
                using (Document pdfDokument
                    = new Document(PageSize.A4, 50, 50, 20, 50))
                {
                    PdfWriter.GetInstance(pdfDokument, mstream).CloseStream = false;

                    pdfDokument.Open();

                    PdfPTable tZaglavlje = new PdfPTable(2);
                    tZaglavlje.HorizontalAlignment = Element.ALIGN_LEFT;
                    tZaglavlje.DefaultCell.Border = Rectangle.NO_BORDER;
                    tZaglavlje.WidthPercentage = 100f;
                    float[] sirinaKolonaZag = new float[] { 1f, 3f };
                    tZaglavlje.SetWidths(sirinaKolonaZag);

                    var logo = iTextSharp.text.Image.GetInstance(
                        HostingEnvironment.MapPath("~/Images/MEV_LOGO.jpg"));
                    logo.Alignment = Element.ALIGN_LEFT;
                    logo.ScaleAbsoluteWidth(50);
                    logo.ScaleAbsoluteHeight(50);
                    PdfPCell cLogo = new PdfPCell(logo);
                    cLogo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cLogo);

                    Paragraph info = new Paragraph();
                    info.Alignment = Element.ALIGN_RIGHT;
                    info.SetLeading(0, 1.2f);
                    info.Add(new Chunk("MEĐIMURSKO VELEUČILIŠTE U ČAKOVCU \n", fontZaglavljeBold));
                    info.Add(new Chunk("Bana Josipa Jelačića 22a \n Čakovec \n", fontZaglavlje));

                    PdfPCell cInfo = new PdfPCell();
                    cInfo.AddElement(info);
                    cInfo.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cInfo.Border = Rectangle.NO_BORDER;
                    tZaglavlje.AddCell(cInfo);

                    pdfDokument.Add(tZaglavlje);

                    Paragraph pNaslov = new Paragraph("POPIS STUDENATA", fontNaslov);
                    pNaslov.Alignment = Element.ALIGN_CENTER;
                    pNaslov.SpacingBefore = 20;
                    pNaslov.SpacingAfter = 20;
                    pdfDokument.Add(pNaslov);

                    PdfPTable t = new PdfPTable(5);
                    t.WidthPercentage = 100;
                    t.SetWidths(new float[] { 1, 3, 2, 1, 4 });

                    t.AddCell(GenerirajCeliju("R.br.", fontTablicaZaglavlje,
                        tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Ime i prezime", fontTablicaZaglavlje,
                        tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("OIB", fontTablicaZaglavlje,
                        tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Redovni", fontTablicaZaglavlje,
                        tPozadinaZaglavlje, true));
                    t.AddCell(GenerirajCeliju("Smjer", fontTablicaZaglavlje,
                        tPozadinaZaglavlje, true));

                    int i = 1;
                    foreach(Student s in studenti)
                    {
                        t.AddCell(GenerirajCeliju(i.ToString(), fontTekst,
                            tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.PrezimeIme, fontTekst,
                            tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.Oib, fontTekst,
                            tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.RedovniStudent ? "DA" : "NE", fontTekst,
                            tPozadinaSadrzaj, false));
                        t.AddCell(GenerirajCeliju(s.UpisaniSmjer?.Naziv, fontTekst,
                            tPozadinaSadrzaj, false));
                        i++;
                    }

                    pdfDokument.Add(t);

                    Paragraph pMjesto = new Paragraph("Čakovec, " +
                        DateTime.Now.ToString("dd.MM.yyyy"), fontTekst);
                    pMjesto.Alignment = Element.ALIGN_RIGHT;
                    pMjesto.SpacingBefore = 30;
                    pdfDokument.Add(pMjesto);
                }

                Podaci = mstream.ToArray();
            }
        }
    }
}