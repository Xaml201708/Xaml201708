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

namespace XamlGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int huzasokSzama = 0;
        FontAwesomeIcon elozoKartya = FontAwesomeIcon.None;
        //Dobókocka
        Random dobokocka = new Random();
        //kell egy hatlapos kártyacsomag, létrehozzuk a hat tárhelyet (polcot) ahova kártyákat tudok tenni
        //tehát létrehozom a kártyatartómat, ahol az egyes lapok számmal lesznek megjelölve
        FontAwesomeIcon[] kartyak = new FontAwesome.WPF.FontAwesomeIcon[6];

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

        }

        private void ShowNewCardButton_Click(object sender, RoutedEventArgs e)
        {
            UjKartyaHuzasa();
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
                NoButton.IsEnabled = true;
                YesButton.IsEnabled = true;

                //Ezt a későbbiekben visszatesszük
                //PartiallyButton.IsEnabled = true;

                //Innentől kezdve csak az igen és a nem gomb kell, hogy éljen
                //Az új kártyakérő gombot letiltjuk
                ShowNewCardButton.IsEnabled = false;
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

        private void AValaszHelyes()
        {
            CardPlaceLeft.Foreground = Brushes.Green;
            CardPlaceLeft.Icon = FontAwesomeIcon.Check;
            AvalaszLassuEltuntetese();
        }

        private void AValaszHelytelen()
        {
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
    }
}
