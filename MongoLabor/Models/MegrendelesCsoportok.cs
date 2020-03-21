using System;
using System.Collections.Generic;

namespace MongoLabor.Models
{
    public class MegrendelesCsoportok
    {
        public IList<DateTime> Hatarok { get; set; }
        public IList<MegrendelesCsoport> Csoportok { get; set; }
    }
}
