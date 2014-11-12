using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Movie.TMDbAPI;
using TMDbLib.Objects.Collections;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for ChooseMatchingMoviePart.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class ChooseMatchingMoviePart : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        public ChooseMatchingMoviePart()
        {
            InitializeComponent();
            SetMoviePartListItems();
        }

        private static List<Part> MovieParts { get; set; }

        private static TmDb TmDb { get; set; }

        private void SetMoviePartListItems()
        {
            foreach(var alternativeTitle in GetAlternativeTitles())
            {
                MoviePartsList.Items.Add(alternativeTitle.Value);
            }
        }

        private IEnumerable<KeyValuePair<int, string>> GetAlternativeTitles()
        {
            return (from moviePart in MovieParts
                let alternativeTitles = TmDb.GetGermanTitleById(moviePart.Id)
                let firstOrDefault = alternativeTitles.Titles.FirstOrDefault()
                where firstOrDefault != null
                select new KeyValuePair<int, string>(moviePart.Id, firstOrDefault.Title)).ToList();
        }

        private void MoviePartsList_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(MoviePartsList.SelectedItem != null)
            {
                foreach(
                    var alternativeTitle in
                        GetAlternativeTitles()
                            .Where(alternativeTitle => MoviePartsList.SelectedItem.ToString() == alternativeTitle.Value)
                    )
                {
                    MessageBox.Show(alternativeTitle.Key.ToString("D"));
                }
            }
        }
    }
}