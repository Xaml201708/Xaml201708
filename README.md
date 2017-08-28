# Xaml201708
A NetAcademia Egyszerű játék készítése XAML alapokon tanfolyamának kiegészítő kódtára

## Előkészületek
A tanfolyamon a Windows operációs rendszeren futó [Visual Studio 2015 Community](https://www.visualstudio.com/vs/community/) fejlesztői keretrendszert fogunk használni. Ingyenes és egy jó parancssorban két parancs kiadásával telepíthető.

### Visual Studio 2015 Community telepítése
A tanfolyamon ezt az alkalmazást fogjuk használni. Ingyenesen elérhető önálló fejlesztők, nyílt forráskódú projektek, akadémiai kutatók számára. Továbbá oktatási célokra és kis (max 5 fős) fejlesztőcsapatoknak.

Letölteni az előző linkről vagy [innen](https://www.visualstudio.com/vs/older-downloads/) lehet. Ehhez a tanfolyamhoz telepíteni az alapértelmezett beállításokkal elég. Mivel már létezik Visual Studio 2017, így ezt a 2015-ös változatot egy kicsit macerás letölteni (be kell lépni, ha nincs Microsoft belépésünk, akkor regisztrálni kell), ezért én azt tanácsolom, hogy inkább telepítsük  [Chocolatey](https://chocolatey.org/) csomagkezelővel.

#### Telepítés: ehhez
1. indítsunk egy adminisztrátori parancssort ([elevated command prompt](http://www.computerhope.com/jargon/e/elevated.htm)),

2. tegyük vágólapra ezt (igen, az egészet!):

**@powershell -NoProfile -ExecutionPolicy Bypass -Command "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"**

3. majd a parancssorunkba illesszük be és futtassuk le.

Ezzel telepítettünk egy csomagkezelőt, innentől kezdve nem kell letölteni és telepíteni kattintgatásokkal az alkalmazásainkat, hanem a csomagkezelőnkre bizhatjuk a dolgot, legalábbis [jelenleg már több, mint 5000 alkalmazás esetében](https://chocolatey.org/packages).

Mivel immáron már van csomagkezelőnk, a Visual Studio Community telepítése [adminisztrátori parancssorból így megy](https://chocolatey.org/packages/VisualStudio2015Community):

4. **cinst visualstudio2015community**

Ezzel meg is vagyunk az előkészületekkel, a többit a tanfolyamon folytatjuk!
