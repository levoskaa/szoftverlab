using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System.Collections.Generic;

namespace MongoLabor.Pages.Termekek
{
    public class IndexModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public IndexModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IList<Termek> Termekek { get; set; }

        public void OnGet()
        {
            Termekek = repository.ListTermekek();
        }
    }
}
