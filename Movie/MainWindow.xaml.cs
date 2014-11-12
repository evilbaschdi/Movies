using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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
        private readonly List _list;

        public MainWindow()
        {
            InitializeComponent();
            _list = new List();
            SearchCategory.Text = "Name";
            Populate();
        }

        private void Populate(string searchText)
        {
            MovieGrid.ItemsSource = _list.MovieList(searchText, SearchCategory.Text);
            Sorting();
        }

        public void Populate()
        {
            MovieGrid.ItemsSource = _list.MovieList();
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
            _movie = _list.GetMovieById(id);
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
            var distributed = dataRowView.Row["Distributed"].ToString();

            LoadData(id);

            DistributeMenuItem.IsEnabled = distributed != "True";
            GotBackMenuItem.IsEnabled = distributed == "True";
        }

        private void DeleteData()
        {
            try
            {
                var delete =
                    MessageBox.Show(string.Format("Are sure want to delete this movie: '{0}' ?", _movie.Name),
                        "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(delete == MessageBoxResult.Yes)
                {
                    _list.Delete(_currentId);
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

        private void NewClick(object sender, RoutedEventArgs e)
        {
            AddEditMovie.CurrentId = null;
            new AddEditMovie().Show();
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            Edit();
        }

        private void MovieGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Edit();
        }

        private void Edit()
        {
            AddEditMovie.CurrentId = _currentId;
            new AddEditMovie().Show();
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteData();
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            //PrintReport();
            //SettingFlyout.AnimateOnPositionChange = true;
            //SettingFlyout.CloseButtonVisibility = Visibility.Visible;
            //SettingFlyout.Visibility = Visibility.Visible;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = (PropertyDescriptor) e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            switch(propertyDescriptor.DisplayName)
            {
                case "Id":
                    e.Cancel = true;
                    break;

                case "Distributed":
                {
                    //var checkboxStyle = new System.Windows.Style();
                    //Style style = new Style();
                    var checkBoxColumn = new DataGridCheckBoxColumn
                    {
                        Header = e.Column.Header,
                        Binding = new Binding(e.PropertyName),
                        IsThreeState = true,
                        //ElementStyle = (System.Windows.Style) Resources["{DynamicResource MetroDataGridCheckBox}"],
                        //EditingElementStyle =
                        //    (System.Windows.Style) Resources["{DynamicResource MetroDataGridCheckBox}"]
                    };

                    // Replace the auto-generated column with the checkBoxColumn.
                    e.Column = checkBoxColumn;
                }
                    break;
            }
        }

        private void DistributeClick(object sender, RoutedEventArgs e)
        {
            _movie.Distributed = "True";
            _list.Update(_movie);
            MovieGrid.SelectedItem = null;
        }

        private void GotBackClick(object sender, RoutedEventArgs e)
        {
            _movie.Distributed = "False";
            _list.Update(_movie);
            MovieGrid.SelectedItem = null;
        }

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            Populate(SearchFilter.Text);
        }

        private void SearchResetClick(object sender, RoutedEventArgs e)
        {
            SearchFilter.Clear();
            Populate();
        }
    }
}