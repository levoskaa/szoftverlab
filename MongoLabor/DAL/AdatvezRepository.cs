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
        private readonly IMongoCollection<Entities.Megrendeles> megrendelesCollection;
        private readonly IMongoCollection<Entities.Vevo> vevoCollection;

        public AdatvezRepository(IMongoDatabase database)
        {
            this.termekCollection = database.GetCollection<Entities.Termek>("termekek");
            this.kategoriaCollection = database.GetCollection<Entities.Kategoria>("kategoriak");
            this.megrendelesCollection = database.GetCollection<Entities.Megrendeles>("megrendelesek");
            this.vevoCollection = database.GetCollection<Entities.Vevo>("vevok");
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
            List<Entities.Megrendeles> dbMegrendelesek;
            if (string.IsNullOrEmpty(keresettStatusz))
            {
                dbMegrendelesek = megrendelesCollection
                .Find(_ => true)
                .ToList();
            }
            else
            {
                dbMegrendelesek = megrendelesCollection
                .Find(m => m.Statusz.Equals(keresettStatusz))
                .ToList();
            }

            var osszErtekek = megrendelesCollection
                    .Find(_ => true)
                    .Project(m => new { ID = m.ID, Ossz = m.MegrendelesTetelek.Sum(mt => mt.Mennyiseg * mt.NettoAr) })
                    .ToList();

            return dbMegrendelesek.Select(m => new Megrendeles
                { 
                    ID = m.ID.ToString(),
                    Datum = m.Datum,
                    Hatarido = m.Hatarido,
                    Statusz = m.Statusz,
                    FizetesMod = m.FizetesMod.Mod,
                    OsszErtek = osszErtekek.SingleOrDefault(oe => oe.ID == m.ID)?.Ossz ?? 0
                })
                .ToList();
        }

        public Megrendeles FindMegrendeles(string id)
        {
            var dbMegrendeles = megrendelesCollection
                .Find(m => m.ID == ObjectId.Parse(id))
                .SingleOrDefault();
            if (dbMegrendeles == null)
                return null;

            var osszErtek = megrendelesCollection
                .Find(m => m.ID == ObjectId.Parse(id))
                .Project(m => new { ID = m.ID, Ossz = m.MegrendelesTetelek.Sum(mt => mt.Mennyiseg * mt.NettoAr) })
                .SingleOrDefault();

            return new Megrendeles
            {
                ID = dbMegrendeles.ID.ToString(),
                Datum = dbMegrendeles.Datum,
                Hatarido = dbMegrendeles.Hatarido,
                Statusz = dbMegrendeles.Statusz,
                FizetesMod = dbMegrendeles.FizetesMod.Mod,
                OsszErtek = osszErtek?.Ossz ?? 0
            };
        }

        public void InsertMegrendeles(Megrendeles megrendeles, Termek termek, int mennyiseg)
        {
            var dbMegrendeles = new Entities.Megrendeles
            {
                VevoID = ObjectId.Parse("5d7e42adcffa8e1b64f7dbb9"),
                TelephelyID = ObjectId.Parse("5d7e42adcffa8e1b64f7dbba"),
                Datum = megrendeles.Datum,
                Hatarido = megrendeles.Hatarido,
                Statusz = megrendeles.Statusz,
                FizetesMod = new Entities.FizetesMod { Mod = megrendeles.FizetesMod },
                MegrendelesTetelek = new Entities.MegrendelesTetel[]
                    {
                        new Entities.MegrendelesTetel
                        {
                            TermekID = ObjectId.Parse(termek.ID),
                            NettoAr = termek.NettoAr,
                            Mennyiseg = mennyiseg,
                            Statusz = megrendeles.Statusz
                        }
                    }
            };
            megrendelesCollection.InsertOne(dbMegrendeles);
        }

        public bool UpdateMegrendeles(Megrendeles megrendeles)
        {
            var result = megrendelesCollection.UpdateOne(
                filter: m => m.ID == ObjectId.Parse(megrendeles.ID),
                update: Builders<Entities.Megrendeles>.Update.Combine(
                        Builders<Entities.Megrendeles>.Update.Set(m => m.Datum, megrendeles.Datum),
                        Builders<Entities.Megrendeles>.Update.Set(m => m.Hatarido, megrendeles.Hatarido),
                        Builders<Entities.Megrendeles>.Update.Set(m => m.Statusz, megrendeles.Statusz),
                        Builders<Entities.Megrendeles>.Update.Set(m => m.FizetesMod.Mod, megrendeles.FizetesMod)
                    ),
                options: new UpdateOptions { IsUpsert = false });
            return result.MatchedCount > 0;
        }

        public void DeleteMegrendeles(string id)
        {
            megrendelesCollection.DeleteOne(m => m.ID == ObjectId.Parse(id));
        }

        public IList<Vevo> ListVevok()
        {
            var dbVevok = vevoCollection
                .Find(_ => true)
                .ToList();

            var osszMegrendelesek = megrendelesCollection
                .Aggregate()
                .Group(m => m.VevoID, g => new { VevoID = g.Key, Ossz = g.Sum(x => x.MegrendelesTetelek.Sum(mt => mt.Mennyiseg * mt.NettoAr)) })
                .ToList();

            return dbVevok
                .Select(v =>
                {
                    var kozpontiTelephelyID = dbVevok
                        .Single(w => w.ID == v.ID).KozpontiTelephelyID;
                    var kozpontiTelephely = dbVevok
                        .Single(w => w.ID == v.ID)
                        .Telephelyek
                        .Single(t => t.ID == kozpontiTelephelyID);
                    var osszMegrendeles = osszMegrendelesek
                        .SingleOrDefault(o => o.VevoID == v.ID);

                    return new Vevo { Nev = v.Nev, IR = kozpontiTelephely.IR, Utca = kozpontiTelephely.Utca,
                                      Varos = kozpontiTelephely.Varos, OsszMegrendeles =  osszMegrendeles?.Ossz};
                })
                .ToList();
        }

        public MegrendelesCsoportok MegrendelesCsoportosit(int csoportDarab)
        {
            throw new NotImplementedException();
        }
    }
}
