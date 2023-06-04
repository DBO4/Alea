using System.ComponentModel.DataAnnotations;

namespace Alea.Models
{
    public class ModeliMV
    {
        public class IscitajKnjige
        {
            //public Guid idKnjige { get; set; }
            public string naslov { get; set; }
            public string opis { get; set; }
            public byte[] naslovna { get; set; }
            public DateTime datumIzdavanja { get; set; }
            public bool vrsta { get; set; }
            public double prosjekOcjene { get; set; }
        }
    }
}
