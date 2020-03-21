using System.ComponentModel.DataAnnotations;

namespace MongoLabor.Models
{
    public class Termek
    {
        public string ID { get; set; }

        [Required]
        public string Nev { get; set; }
        [Required]
        public double? NettoAr { get; set; }
        [Required]
        public int? Raktarkeszlet { get; set; }
    }
}
