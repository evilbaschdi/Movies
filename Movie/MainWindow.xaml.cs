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
        private string _action;
        private string _currentId;
        private string _exception;
        private MovieRecord _movie;
        private bool _result;

        public MainWindow()
        {
            InitializeComponent();
            Populate();
        }

        private void Populate()
        {
            //MovieGrid.ItemsSource = List.MovieList();
            //Sorting();
        }

        private void Sorting()
        {
            //create a collection view for the datasoruce binded with grid

            ICollectionView dataView = CollectionViewSource.GetDefaultView(MovieGrid.ItemsSource);
            //clear the existing sort order
            dataView.SortDescriptions.Clear();
            //create a new sort order for the sorting that is done lastly
            dataView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            //refresh the view which in turn refresh the grid
            dataView.Refresh();
        }

        private void LoadData(string id)
        {
            _movie = List.GetMovie(id);
            if (_movie != null)
            {
                _currentId = _movie.Id;
                txtName.Text = _movie.Name;
                txtYear.Text = _movie.Year;
                txtFormat.Text = _movie.Format;
            }
        }

        private void MovieGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovieGrid.SelectedItem == null) return;
            var dataRowView = (DataRowView) MovieGrid.SelectedItem;

            string id = dataRowView.Row["Id"].ToString();

            LoadData(id);
        }

        private void IsAlreadyExist()
        {
            _movie = List.GetMovie(_currentId);
            _action = _movie != null ? "update" : "insert";
        }

        private bool IsValidate()
        {
            return txtName.Text != string.Empty;
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtYear.Text = "";
            txtFormat.Text = "";
        }

        public void CancelData()
        {
            ClearForm();
        }

        public void CloseForm()
        {
            Close();
        }

        public void DeleteData()
        {
            try
            {
                MessageBoxResult rslt =
                    MessageBox.Show(string.Format("Are sure want to delete this record movie: {0} ?", txtName.Text),
                        "[Confirmation]", MessageBoxButton.YesNo);
                if (rslt == MessageBoxResult.Yes)
                {
                    List.Delete(_currentId);
                    _result = true;
                }
            }
            catch (Exception exp)
            {
                _result = false;
                _exception = string.Format("Record {0} failed delete to datasource\n Message : {1}",
                    txtName.Text.Trim(), exp.Message);
                MessageBox.Show(_exception, "[Status Dialog]", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            Populate();
            ClearForm();
        }

        public void NewEntry()
        {
            ClearForm();
            _movie = null;
            txtName.Focus();
        }

        public void PrintReport()
        {
            //var s = new ExportToExcel<MovieRecord>();
            //ICollectionView view = CollectionViewSource.GetDefaultView(MovieGrid.ItemsSource);
            //s.DataToPrint = view.SourceCollection as List<MovieRecord>;
            //s.GenerateReport();
        }

        public void RefreshData()
        {
            ClearForm();
            Populate();
        }

        public void SaveOrUpdateData()
        {
            if (!IsValidate()) return;
            IsAlreadyExist();
            SaveOrUpdateAction();
            MessageBox.Show(_exception, "[Modified Dialog]", MessageBoxButton.OK,
                !_result ? MessageBoxImage.Exclamation : MessageBoxImage.Information);
        }

        public void SaveOrUpdateAction()
        {
            _movie = new MovieRecord
            {
                Id = _currentId,
                Name = txtName.Text,
                Year = txtYear.Text,
                Format = txtFormat.Text
            };
            if (_action.Equals("insert"))
            {
                try
                {
                    List.Insert(_movie);
                    _result = true;
                    _exception = string.Format("'{0}' successfully insert to datasource", txtName.Text);
                }
                catch (Exception exp)
                {
                    _result = false;
                    _exception = string.Format("'{0}' failed insert to datasource\n Message : {1}",
                        _movie.Id.Trim(), exp.Message);
                }
            }
            else
            {
                try
                {
                    List.Update(_movie);
                    _result = true;
                    _exception = string.Format("'{0}' successfully update to datasource", txtName.Text);
                }
                catch (Exception exp)
                {
                    _result = false;
                    _exception = string.Format("'{0}' failed update to datasource\n Message : {1}",
                        txtName.Text, exp.Message);
                }
            }
            Populate();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //AddEditMovie.connstring = p_connstring();

            new AddEditMovie().Show();
            //NewEntry();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveOrUpdateData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnExport_OnClick(object sender, RoutedEventArgs e)
        {
            PrintReport();
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var propertyDescriptor = (PropertyDescriptor) e.PropertyDescriptor;
            e.Column.Header = propertyDescriptor.DisplayName;
            if (propertyDescriptor.DisplayName == "Id")
            {
                e.Cancel = true;
            }
        }
    }
}