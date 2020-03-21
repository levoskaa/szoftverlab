using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoLabor.DAL;
using MongoLabor.Models;

namespace MongoLabor.Pages.Megrendelesek
{
    public class CreateModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public CreateModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public SelectList Termekek { get; set; }

        [BindProperty]
        public Megrendeles Megrendeles { get; set; }

        [BindProperty]
        [Required]
        public string TermekID { get; set; }

        [BindProperty]
        [Range(1, double.PositiveInfinity)]
        public int Mennyiseg { get; set; }

        public void OnGet()
        {
            var termekek = repository.ListTermekek();
            Termekek = new SelectList(termekek, "ID", "Nev");
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Megrendeles.Datum.HasValue)
                Megrendeles.Datum = DateTime.SpecifyKind(Megrendeles.Datum.Value, DateTimeKind.Utc);
            if (Megrendeles.Hatarido.HasValue)
                Megrendeles.Hatarido = DateTime.SpecifyKind(Megrendeles.Hatarido.Value, DateTimeKind.Utc);

            var termek = repository.FindTermek(TermekID);
            repository.InsertMegrendeles(Megrendeles, termek, Mennyiseg);

            return RedirectToPage("./Index");
        }
    }
}