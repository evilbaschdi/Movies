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
        public Movies(IXmlDatabase xmlDatabase)
        {
            _xmlDatabase = xmlDatabase ?? throw new ArgumentNullException(nameof(xmlDatabase));
        }

        public IMovieRecord GetMovieById(string id)
        {
            _movieRecord = null;

            var dataRow = _xmlDatabase.SelectById(id);
            if (dataRow != null)
            {
                _movieRecord = new MovieRecord
                               {
                                   Id = dataRow["Id"] != DBNull.Value ? dataRow["Id"].ToString() : string.Empty,
                                   Name = dataRow["Name"] != DBNull.Value ? dataRow["Name"].ToString() : string.Empty,
                                   Year = dataRow["Year"] != DBNull.Value ? dataRow["Year"].ToString() : string.Empty,
                                   Format = dataRow["Format"] != DBNull.Value ? dataRow["Format"].ToString() : string.Empty,
                                   Distributed =
                                       dataRow["Distributed"] != DBNull.Value ? dataRow["Distributed"].ToString() : string.Empty,
                                   DistributedTo =
                                       dataRow["DistributedTo"] != DBNull.Value ? dataRow["DistributedTo"].ToString() : string.Empty,
                                   Watched = dataRow["Watched"] != DBNull.Value ? dataRow["Watched"].ToString() : string.Empty
                               };
            }
            return _movieRecord;
        }

        public IMovieRecord GetMovieByName(string name)
        {
            _movieRecord = null;

            var dataRow = _xmlDatabase.SelectByName(name);
            if (dataRow != null)
            {
                _movieRecord = new MovieRecord
                               {
                                   Id = dataRow["Id"] != DBNull.Value ? dataRow["Id"].ToString() : string.Empty,
                                   Name = dataRow["Name"] != DBNull.Value ? dataRow["Name"].ToString() : string.Empty,
                                   Year = dataRow["Year"] != DBNull.Value ? dataRow["Year"].ToString() : string.Empty,
                                   Format = dataRow["Format"] != DBNull.Value ? dataRow["Format"].ToString() : string.Empty,
                                   Distributed =
                                       dataRow["Distributed"] != DBNull.Value ? dataRow["Distributed"].ToString() : string.Empty,
                                   DistributedTo =
                                       dataRow["DistributedTo"] != DBNull.Value ? dataRow["DistributedTo"].ToString() : string.Empty,
                                   Watched = dataRow["Watched"] != DBNull.Value ? dataRow["Watched"].ToString() : string.Empty
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
                movieRecord.Distributed, movieRecord.DistributedTo, movieRecord.Watched);
        }

        public void Insert(IMovieRecord movieRecord)
        {
            _xmlDatabase.Insert(movieRecord.Name, movieRecord.Year, movieRecord.Format,
                movieRecord.Distributed, movieRecord.DistributedTo, movieRecord.Watched);
        }

        public void Delete(string id)
        {
            _xmlDatabase.Delete(id);
        }
    }
}