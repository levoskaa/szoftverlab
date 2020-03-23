using MongoDB.Bson;
using MongoDB.Driver;
using MongoLabor.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoLabor.DAL
{
    public class AdatvezRepository : IAdatvezRepository
    {
        private readonly IMongoCollection<Entities.Termek> termekCollection;
        private readonly IMongoCollection<Entities.Kategoria> kategoriaCollection;

        public AdatvezRepository(IMongoDatabase database)
        {
            this.termekCollection = database.GetCollection<Entities.Termek>("termekek");
            this.kategoriaCollection = database.GetCollection<Entities.Kategoria>("kategoriak");
        }

        public IList<Termek> ListTermekek()
        {
            var dbTermekek = termekCollection
                .Find(_ => true)
                .ToList();
            return dbTermekek
                .Select(t => new Termek
                {
                    ID = t.ID.ToString(),
                    Nev = t.Nev,
                    NettoAr = t.NettoAr,
                    Raktarkeszlet = t.Raktarkeszlet
                })
                .ToList();
        }

        public Termek FindTermek(string id)
        {
            var dbTermek = termekCollection
                .Find(t => t.ID == ObjectId.Parse(id))
                .SingleOrDefault();
            if (dbTermek == null)
                return null;
            return  new Termek
                {
                    ID = dbTermek.ID.ToString(),
                    Nev = dbTermek.Nev,
                    NettoAr = dbTermek.NettoAr,
                    Raktarkeszlet = dbTermek.Raktarkeszlet
                };
        }

        public void InsertTermek(Termek termek)
        {
            var dbTermek = new Entities.Termek
            {
                Nev = termek.Nev,
                NettoAr = termek.NettoAr,
                Raktarkeszlet = termek.Raktarkeszlet,
                AFA = new Entities.AFA { Nev = "Általános", Kulcs = 20 },
                KategoriaID = ObjectId.Parse("5d7e42adcffa8e1b64f7dbc1")
            };
            termekCollection.InsertOne(dbTermek);
        }

        public bool TermekElad(string id, int mennyiseg)
        {
            var result = termekCollection.UpdateOne(
                filter: t => t.ID == ObjectId.Parse(id) && t.Raktarkeszlet >= mennyiseg,
                update: Builders<Entities.Termek>.Update.Inc(t => t.Raktarkeszlet, -mennyiseg),
                options: new UpdateOptions { IsUpsert = false });
            return result.MatchedCount > 0;
        }

        public void DeleteTermek(string id)
        {
            termekCollection.DeleteOne(t => t.ID == ObjectId.Parse(id));
        }

        public IList<Kategoria> ListKategoriak()
        {
            var dbKategoriak = kategoriaCollection
                .Find(_ => true)
                .ToList();

            var termekDarabok = termekCollection
                .Aggregate()
                .Group(t => t.KategoriaID, g => new { KategoriaID = g.Key, TermekDarab = g.Count() })
                .ToList();

            return dbKategoriak
                .Select(k =>
                {
                    string szuloKategoriaNev = null;
                    if (k.SzuloKategoriaID.HasValue)
                        szuloKategoriaNev = dbKategoriak.Single(sz => sz.ID == k.SzuloKategoriaID.Value).Nev;

                    var termekDarab = termekDarabok.SingleOrDefault(td => td.KategoriaID == k.ID)?.TermekDarab ?? 0;

                    return new Kategoria { Nev = k.Nev, SzuloKategoriaNev = szuloKategoriaNev, TermekDarab = termekDarab };
                })
                .ToList();
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
