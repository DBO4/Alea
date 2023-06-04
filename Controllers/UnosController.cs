using Alea.Data;
using Alea.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Globalization;

namespace Alea.Controllers
{
    public class UnosController : Controller
    {

        private readonly AleaContext _context;
        private readonly ILogger<UnosController> _logger;
        private readonly IWebHostEnvironment webHostEnviroment;
        public UnosController(AleaContext context, ILogger<UnosController> logger, IWebHostEnvironment hostEnvironment)
        {
            this._context = context;
            this._logger = logger;
            this.webHostEnviroment= hostEnvironment;

        }
        public IActionResult UnesiKnjige()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UnesiKnjige(Knjige knjiga, IFormFile slika, string vrsta, DateTime datumIzdavanja)
        {
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await slika.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

           


            var knjig = new Knjige()
            {
                idKnjige = new Guid(),
                opis = knjiga.opis,
                naslov = knjiga.naslov,
                datumIzdavanja = datumIzdavanja,
                naslovna = imageData,
                likovi = knjiga.likovi,
                vrsta  = vrsta == "knjiga"
            };


            try
            {
                using (var dbContext = _context) 
                {
                    dbContext.Knjige.Add(knjig);
                    dbContext.SaveChanges();
                }

                return RedirectToAction("UnesiKnjige"); 
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error"); 
            }
        }

        public IActionResult UnesiViseKnjiga()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UnesiViseKnjiga(IFormFile knjigeFajl)
        {
            try
            {
                using (var dbContext = _context)
                {
                    using (var reader = new StreamReader(knjigeFajl.OpenReadStream()))
                    {
                        var json = reader.ReadToEnd();
                        var knjige = JsonConvert.DeserializeObject<List<Knjige>>(json);

                        foreach (var knjigaModel in knjige)
                        {
                            if (knjigaModel.naslovna.ToString() == null)
                            {
                                ViewBag.ErrorMessage = "NEOK";
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "OK";
                            }
                            string filePath = knjigaModel.naslovna.ToString(); 

                            byte[] imageData;
                            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                            {
                                imageData = new byte[fileStream.Length];
                                await fileStream.ReadAsync(imageData, 0, (int)fileStream.Length);
                            }

                            var dbKnjiga = new Knjige
                            {
                                idKnjige = Guid.NewGuid(),
                                naslov = knjigaModel.naslov,
                                opis = knjigaModel.opis,
                                datumIzdavanja = knjigaModel.datumIzdavanja,
                                naslovna = imageData,
                                vrsta = knjigaModel.vrsta,
                                likovi = knjigaModel.likovi
                            };

                            dbContext.Knjige.Add(dbKnjiga);
                        }
                    }

                    dbContext.SaveChanges();
                }

                return RedirectToAction("UnesiViseKnjiga");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.InnerException?.Message ?? ex.Message;
                return View("Error");
            }
        }

        public static byte[] Base64ToHexBytes(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            return bytes;
        }




    }
}
