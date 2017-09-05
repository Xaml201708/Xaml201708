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

namespace XamlGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int huzasokSzama = 0;
        FontAwesomeIcon elozoKartya = FontAwesomeIcon.None;

        public MainWindow()
        {
            InitializeComponent();
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

            //kell egy hatlapos kártyacsomag
            var kartyak = new FontAwesome.WPF.FontAwesomeIcon[6];
            kartyak[0] = FontAwesome.WPF.FontAwesomeIcon.Car;
            kartyak[1] = FontAwesome.WPF.FontAwesomeIcon.SnowflakeOutline;
            kartyak[2] = FontAwesome.WPF.FontAwesomeIcon.Briefcase;
            kartyak[3] = FontAwesome.WPF.FontAwesomeIcon.Book;
            kartyak[4] = FontAwesome.WPF.FontAwesomeIcon.Male;
            kartyak[5] = FontAwesome.WPF.FontAwesomeIcon.Female;

            //dobunk dobókockával
            var dobokocka = new Random();
            var dobas = dobokocka.Next(0, 5);

            //System.Diagnostics.Debug.WriteLine(dobas);

            //amelyik kártyát kijelöli a kocka, megjelenítjük a jobboldali kártyahelyen.

            elozoKartya = CardPlaceRight.Icon;

            CardPlaceRight.Icon = kartyak[dobas];

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
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
        }

        private void AValaszHelytelen()
        {
            CardPlaceLeft.Foreground = Brushes.Red;
            CardPlaceLeft.Icon = FontAwesomeIcon.Times;
        }

    }
}
