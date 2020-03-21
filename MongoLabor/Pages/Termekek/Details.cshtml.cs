using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;

namespace MongoLabor.Pages.Termekek
{
    public class DetailsModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public DetailsModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public Termek Termek { get; set; }

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
    }
}
