using MongoDB.Driver;
using MongoLabor.Models;
using System;
using System.Collections.Generic;

namespace MongoLabor.DAL
{
    public class AdatvezRepository : IAdatvezRepository
    {
        public AdatvezRepository(IMongoDatabase database)
        {
            // TODO
        }

        public IList<Termek> ListTermekek()
        {
            throw new NotImplementedException();
        }

        public Termek FindTermek(string id)
        {
            throw new NotImplementedException();
        }

        public void InsertTermek(Termek termek)
        {
            throw new NotImplementedException();
        }

        public bool TermekElad(string id, int mennyiseg)
        {
            throw new NotImplementedException();
        }

        public void DeleteTermek(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Kategoria> ListKategoriak()
        {
            throw new NotImplementedException();
        }

        public IList<Megrendeles> ListMegrendelesek(string keresettStatusz = null)
        {
            throw new NotImplementedException();
        }

        public Megrendeles FindMegrendeles(string id)
        {
            throw new NotImplementedException();
        }

        public void InsertMegrendeles(Megrendeles megrendeles, Termek termek, int mennyiseg)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMegrendeles(Megrendeles megrendeles)
        {
            throw new NotImplementedException();
        }

        public void DeleteMegrendeles(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Vevo> ListVevok()
        {
            throw new NotImplementedException();
        }

        public MegrendelesCsoportok MegrendelesCsoportosit(int csoportDarab)
        {
            throw new NotImplementedException();
        }
    }
}
