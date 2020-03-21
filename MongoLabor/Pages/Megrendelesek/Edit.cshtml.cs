using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System;

namespace MongoLabor.Pages.Megrendelesek
{
    public class EditModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public EditModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        [BindProperty]
        public Megrendeles Megrendeles { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Megrendeles = repository.FindMegrendeles(id);

            if (Megrendeles == null)
            {
                return NotFound();
            }
            return Page();
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

            bool success = repository.UpdateMegrendeles(Megrendeles);
            if (success)
                return RedirectToPage("./Index");
            else
                return NotFound();
        }
    }
}
