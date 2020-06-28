using System;
using MahApps.Metro.Controls.Dialogs;
using Movie.Core;
using Movie.Core.Models;

namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public class AddEdit : IAddEdit
    {
        private readonly MainWindow _mainWindow;
        private readonly IMovies _movies;
        private IMovieRecord _movieRecord;


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="movies"></param>
        public AddEdit(MainWindow mainWindow, IMovies movies)
        {
            _mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            _movies = movies ?? throw new ArgumentNullException(nameof(movies));
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Mode { private get; set; }


        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="year"></param>
        /// <param name="format"></param>
        /// <param name="id"></param>
        public void MovieData(string name, double? year, string format, string id)
        {
            _movieRecord = new MovieRecord
                           {
                               Id = id,
                               Name = name,
                               Year = year.ToString(),
                               Format = format
                           };
        }

        /// <inheritdoc />
        public async void Save()
        {
            if (IsDuplicate())
            {
                await _mainWindow.ShowMessageAsync("Already existing!", $"'{_movieRecord.Name}'");
            }
            else
            {
                if (!IsValid())
                {
                    return;
                }

                InsertOrUpdateAction();
            }
        }

        private void InsertOrUpdateAction()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_movieRecord.Id))
                {
                    _movies.Create(_movieRecord);
                }
                else
                {
                    _movies.Update(_movieRecord);
                }
            }
            catch (Exception exception)
            {
                var e = $"'Create or update {_movieRecord.Name}' failed\n Message : {exception.Message}";

                _mainWindow.ShowMessageAsync("Error", e);
            }

            //_mainWindow.Load();
        }

        private bool IsDuplicate()
        {
            return _movies.ValueByName(_movieRecord.Name) != null && Mode == "add";
        }

        private bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_movieRecord.Name);
        }
    }
}