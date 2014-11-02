using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MahApps.Metro.Controls;
using Movie.Core;

namespace Movie
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        private string _currentId;
        private string _exception;
        private MovieRecord _movie;

        public MainWindow()
        {
            InitializeComponent();
            Populate();
        }

        public void Populate()
        {
            MovieGrid.ItemsSource = List.MovieList();
            Sorting();
        }

        private void Sorting()
        {
            //create a collection view for the datasoruce binded with grid

            var dataView = CollectionViewSource.GetDefaultView(MovieGrid.ItemsSource);
            //clear the existing sort order
            dataView.SortDescriptions.Clear();
            //create a new sort order for the sorting that is done lastly
            dataView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //refresh the view which in turn refresh the grid
            dataView.Refresh();
        }

        private void LoadData(string id)
        {
            _movie = List.GetMovieById(id);
            if(_movie != null)
            {
                _currentId = _movie.Id;
            }
        }

        private void MovieGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MovieGrid.SelectedItem == null)
            {
                return;
            }
            var dataRowView = (DataRowView) MovieGrid.SelectedItem;

            var id = dataRowView.Row["Id"].ToString();

            LoadData(id);
        }

        public void DeleteData()
        {
            try
            {
                var delete =
                    MessageBox.Show(string.Format("Are sure want to delete this movie: {0} ?", _movie.Name),
                        "", MessageBoxButton.YesNo);
                if(delete == MessageBoxResult.Yes)
                {
                    List.Delete(_currentId);
                }
            }
            catch(Exception exp)
            {
                _exception = string.Format("Record {0} failed delete to database\n Message : {1}",
                    _movie.Name.Trim(), exp.Message);
                MessageBox.Show(_exception, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            Populate();
        }

        public void PrintReport()
        {
            //var s = new ExportToExcel<MovieRecord>();
            //ICollectionView view = CollectionViewSource.GetDefaultView(MovieGrid.ItemsSource);
            //s.DataToPrint = view.SourceCollection as List<MovieRecord>;
            //s.GenerateReport();
        }

        private void NewClick(object sender, EventArgs e)
        {
            AddEditMovie.CurrentId = null;
            new AddEditMovie().Show();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            AddEditMovie.CurrentId = _currentId;
            new AddEditMovie().Show();
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            PrintReport();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = (PropertyDescriptor) e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if(propertyDescriptor.DisplayName == "Id")
            {
                e.Cancel = true;
            }
        }
    }
}