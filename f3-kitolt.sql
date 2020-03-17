declare @szamlaid int
declare @szum int

declare c cursor for select ID from Szamla
open c

fetch next from c into @szamlaid
while @@FETCH_STATUS = 0
begin
    set @szum = (select SUM(Mennyiseg) from SzamlaTetel where SzamlaID = @szamlaid)

    update Szamla
    set Tetelszam = @szum
    where ID = @szamlaid

    fetch next from c into @szamlaid
end

close c
deallocate c