# Egyszerű játék XAML alapokon

## Áttekintés
A játék egy egyszerű ***desktop*** alkalmazás (fontos döntés!) ami méri és így segít fejleszteni a reakcióidőnket. A játék lényege, hogy kártyákat mutatunk egymás után, és minden alkalommal el kell dönteni, hogy az adott kártya egyforma-e az előzővel.


## Szereplők
### Tudom Ányos

  A szellemi képességeinek a növekesét kívánja nyomon követni.

  Mivel az egyes reakcióidők és a játékidő hosszának mérése fontos, ezeket pontosan kell mérni, adja magát a számítógépes megvalósítás.

## Forgatókönyvek
### Játék (A felhasználó szemszögéből)
  
  Ányos elindítja az alkalmazást (fontos döntés!), majd, ha felkészült, akkor elindítja a játékot. Ha végzett, akkor a végeredményt a játék kijelzi.

  Mivel rövid reakcióidőt mérünk, ezért az alkalmazás indításának idejére nem használhatjuk a játékot. Ezért az alkalmazás indítása és a játék elkezdése között különbséget kell tenni.

  Hány képernyő legyen?

  Egy és három között valahol:

  - alkalmazás induló képernyője 
  
    mindkét kártya lefordítva ezt megoldja

  - játék képernyő
  - végeredmény képernyő 
  
    mindkét kártya lefordítva megoldja, mert az eredményt folyamatosan kijelezzük

Kezdetben elindulhatunk úgy, hogy a játék kezdőképernyője egyben az alkalmazás induló képernyője. És, a játék utolsó képernyője az a végeredmény képernyője.

  Fontos, hogy a lényeg a reakcióidő mérése, így nem gombokra hanem billentyűleütésekre tesszük a játékos reakcióit. Viszont, érdemes megjeleníteni játék közben a lehetőségeket, erre jók lesznek a gombok.

Játék közben vagy eltaláljuk a helyes választ, vagy nem. Ha eltaláljuk, azt a képernyőn például zöld pipával tudjuk jelezni.
Ha nem találjuk el a helyes választ, akkor lehet piros kereszt, vagy az előző kártyalap megmutatásával a felhasználónak jelezni lehet a hibás választ.

![Játék képernyő](img/playdisplay.png)

### A játék menete részletesen (a programozó szemszögéből)

Elindul a játék
- A kezdőképernyőn nincs semmilyen felfordított kártya.
- Megmutatjuk az első kártyát
  
  Fontos, hogy egymás után következhessen két egyforma kártya. Ez csak akkor lehetséges, ha vannak egyforma kártyák a pakliban. Vagy minden húzás után visszatesszük a kártyát és újra megkeverjük, vagy a pakliban eleve több egyforma kártya van.

  A programozáshoz válasszunk egy könnyen áttekinthető lépéssorozatot: minden lépés után keverjük újra az egész paklit.
  Már csak azt kell eldönteni, hogy mekkora legyen a kártyapakli. Minél nagyobb a kártyapakli, annál valószínűtlenebb, hogy egymás után kétszer ugyanaz a kártya legyen a legtetején a paklinak.

  Tehát egy értelmes kártyapakli méretet kell választanunk, legyen 6 kezdetben, azonban azt vegyük észre, hogy ez a szám, ez nem biztos, hogy megfelel egy élvezetes játéknak. Így már most érdemes rögzíteni, hogy később ez a szám változhat.
  
  - Minden alkalommal megkeverjük a kártyát és vesszük a felső lapot
  - Minden alkalommal kivesszük véletlenszerűen az egyik lapot, majd visszatesszük
  - dobunk egy dobókockával, és amelyik számot dobtuk azt a számú kártyát választjuk.

  Ez a három ugyanazt eredményezi, de programozni az utolsót a legegyszerűbb.

  **ilyenkor még nem várunk visszajelzést**

- Megmutatjuk a következő kártyát
  
  Ugyanúgy, mint az első kártyánál
  
  - Várunk a felhasználó visszajelzésére
    - első lépésben gombokon keresztül
    - második lépésben a billentyűzetről
  - Vagy lejár az idő
  - Ha a felhasználó reagált, értékeljük a visszajelzést
    - Jó/nem jó
    - Mennyi volt a reakcióidő
  - Az értékelést megjelenítjük
    - jó/nem jó
    - pontszám frissítése (hogy számoljuk a pontokat?)

- Ezt ismételjük, amíg le nem jár az idő 
  - a hátralévő időt folyamatosan kijelezzük (mennyi a játékidő?)


