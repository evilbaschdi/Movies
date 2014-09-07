using MahApps.Metro.Controls;
using System.Collections.Generic;
using TMDbLib.Objects.Collections;

namespace Movie
{
    /// <summary>
    /// Interaction logic for ChooseMatchingMoviePart.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class ChooseMatchingMoviePart : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        public ChooseMatchingMoviePart()
        {
            InitializeComponent();
            GetMovieParts();
        }

        public static List<Part> MovieParts { get; set; }

        private void GetMovieParts()
        {
            foreach (var moviePart in MovieParts)
            {
                MoviePartsList.Items.Add(moviePart.Title);
            }
        }
    }
}