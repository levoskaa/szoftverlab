using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoLabor.DAL;
using MongoLabor.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MongoLabor.Pages.MegrendelesCsoportok
{
    public class IndexModel : PageModel
    {
        private readonly IAdatvezRepository repository;

        public IndexModel(IAdatvezRepository repository)
        {
            this.repository = repository;
        }

        public IList<DateTime> Hatarok { get; set; }
        public Dictionary<DateTime, MegrendelesCsoport> Csoportok { get; set; }

        [BindProperty(SupportsGet = true)]
        [Range(1, double.PositiveInfinity)]
        public int CsoportDarab { get; set; } = 5;

        public void OnGet()
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            var megrendelesCsoportok = repository.MegrendelesCsoportosit(CsoportDarab);
            Hatarok = megrendelesCsoportok.Hatarok;
            Csoportok = megrendelesCsoportok.Csoportok.ToDictionary(cs => cs.Datum);
        }
    }
}