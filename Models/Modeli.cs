using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alea.Models
{
    public class Knjige
    {
        [Key]
        public Guid idKnjige { get; set; }
        public string naslov { get; set; }
        public string opis { get; set; }

        //[NotMapped]
        public byte[] naslovna { get; set; }

        public DateTime datumIzdavanja { get; set; }
        public bool vrsta { get; set; }
        public string likovi { get; set;}
    }

    public class Ocjene{
        [Key]
        public Guid idOcjene { get; set;}
        public Guid idKnjie { get; set;}
        public int ocjena { get; set;}
        public string revizor { get; set; }
    }

    public class Glumci
    {
        [Key]
        public Guid idGlumci { get; set; }
        public string ime { get; set; }
        public string prezime { get; set; }
    }


}
