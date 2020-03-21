using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System.Collections.Generic;

namespace MongoLabor.Pages.Megrendelesek
{
    public class IndexModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public IndexModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IList<Megrendeles> Megrendelesek { get; set; }

        [BindProperty(SupportsGet = true)]
        public string KeresettStatusz { get; set; }

        public void OnGet()
        {
            Megrendelesek = repository.ListMegrendelesek(KeresettStatusz);
        }
    }
}
