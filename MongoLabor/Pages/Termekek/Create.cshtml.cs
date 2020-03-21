using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;

namespace MongoLabor.Pages.Termekek
{
    public class CreateModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public CreateModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Termek Termek { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            repository.InsertTermek(Termek);

            return RedirectToPage("./Index");
        }
    }
}