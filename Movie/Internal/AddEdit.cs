using System;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;

namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public class AddEdit : IAddEdit
    {
        private readonly MainWindow _mainWindow;
        private IMovieRecord _movieRecord;
        private readonly IMovies _movies;

        /// <summary>
        /// </summary>
        public string Mode { private get; set; }

        /// <summary>
        /// </summary>
        public string CurrentId { private get; set; }

        private string _name;
        private double? _year;
        private string _format;
        private string _action;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        /// <param name="mainWindow"></param>
        public AddEdit(MainWindow mainWindow)
        {
            if (mainWindow == null)
            {
                throw new ArgumentNullException(nameof(mainWindow));
            }
            _mainWindow = mainWindow;
            _movies = new Movies();
        }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="year"></param>
        /// <param name="format"></param>
        public void MovieData(string name, double? year, string format)
        {
            _name = name;
            _year = year;
            _format = format;
        }

        /// <summary>
        /// </summary>
        /// <param name="addNew"></param>
        public async void SaveAndAddNew(bool addNew)
        {
            if (IsDuplicate())
            {
                var options = new MetroDialogSettings
                              {
                                  ColorScheme = MetroDialogColorScheme.Theme
                              };

                _mainWindow.MetroDialogOptions = options;
                await _mainWindow.ShowMessageAsync("Already existing!", $"'{_name}'");
            }
            else
            {
                SaveOrUpdateData();

                if (addNew)
                {
                    _mainWindow.NewEntry();
                }
                else
                {
                    _mainWindow.CleanupAndClose();
                }
            }
        }

        private void SaveOrUpdateData()
        {
            if (!IsValid())
            {
                return;
            }
            InsertOrUpdate();
            InsertOrUpdateAction();
        }

        private void InsertOrUpdateAction()
        {
            _movieRecord = new MovieRecord
                           {
                               Id = CurrentId,
                               Name = _name,
                               Year = _year.ToString(),
                               Format = _format,
                               Distributed = "False",
                               DistributedTo = ""
                           };

            try
            {
                switch (_action)
                {
                    case "insert":
                        _movies.Insert(_movieRecord);
                        break;

                    case "update":
                        _movies.Update(_movieRecord);
                        break;
                }
            }
            catch (Exception exception)
            {
                var e = $"'{_name}' failed to {_action} database\n Message : {exception.Message}";
                _mainWindow.ShowErrorMessage(e);
            }

            _mainWindow.Populate();
        }

        private void InsertOrUpdate()
        {
            _movieRecord = _movies.GetMovieById(CurrentId);
            _action = _movieRecord != null ? "update" : "insert";
        }

        private bool IsDuplicate()
        {
            return _movies.GetMovieByName(_name) != null && Mode == "add";
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_name);
        }
    }
}