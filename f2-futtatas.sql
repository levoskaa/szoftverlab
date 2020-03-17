declare c2 cursor for select ID from Szamla
open c2

declare @szamlaid int
declare @eredmeny int

fetch next from c2 into @szamlaid
  while @@FETCH_STATUS = 0
  begin
    print 'Szamla: ' + convert(varchar(5), @szamlaid)
    exec @eredmeny = SzamlaEllenoriz @szamlaid
    if @eredmeny = 0
    begin
        print 'Helyes szamla'
    end

    fetch next from c2 into @szamlaid
  end

close c2
deallocate c2