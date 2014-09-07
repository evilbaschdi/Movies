using MahApps.Metro.Controls;
using Movie.TMDbAPI;
using System.Linq;
using System.Windows;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for AddEditMovie.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class AddEditMovie : MetroWindow
    // ReSharper restore RedundantExtendsListEntry
    {
        private readonly TmDb _tmDb;

        public AddEditMovie()
        {
            InitializeComponent();

            _tmDb = new TmDb();
        }

        private void Name_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var searchContainer = _tmDb.GetMovieByTitle(Name.Text);

            if (searchContainer.Results.Count == 1)
            {
                var movie = searchContainer.Results.FirstOrDefault();
                if (movie != null)
                {
                    Year.Text = movie.ReleaseDate != null ? movie.ReleaseDate.Value.Year.ToString("D") : "";
                }
            }
            else
            {
                var seachCollection = _tmDb.GetMovieSearchCollection(Name.Text);

                ChooseMatchingMoviePart.MovieParts = seachCollection.Parts;
                new ChooseMatchingMoviePart().Show();
            }
        }
    }
}