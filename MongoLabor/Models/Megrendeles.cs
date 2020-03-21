using System;
using System.ComponentModel.DataAnnotations;

namespace MongoLabor.Models
{
    public class Megrendeles
    {
        public string ID { get; set; }

        [Required]
        public DateTime? Datum { get; set; }
        [Required]
        public DateTime? Hatarido { get; set; }
        [Required]
        public string Statusz { get; set; }
        [Required]
        public string FizetesMod { get; set; }
        public double? OsszErtek { get; set; }
    }
}
