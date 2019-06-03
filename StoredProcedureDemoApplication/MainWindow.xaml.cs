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
using StoredProcedureDemoApplication.Classes;

namespace StoredProcedureDemoApplication
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

        private void ImportSingles_Click(object sender, RoutedEventArgs e)
        {

            var moviesToImport = ImportLibrary.GetMoviesFromCSV();

            var startTime = DateTime.Now;
            var moviesImported = ImportLibrary.WriteSingleRecords(moviesToImport,System.Convert.ToInt32(NumberToImport.Text));
            var endTime = DateTime.Now;

            MessageBox.Show($"{moviesImported} imported in {(endTime - startTime).TotalSeconds}");

        }

        private void ImportMultiples_Click(object sender, RoutedEventArgs e)
        {
            var moviesToImport = ImportLibrary.GetMoviesFromCSV();

            var startTime = DateTime.Now;
            var moviesImported = ImportLibrary.WriteBulkRecords(moviesToImport, System.Convert.ToInt32(NumberToImport.Text));
            var endTime = DateTime.Now;

            MessageBox.Show($"{moviesImported} imported in {(endTime - startTime).TotalSeconds}");
        }

        private void QueryByYear_Click(object sender, RoutedEventArgs e)
        {
            var movies = ImportLibrary.GetMoviesByYears(YearsToImport.Text);
            MovieGrid.ItemsSource = movies;
            //MessageBox.Show($"{movies.Count} Retrieved");
        }

        private void QueryByYearAndName_Click(object sender, RoutedEventArgs e)
        {
            var movies = ImportLibrary.GetMoviesByYearsAndName(TitleContains.Text,YearsToImport.Text);
            MovieGrid.ItemsSource = movies;
            //MessageBox.Show($"{movies.Count} Retrieved");
        }
    }
}
