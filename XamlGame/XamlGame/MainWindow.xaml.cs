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

namespace XamlGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowNewCard_Click(object sender, RoutedEventArgs e)
        {
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
            CardPlaceRight.Icon = kartyak[dobas];

        }
    }
}
