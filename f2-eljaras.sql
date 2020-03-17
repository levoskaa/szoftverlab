create procedure SzamlaEllenoriz
    @szamlaid int
as
begin
declare @szamlamennyiseg int
declare @megrendelestetelid int
declare @megrendelesmennyiseg int
declare @nev nvarchar(50)
declare @return int = 0

declare c cursor for select Mennyiseg, MegrendelesTetelID from SzamlaTetel where SzamlaID = @szamlaid
open c

fetch next from c into @szamlamennyiseg, @megrendelestetelid
  while @@FETCH_STATUS = 0
  begin
    set @megrendelesmennyiseg = (select Mennyiseg from MegrendelesTetel where ID = @megrendelestetelid)

    if @szamlamennyiseg <> @megrendelesmennyiseg
    begin
        set @nev = (select Nev from SzamlaTetel where MegrendelesTetelID = @megrendelestetelid)
        print 'Elteres: ' + @nev
	    + ' (szamlan ' + convert(varchar(5), @szamlamennyiseg)
	    + ' megrendelesen ' + convert(varchar(5), @megrendelesmennyiseg) + ')'
        set @return = 1
    end

    fetch next from c into @szamlamennyiseg, @megrendelestetelid
  end

  close c
  deallocate c

  return @return
end