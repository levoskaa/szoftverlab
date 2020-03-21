using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;

namespace MongoLabor.Pages.Megrendelesek
{
    public class DetailsModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public DetailsModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

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
    }
}
