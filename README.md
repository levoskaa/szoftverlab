# Lekérdezés optimalizálás labor

## Feladat 1

A kiadott parancsok:

- `select * from vevo`
- `select * from vevo where id = 1`
- a többi is hasonló, csak az `id` mezőt használjuk

A lekérdezési terv mindegyikre nagyon hasonló, mindegyik _table scan_-t használt:

![](f1.png)

Magyarázat: az optimalizáló nem tud indexet használni, a tárolás sorrendjével kapcsolatban sem élhet feltételezéssel, így minden lekérdezés _table scan_ lesz.

## Feladat 2

a) A kiadott parancsok:

- `select * from vevo`

![](f2a.png)

Magyarázat: Elsődleges kulcs mentén létrejön a Clustered Index, a tábla rekordjai index szerint növekvő sorrendben vannak tárolva, így a lekérdező index szerint végig megy a táblán.

b) A kiadott parancsok:

- `select * from vevo where id = 1`

![](f2b.png)

Magyarázat: Clustered Index Seekre változott, mert a feltétel az elsődleges kulccsal ellátott oszlop egy elemére vonatkozik.

c) A kiadott parancsok:

- `select * from vevo where id <> 1`

A lekérdezési terv nem változott.

Magyarázat: Az optimalizáló külön-külön kiértékeli az id hol nagyobb és hol kisebb mint 1. A lekérdezés elsődleges kulcsra vonatkozik, ezért nagyon gyors.

d) A kiadott parancsok:

- `select * from vevo where id > 1`

A lekérdezési terv nem változott.

Magyarázat: Ugyanúgy, mint az előző, de csak egy intervallummal.

e) A kiadott parancsok:

- `select * from vevo where id > 1 order by id desc`

![](f2e.png)

Magyarázat: Az optimalizáló kikeresi a határon lévő rekordot (ahol id = 1) és onnan visszafele indul el, így a rekordok az eredményben eleve csökkenő sorrendben vannak rendezve.

## Feladat 3

f) A kiadott parancsok:

- `select * from termek`

![](f3f.png)

Magyarázat: Lásd Feladat 2 a).

g) A kiadott parancsok:

- `select * from termek where NettoAr = 800`

![](f3g.png) TODO

Magyarázat: Az egyenlőségi feltétel nem elsődleges kulcsra vonatkozik, így a teljes táblát végig kell nézni és mindenhol ki kell érteklni a feltételt.

h) A kiadott parancsok:

- `select * from termek where NettoAr <> 800`

A lekérdezési terv ugyanaz marad.

Magyarázat: Nincs változás, mert még mindig nem elsődleges kulcsra vonatkozik a feltétel.

i) A kiadott parancsok:

- `select * from termek where NettoAr > 800`

A lekérdezési terv ugyanaz marad.

Magyarázat: Nincs változás, mert még mindig nem elsődleges kulcsra vonatkozik a feltétel.

j) A kiadott parancsok:

- `select * from termek where NettoAr > 800 order by NettoAr desc`

![](f3j.png)

Magyarázat: Scan után van még egy költséges rendezés, mivel a kiolvasott rekordok nem NettoAr, hanem id szerint vannak rendezve eredetileg.

## Feladat 4

f) A kiadott parancsok:

- `select * from termek`

![](f4f.png)

Magyarázat: Nem számít, hogy van-e indexünk vagy nincs, teljes tábla lekérdezésénél az optimalizálónak mindig végig kell mennie az összes rekordon.

g) A kiadott parancsok:

- `select * from termek where NettoAr = 800`

![](f4g.png)

Magyarázat: Ha az optimalizáló a létrehozott index alapján dolgozik, akkor onnan először rekord referenciákat kap vissza, ami alapján még ki kell keresnie a tényleges rekordokat. Mivel a termék tábla kicsi, ezért az optimalizó dönthet úgy, hogy kisebb költséggel jár kiolvasni az egész táblát mint referenciákkal játszani.

h) A kiadott parancsok:

- `select * from termek where NettoAr <> 800`

A lekérdezési terv ugyanaz marad.

Magyarázat: Lásd Feladat 4 g).

i) A kiadott parancsok:

- `select * from termek where NettoAr > 800`

A lekérdezési terv ugyanaz marad.

Magyarázat: Lásd Feladat 4 g).

j) A kiadott parancsok:

- `select * from termek where NettoAr > 800 order by NettoAr desc`

![](f4j.png)

Magyarázat: A Non-Clustered index visszaadja a referenciákat, amik alapján kikeressük a rekordokat és mivel a referenciák sorrendben vannak, ezért ha ezekhez kigyűjtjük az egyes rekordokat ezzel megspórolható a rendezés.

## Feladat 5

f) A kiadott parancsok:
- `select * from termek`

Magyarázat: Nincs változás Feladat 4 f)-hez képest.

g) A kiadott parancsok:
- `select * from termek where NettoAr = 800`

![](f5g.png)

Magyarázat: Sima Clustered index scan nagy méretű táblára nagyon drága. Mivel a NettoAr oszlopra az egyenlőség operátor nagyon jól szűr (valószínűleg nagyon kevés rekordban szerepel ugyanaz az érték), így a Non-Clustered index tényleg a segítségünkre lehet.

h) A kiadott parancsok:
- `select * from termek where NettoAr <> 800`

![](f5h.png)

Magyarázat: Ha az egyenlőség operátor jól szűr, akkor a nem-egyenlőség nem jól szűr, így mindenképpen sok rekordot kell végigolvasni, nincs értelme Non-Clustered indexet használni.

i) A kiadott parancsok:
- `select * from termek where NettoAr > 800`

![](f5i.png)

Magyarázat: A lekérdezés végrahajtási terve attól függ, hogy milyen konstanst választunk. Ha az adott konstans jól szűr (pl. nagy szám) akkor használja a Non-Clustered index scant, ha viszont rosszul szűr akkor Clustered index scant fog használni.

j) A kiadott parancsok:
- `select * from termek where NettoAr > 800 order by NettoAr desc`

![](f5j.png)

Magyarázat: Mindegy, hogy a megadott konstans jól vagy rosszul szűr, az optimalizálónak fontosabb, hogy elkerülje a költséges rendezést, ezért használni fogja a Non-Clustered indexet.

## Következő nagyon izgalmas feladat

Add meg a használt SQL utasításokat. Ha egy feladatban nagyon hasonlóak (mint fentebb), nem szükséges mindegyiket megadod, csak jelezd 1-2 példával.

A kapott lekérdezési tervet képként tedd be. Ha a lekérdezési tervek nagyon hasonlóak (mint az első feladatban), elég csak egyet megmutatnod.

Értékeld a kapott tervet, magyarázd meg, mit látsz, és miért.
