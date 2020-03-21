using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System.Collections.Generic;

namespace MongoLabor.Pages.Kategoriak
{
    public class IndexModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public IndexModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IList<Kategoria> Kategoriak { get; set; }

        public void OnGet()
        {
            Kategoriak = repository.ListKategoriak();
        }
    }
}