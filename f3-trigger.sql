create trigger SzamlaTetelszamKarbantart
on SzamlaTetel
after insert, update, delete
as
begin
	declare @szamlaid int
	declare @mennyiseg int

	declare c cursor for select SzamlaID, Mennyiseg from inserted
	open c
		fetch next from c into @szamlaid, @mennyiseg
		while @@FETCH_STATUS = 0
		begin
			update Szamla
			set Tetelszam += @mennyiseg
			where ID = @szamlaid

			fetch next from c into @szamlaid, @mennyiseg
		end
	close c
	deallocate c

	declare c cursor for select SzamlaID, Mennyiseg from deleted
	open c
		fetch next from c into @szamlaid, @mennyiseg
		while @@FETCH_STATUS = 0
		begin
			update Szamla
			set Tetelszam -= @mennyiseg
			where ID = @szamlaid

			fetch next from c into @szamlaid, @mennyiseg
		end
	close c
	deallocate c
end