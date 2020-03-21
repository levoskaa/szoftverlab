using MongoLabor.Models;
using System.Collections.Generic;

namespace MongoLabor.DAL
{
    public interface IAdatvezRepository
    {
        IList<Termek> ListTermekek();
        Termek FindTermek(string id);
        void InsertTermek(Termek termek);
        bool TermekElad(string id, int mennyiseg);
        void DeleteTermek(string id);

        IList<Kategoria> ListKategoriak();

        IList<Megrendeles> ListMegrendelesek(string keresettStatusz);
        Megrendeles FindMegrendeles(string id);
        void InsertMegrendeles(Megrendeles megrendeles, Termek termek, int mennyiseg);
        bool UpdateMegrendeles(Megrendeles megrendeles);
        void DeleteMegrendeles(string id);

        IList<Vevo> ListVevok();

        MegrendelesCsoportok MegrendelesCsoportosit(int csoportDarab);
    }
}
