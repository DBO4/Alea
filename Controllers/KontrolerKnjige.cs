using Alea.Data;
using Alea.Models;
using Microsoft.AspNetCore.Mvc;
using Alea.Areas.Identity.Data;

namespace Alea.Controllers
{
    public class KontrolerKnjige : Controller
    {

        private readonly AleaContext _context;
        public KontrolerKnjige(AleaContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            var topRatedBooks = GetTopRatedBooks();
            return View(topRatedBooks);
        }

        public List<ModeliMV.IscitajKnjige> GetTopRatedBooks()
        {
            using (var dbContext = _context) 
            {
                var booksWithRatings = dbContext.Knjige
                .Join(dbContext.Ocjene, b => b.idKnjige, r => r.idKnjie, (book, rating) => new { Book = book, Rating = rating })
                .GroupBy(b => b.Book)
                .Select(g => new ModeliMV.IscitajKnjige
                {
                    naslov = g.Key.naslov,
                    prosjekOcjene = g.Average(r => r.Rating.ocjena)
                })
                .ToList();

                var sortedBooks = booksWithRatings.OrderByDescending(b => b.prosjekOcjene);
                var topRatedBooks = sortedBooks.Take(10).ToList();

                return topRatedBooks;
            }
        }
    }
}
