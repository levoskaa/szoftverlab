create trigger KategoriaSzulovelBeszur -- a trigger neve
on KategoriaSzulovel -- a nézet neve
instead of insert    -- beszúrás helyetti trigger
as
begin
  declare @ujnev nvarchar(255) -- a kapott adatok
  declare @szulonev nvarchar(255)
  declare @szuloid int

  -- kurzort használunk, mert az inserted implicit táblában
  -- több elem is lehet, és mi egyesével dolgozzuk fel
  declare ic cursor for select * from inserted
  open ic
  -- standard kurzor használat
  fetch next from ic into @ujnev, @szulonev
  while @@FETCH_STATUS = 0
  begin
    -- ide jön a változóban elérhető adatok ellenőrzése
    -- ha hiba van, akkor dobjunk hibát
    -- ha minden rendben van, akkor jöhet beszúrás    
    set @szuloid = null

    if @szulonev is not null
    begin
        set @szuloid = (select ID from Kategoria where Nev = @szulonev)
        if @szuloid is null
        begin
            ;throw 51000, 'Hiba', 1;
        end
    end

    insert into Kategoria(Nev, SzuloKategoria)
    values (@ujnev, @szuloid)

    fetch next from ic into @ujnev, @szulonev
  end

  close ic -- kurzor lezárása és felszabadítása
  deallocate ic
end