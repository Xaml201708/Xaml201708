using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FontAwesome.WPF;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace XamlGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FontAwesomeIcon elozoKartya = FontAwesomeIcon.None;
        //Dobókocka
        Random dobokocka = new Random();
        //kell egy hatlapos kártyacsomag, létrehozzuk a hat tárhelyet (polcot) ahova kártyákat tudok tenni
        //tehát létrehozom a kártyatartómat, ahol az egyes lapok számmal lesznek megjelölve
        FontAwesomeIcon[] kartyak = new FontAwesome.WPF.FontAwesomeIcon[6];

        TimeSpan visszalevoIdo;
        DispatcherTimer ingaora;

        //stopperóra a reakcióidő méréséhez
        Stopwatch stopper = new Stopwatch();

        int huzasokSzama = 0;
        //Az összes reakciót eltároljuk, hogy átlagot tudjunk számítani
        List<long> osszesReakcio = new List<long>();
        long pontszam = 0;

        //top lista
        List<long> topList = new List<long>();

        public MainWindow()
        { //Ez a függvény akkor fut le (hajtódik végre) amikor megjelenik először a MainWindow nevű ablak
            InitializeComponent();

            //A kártyatartót feltöltöm kártyalapokkal
            kartyak[0] = FontAwesome.WPF.FontAwesomeIcon.Car;
            kartyak[1] = FontAwesome.WPF.FontAwesomeIcon.SnowflakeOutline;
            kartyak[2] = FontAwesome.WPF.FontAwesomeIcon.Briefcase;
            kartyak[3] = FontAwesome.WPF.FontAwesomeIcon.Book;
            kartyak[4] = FontAwesome.WPF.FontAwesomeIcon.Male;
            kartyak[5] = FontAwesome.WPF.FontAwesomeIcon.Female;

            //Felparaméterezzük az ingaóránkat:
            //másodpercenként adjon egy eseményt
            ingaora = new DispatcherTimer(
                TimeSpan.FromSeconds(1)         //egy másodpercenként kérem az eseményt
                ,DispatcherPriority.Normal      //semmi különleges, semmi fontos, néhány századmásodperc nem számít
                ,IngaoraUt                      //ezt hívjuk minden alkalommal
                ,Application.Current.Dispatcher //ennek segítségével tudunk a felületre adatot küldeni
                );
            //mivel azonnal elindul, megállítom, és csak a játék kezdetekor
            //indítjuk el
            ingaora.Stop();

            JatekFelkeszules();

        }

        /// <summary>
        /// Ezt a függvényt hívja az ingaóra minden alkalommal, amikor üt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IngaoraUt(object sender, EventArgs e)
        {
            //egy másodperccel csökkentem a hátralévő időt
            visszalevoIdo = visszalevoIdo.Add(TimeSpan.FromSeconds(-1));
            //szöveg a belsejében cserével
            HatralevoIdoFrissitese();
            //CountdownLabel.Content = $"Visszaszámolás: {visszalevoIdo.Minutes}:{visszalevoIdo.Seconds}"; //ez nem elég jó, levágja a nullákat
            //CountdownLabel.Content = $"Visszaszámolás: {visszalevoIdo.Minutes:00}:{visszalevoIdo.Seconds:00}"; //ez így már jó, kiegészíti a számokat két 0-val
            if (visszalevoIdo == TimeSpan.Zero)
            {
                JatekVege();
            }
        }

        private void HatralevoIdoFrissitese()
        {
            CountdownLabel.Content = $"Visszaszámolás: {visszalevoIdo.ToString("mm\\:ss")}";
        }

        /// <summary>
        /// Ezzel előkészítjük a játék változóit és
        /// húzunk egy kártyát
        /// </summary>
        private void JatekFelkeszules()
        {
            //új játékhoz új kezdet
            huzasokSzama = 0;
            osszesReakcio = new List<long>();
            pontszam = 0;
            visszalevoIdo = TimeSpan.FromSeconds(3);

            UjKartyaHuzasa();

            //újrakezdés gomb eltűnik
            RestartGameButton.Visibility = Visibility.Hidden;
            //kezdés gomb megjelenik
            StartGameButton.Visibility = Visibility.Visible;

            //az eredményjelző frissítése
            HatralevoIdoFrissitese();
            ReakcioIdoFrissitese(0, 0);
            PontszamFrissitese();
        }

        private void JatekKezdete()
        {
            ingaora.Start();
        }

        private void JatekVege()
        {
            ingaora.Stop();
            //újrakezdés gomb megjelenik
            RestartGameButton.Visibility = Visibility.Visible;
            
            //todo: a toplista méretét változóban szabályozni
            if (topList.Count < 5 //ha még nincs 5 elem a listán
                ||  pontszam>=topList.Min()) //vagy ráférünk a top 5 listára
            { //a toplista frissítése
                topList.Add(pontszam);

                if (topList.Count>5)
                { //a fölösleget törölni kell
                    topList.Sort(); //sorbarendezés a kisebbtől a nagyobbig
                    topList.RemoveAt(0); //mivel a sorbarendezés a legkisebbtől a legnagyobbig megy, a törlendő a tetején lesz
                }
                ToplistaMegjelenitese();
            }

            //vége a játéknak, gombokat tiltani
            NoButton.IsEnabled = false;
            YesButton.IsEnabled = false;
            //billenytűket nem figyelni
            huzasokSzama = 0;
        }

        private void ToplistaMegjelenitese()
        {
            //ObservableCollection a ListBox-szal remekül együttműködik, és a List-ből pedig létre tudjuk hozni
            TopListBox.ItemsSource = new ObservableCollection<long>(topList.OrderByDescending(x=>x));
        }

        /// <summary>
        /// Egy kocka dobása és új kártya húzása a dobás alapján
        /// </summary>
        private void UjKartyaHuzasa()
        {
            huzasokSzama++;

            //ha (a húzások száma egyenlő kettővel)
            if (huzasokSzama == 2)
            { //akkor engedélyezzük a gombokat
                //todo: ez a játék kezdete, ki is lehetne szervezni
                NoButton.IsEnabled = true;
                YesButton.IsEnabled = true;

                //Ezt a későbbiekben visszatesszük
                //PartiallyButton.IsEnabled = true;

                //Innentől kezdve csak az igen és a nem gomb kell, hogy éljen
                //Az új kártyakérő gombot letiltjuk
                //StartGameButton.IsEnabled = false;
                //nem letiltjuk, hanem eltüntetjük
                StartGameButton.Visibility = Visibility.Hidden;

                JatekKezdete();
            }

            //dobunk dobókockával
            var dobas = dobokocka.Next(0, 5);

            //System.Diagnostics.Debug.WriteLine(dobas);

            //elmentem aktuális kártyát, ami a húzás után az előző kártya lesz
            //az erre a célra létrehozott változóba
            elozoKartya = CardPlaceRight.Icon;

            //eltüntetjük az előző kártyát
            var animationOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(100));
            CardPlaceRight.BeginAnimation(OpacityProperty, animationOut);

            //amelyik kártyát kijelöli a kocka, megjelenítjük a jobboldali kártyahelyen.
            CardPlaceRight.Icon = kartyak[dobas];

            //nem foglalkozom esetszétválasztással, mindig újraindítom
            stopper.Restart();

            //megjelenítjük az új kártyát
            var animationIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(100));
            CardPlaceRight.BeginAnimation(OpacityProperty, animationIn);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Debug.WriteLine(e.Key);
            if (huzasokSzama<2)
            { // még nincs két kártya, a gombok nem élnek
                return; //ebben az esetben a függvény végrehajtása nem folytatódik, hanem visszatér a hívóhoz
            }

            if (e.Key == Key.Left)
            { //balranyilat nyomtunk
                NoAnswer();
            }

            if (e.Key == Key.Right)
            { //jobbranyilat nyomtunk
                YesAnswer();
            }

            if (e.Key == Key.Down)
            { }

        }

        private void PartiallyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            YesAnswer();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            NoAnswer();
        }

        private void YesAnswer()
        {
            if (elozoKartya == CardPlaceRight.Icon)
            { //valóban egyezik, a két kártya azonos
                AValaszHelyes();
            }
            else
            { //nem egyezik
                AValaszHelytelen();
            }

            UjKartyaHuzasa();
        }

        private void NoAnswer()
        {
            if (elozoKartya == CardPlaceRight.Icon)
            { //Egyezik, a válasz helytelen
                AValaszHelytelen();
            }
            else
            { //nem egyzik, a válasz helyes
                AValaszHelyes();
            }

            UjKartyaHuzasa();
        }

        /// <summary>
        /// reakcióidő mérése
        /// átlagos reakcióidő számítása
        /// pont számítás
        /// eredmények megjelenítése
        /// </summary>
        private void ReakcioidoEsPontSzamitas(bool helyesValasz)
        {
            stopper.Stop();
            var utolsoReakcioIdo = stopper.ElapsedMilliseconds;

            //Az utolsó reakcióidőt elmentjük a listánkba
            osszesReakcio.Add(utolsoReakcioIdo);

            //atlagosReakcioIdo = ?

            //------------------------------------------------------------------------------------
            // Ismétlődő műveletek kezelése: ciklusok
            //------------------------------------------------------------------------------------

            //var reakciokOsszeg = osszesReakcio[0] + osszesReakcio[1] +...+ osszesReakcio[utolso];

            //Több sor megjegyzésbe tétele: Ctrl+K aztán Ctrl+C

            //var reakciokOsszeg = 0;
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[0];
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[1];
            //...
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[utolso];

            //var reakciokOsszeg = 0;
            //var hanyadikElem = 0
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[hanyadikElem];
            //hanyadikElem = hanyadikElem + 1;
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[hanyadikElem];
            //hanyadikElem = hanyadikElem + 1;

            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[hanyadikElem];
            //hanyadikElem = hanyadikElem + 1;


            //var reakciokOsszeg = 0;
            //var hanyadikElem = 0
            //reakciokOsszeg = reakciokOsszeg + osszesReakcio[hanyadikElem];
            //hanyadikElem = hanyadikElem + 1;
            //ha van még elem akkor visszaugrani két sort és tovább számolni különben folytatni a programot

            long reakciokOsszege = 0;

            //Az i értéke 0-val kezdve egészen addig, amíg igaz az, hogy i< length
            //ez hajtódik végre: i++ (Az i értéke eggyel nő)
            //és minden ilyen lépés után ami a kódblokkban van, az egyszer végrehajtódik.
            //for (int i = 0; i < length; i++) 
            //{
            //}

            for (int hanyadikElem = 0; hanyadikElem < osszesReakcio.Count; hanyadikElem++)
            {
                reakciokOsszege = reakciokOsszege + osszesReakcio[hanyadikElem];
            }

            var reakciokAtlaga = reakciokOsszege / osszesReakcio.Count;

            //Következő ciklus: egy emberközelibb megoldás
            reakciokOsszege = 0;

            foreach (var reakcioIdo in osszesReakcio)
            {
                reakciokOsszege = reakciokOsszege + reakcioIdo;
            }

            var reakciokAtlaga2 = reakciokOsszege / osszesReakcio.Count;

            //ciklus nélkül: LINQ
            //var reakciokAtlaga3 = osszesReakcio.Average();
            //magától az átlag számítás törtszámot ad vissza, ezt nekünk kell ilyenkor egésszé alakítani, például így
            //ennek neve: típuskonverzió
            var reakciokAtlaga3 = (long)osszesReakcio.Average();

            //amikor nem tudjuk előre, hogy mennyiszer kell végrehajtani
            //például egy állományt ha végig akarok olvasni, és nem ismerem a méretét, 
            //akkor addig ismétlem az olvasást, amíg az állomány tartalma el nem fogy
            //while (true)
            //{

            //}
            reakciokOsszege = 0;
            var hanyadikElem2 = 0;
            while (hanyadikElem2 < osszesReakcio.Count)
            {
                reakciokOsszege = reakciokOsszege + osszesReakcio[hanyadikElem2];
                hanyadikElem2 = hanyadikElem2 + 1;
            }
            var reakciokAtlaga4 = reakciokOsszege / osszesReakcio.Count;

            ReakcioIdoFrissitese(utolsoReakcioIdo, reakciokAtlaga3);

            //Pontszámítás

            if (helyesValasz)
            {
                //minél nagyobb a reakcióidőnk, annál kisebb a szám: fordított arányosság
                pontszam = pontszam + 10000 / utolsoReakcioIdo;
            }
            else
            {   //minél nagyobb a reakcióidőnk annál nagyobb a szám: egyenes arányosság
                pontszam = pontszam - utolsoReakcioIdo / 1000;
            }

            PontszamFrissitese();

        }

        private void ReakcioIdoFrissitese(long utolsoReakcioIdo, long reakciokAtlaga3)
        {
            ReakcioLabel.Content = $"Reakció: {utolsoReakcioIdo}/{reakciokAtlaga3}";
        }

        private void PontszamFrissitese()
        {
            PontszamLabel.Content = $"Pontszám: {pontszam}";
        }

        private void AValaszHelyes()
        {
            ReakcioidoEsPontSzamitas(true);
            CardPlaceLeft.Foreground = Brushes.Green;
            CardPlaceLeft.Icon = FontAwesomeIcon.Check;
            AvalaszLassuEltuntetese();
        }

        private void AValaszHelytelen()
        {
            ReakcioidoEsPontSzamitas(false);
            CardPlaceLeft.Foreground = Brushes.Red;
            CardPlaceLeft.Icon = FontAwesomeIcon.Times;
            AvalaszLassuEltuntetese();
        }

        /// <summary>
        /// Animáció segítségével az Opacity tulajdonságát a CardPlaceLeft-nek 1-ről 0-ra állítjuk
        /// </summary>
        private void AvalaszLassuEltuntetese()
        {
            //animáció: idő elteltével egy tulajdonság értékének a változtatása
            //todo: ezt a változót kihelyezni a Window szintjére
            var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(1000));
            CardPlaceLeft.BeginAnimation(OpacityProperty, animation);
        }

        private void RestartGameButton_Click(object sender, RoutedEventArgs e)
        {
            JatekFelkeszules();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            UjKartyaHuzasa();
        }
    }
}
