using System;
using System.Collections;

namespace Movie.Core
{
    public class Movies : IMovies
    {
        private IMovieRecord _movieRecord;
        private readonly IXmlDatabase _xmlDatabase;

        /// <summary>
        ///     Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.
        /// </summary>
        public Movies()
        {
            _xmlDatabase = new XmlDatabase();
        }

        public IMovieRecord GetMovieById(string id)
        {
            _movieRecord = null;

            var dataRow = _xmlDatabase.SelectById(id);
            if(dataRow != null)
            {
                _movieRecord = new MovieRecord
                {
                    Id = dataRow[0] != DBNull.Value ? dataRow[0].ToString() : string.Empty,
                    Name = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty,
                    Year = dataRow[2] != DBNull.Value ? dataRow[2].ToString() : string.Empty,
                    Format = dataRow[3] != DBNull.Value ? dataRow[3].ToString() : string.Empty,
                    Distributed = dataRow[4] != DBNull.Value ? dataRow[3].ToString() : string.Empty
                };
            }
            return _movieRecord;
        }

        public IMovieRecord GetMovieByName(string name)
        {
            _movieRecord = null;

            var dataRow = _xmlDatabase.SelectByName(name);
            if(dataRow != null)
            {
                _movieRecord = new MovieRecord
                {
                    Id = dataRow[0] != DBNull.Value ? dataRow[0].ToString() : string.Empty,
                    Name = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty,
                    Year = dataRow[2] != DBNull.Value ? dataRow[2].ToString() : string.Empty,
                    Format = dataRow[3] != DBNull.Value ? dataRow[3].ToString() : string.Empty,
                    Distributed = dataRow[4] != DBNull.Value ? dataRow[3].ToString() : string.Empty
                };
            }
            return _movieRecord;
        }

        public IList MovieDataView()
        {
            var dataView = _xmlDatabase.SelectAll();
            return dataView;
        }

        public IList MovieDataView(string filter, string category)
        {
            var dataView = _xmlDatabase.SelectFiltered(filter, category);
            return dataView;
        }

        public void Update(IMovieRecord movieRecord)
        {
            _xmlDatabase.Update(movieRecord.Id, movieRecord.Name, movieRecord.Year, movieRecord.Format,
                movieRecord.Distributed);
        }

        public void Insert(IMovieRecord movieRecord)
        {
            _xmlDatabase.Insert(movieRecord.Name, movieRecord.Year, movieRecord.Format,
                movieRecord.Distributed);
        }

        public void Delete(string id)
        {
            _xmlDatabase.Delete(id);
        }
    }
}