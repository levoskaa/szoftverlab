using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System.Collections.Generic;

namespace MongoLabor.Pages.Vevok
{
    public class IndexModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public IndexModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IList<Vevo> Vevok { get; set; }

        public void OnGet()
        {
            Vevok = repository.ListVevok();
        }
    }
}