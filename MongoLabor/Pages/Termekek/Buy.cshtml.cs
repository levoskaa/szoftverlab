using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System.ComponentModel.DataAnnotations;

namespace MongoLabor.Pages.Termekek
{
    public class BuyModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public BuyModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public Termek Termek { get; set; }

        [BindProperty]
        [Range(1, double.PositiveInfinity)]
        public int Mennyiseg { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Termek = repository.FindTermek(id);

            if (Termek == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var success = repository.TermekElad(id, Mennyiseg);

            if (success)
            {
                return RedirectToPage("./Index");
            }     
            else
            {
                Termek = repository.FindTermek(id);
                ModelState.AddModelError(nameof(Mennyiseg), "Nincs elegendő termék raktáron!");
                return Page();
            }
        }
    }
}
