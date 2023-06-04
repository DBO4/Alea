using Alea.Auth;
using Alea.Data;
using Alea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Alea.Controllers
{

    public class PregledController : Controller
    {


        private readonly AleaContext _context;
        private readonly ILogger<PregledController> _logger;
        private readonly IWebHostEnvironment webHostEnviroment;
        public PregledController(AleaContext context, ILogger<PregledController> logger, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._logger = logger;
            this.webHostEnviroment = hostEnvironment;

        }


        [HttpGet]
        public IActionResult PregledKnjiga()
        {
            var knjige = _context.Knjige.ToList();
            return View(knjige);
        }

        [HttpPost]
        public async Task<IActionResult> UnesiOcjenu(IFormCollection form)
        {
            var stariUnos = _context.Ocjene.FirstOrDefault(o => o.idKnjie == Guid.Parse(Request.Form["idKnjige"]) && o.revizor == User.Identity.Name);

            if (stariUnos != null) {
                stariUnos.ocjena = Int32.Parse(Request.Form["selectedRating"]);
            }
            else
            {
                var ocjena = new Ocjene()
                {
                    idOcjene = new Guid(),
                    idKnjie = Guid.Parse(Request.Form["idKnjige"]),
                    ocjena = Int32.Parse(Request.Form["selectedRating"]),
                    revizor = User.Identity.Name
                };
                _context.Ocjene.Add(ocjena);
            }
            _context.SaveChanges();
       
            return RedirectToAction("PregledKnjiga");
        }



        [HttpGet]
        public IActionResult PregledTop10(bool toggle)
        {
            var knjige = GetTopRatedBooks(toggle);
            return View(knjige);
        }


        [HttpGet]
        public IActionResult PregledTop10Json(bool toggle)
        {
            var knjige = GetTopRatedBooks(toggle);
            return Json(knjige);
        }

        [HttpGet]
        public IActionResult PretraziKnjige(string termin)
        {
            var knjige = DonesiTrazeno(termin);
            return Json(knjige);
        }

        public List<ModeliMV.IscitajKnjige> DonesiTrazeno(string termin)
        {
            
            using (var dbContext = _context)
            {
                string[] rijeci = termin.Split(' ');
                if (termin.ToLower().Contains("zvjezdica") ||
                   termin.ToLower().Contains("zvjezdice") ||
                   termin.ToLower().Contains("zvijezde") ||
                   termin.ToLower().Contains("zvezdice") ||
                   termin.ToLower().Contains("zvezdica") ||
                   termin.ToLower().Contains("zvezde") ||
                   termin.ToLower().Contains("zvezda") ||
                   termin.ToLower().Contains("star"))
                {
                    int ocjena;
                    foreach (string rijec in rijeci)
                    {
                        if (int.TryParse(rijeci[0], out ocjena))
                        {
                            var rezultat = dbContext.Knjige
                           .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                           .GroupBy(b => b.Book)
                           .Where(g => g.Average(r => r.Rating.ocjena) >= (ocjena - 0.5) && g.Average(r => r.Rating.ocjena) < (ocjena + 0.5))
                           .Select(g => new ModeliMV.IscitajKnjige
                           {
                               naslov = g.Key.naslov,
                               prosjekOcjene = g.Average(r => r.Rating.ocjena),
                               naslovna = g.Key.naslovna,
                               opis = g.Key.opis,
                               datumIzdavanja = g.Key.datumIzdavanja
                           })
                            .ToList();
                            return rezultat;
                        }
                        else
                        {
                            var rezultat = dbContext.Knjige
                           .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                           .GroupBy(b => b.Book)
                           .Select(g => new ModeliMV.IscitajKnjige
                           {
                               naslov = g.Key.naslov,
                               prosjekOcjene = g.Average(r => r.Rating.ocjena),
                               naslovna = g.Key.naslovna,
                               opis = g.Key.opis,
                               datumIzdavanja = g.Key.datumIzdavanja
                           })
                            .ToList();
                            return rezultat;
                        }
                    }

                }
                else if (termin.ToLower().Contains("poslije") ||
                        termin.ToLower().Contains("posle") ||
                        termin.ToLower().Contains("after"))
                {
                    int godina;
                    foreach (string rijec in rijeci)
                    {
                        if (int.TryParse(rijeci[1], out godina))
                        {
                            var rezultat = dbContext.Knjige
                            .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                            .Where(b => b.Book.datumIzdavanja.Year > godina)
                            .GroupBy(b => b.Book)
                            .Select(g => new ModeliMV.IscitajKnjige
                            {
                                naslov = g.Key.naslov,
                                prosjekOcjene = g.Average(r => r.Rating.ocjena),
                                naslovna = g.Key.naslovna,
                                opis = g.Key.opis,
                                datumIzdavanja = g.Key.datumIzdavanja
                            })
                            .ToList();
                            return rezultat;
                        }
                        else
                        {
                            var rezultat = dbContext.Knjige
                           .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                           .GroupBy(b => b.Book)
                           .Select(g => new ModeliMV.IscitajKnjige
                           {
                               naslov = g.Key.naslov,
                               prosjekOcjene = g.Average(r => r.Rating.ocjena),
                               naslovna = g.Key.naslovna,
                               opis = g.Key.opis,
                               datumIzdavanja = g.Key.datumIzdavanja
                           })
                            .ToList();
                            return rezultat;
                        }
                    }
                }
                else if (termin.ToLower().Contains("starije od") ||
                        termin.ToLower().Contains("stariji od") ||
                        termin.ToLower().Contains("starija od") ||
                        termin.ToLower().Contains("older then"))
                {
                    int godine;
                    foreach (string rijec in rijeci)
                    {
                        if (int.TryParse(rijeci[2], out godine))
                        {

                            var rezultat = dbContext.Knjige
                            .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                            .Where(b => b.Book.datumIzdavanja.Year == 2022 )
                            .GroupBy(b => b.Book)
                            .Select(g => new ModeliMV.IscitajKnjige
                            {
                                naslov = g.Key.naslov,
                                prosjekOcjene = g.Average(r => r.Rating.ocjena),
                                naslovna = g.Key.naslovna,
                                opis = g.Key.opis,
                                datumIzdavanja = g.Key.datumIzdavanja
                            })
                            .ToList();
                            return rezultat;
                        }
                        else
                        {
                            var rezultat = dbContext.Knjige
                           .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                           .GroupBy(b => b.Book)
                           .Select(g => new ModeliMV.IscitajKnjige
                           {
                               naslov = g.Key.naslov,
                               prosjekOcjene = g.Average(r => r.Rating.ocjena),
                               naslovna = g.Key.naslovna,
                               opis = g.Key.opis,
                               datumIzdavanja = g.Key.datumIzdavanja
                           })
                            .ToList();
                            return rezultat;
                        }

                    }
                }
                else
                {
                    var rezultat = dbContext.Knjige
                             .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                             .Where(b => b.Book.naslov.Contains(termin) || b.Book.likovi.Contains(termin) || b.Book.opis.Contains(termin))
                             .GroupBy(b => b.Book)
                             .Select(g => new ModeliMV.IscitajKnjige
                             {
                                 naslov = g.Key.naslov,
                                 prosjekOcjene = g.Any() ? g.Average(r => r.Rating.ocjena) : 0,
                                 naslovna = g.Key.naslovna,
                                 opis = g.Key.opis,
                                 datumIzdavanja = g.Key.datumIzdavanja
                             })
                             .ToList();
                    return rezultat;
                }
                var rezultatG = dbContext.Knjige
                         .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                         .Where(b => b.Book.naslov.Contains(termin) || b.Book.likovi.Contains(termin) || b.Book.opis.Contains(termin))
                         .GroupBy(b => b.Book)
                         .Select(g => new ModeliMV.IscitajKnjige
                         {
                             naslov = g.Key.naslov,
                             prosjekOcjene = g.Any() ? g.Average(r => r.Rating.ocjena) : 0,
                             naslovna = g.Key.naslovna,
                             opis = g.Key.opis,
                             datumIzdavanja = g.Key.datumIzdavanja
                         })
                         .ToList();
                return rezultatG;
            }
        }

        public List<ModeliMV.IscitajKnjige> GetTopRatedBooks(bool toggle)
        {
            using (var dbContext = _context)
            {
                var booksWithRatings = dbContext.Knjige
                .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                .Where(b => b.Book.vrsta == toggle)
                .GroupBy(b => b.Book)
                .Select(g => new ModeliMV.IscitajKnjige
                {
                    naslov = g.Key.naslov,
                    prosjekOcjene = g.Average(r => r.Rating.ocjena),
                    naslovna = g.Key.naslovna
                })
                .ToList();

                var sortedBooks = booksWithRatings.OrderByDescending(b => b.prosjekOcjene);
                var topRatedBooks = sortedBooks.Take(10).ToList();

                return topRatedBooks;
            }
        }

    }
}
