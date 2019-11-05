using System;
using System.Collections;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <inheritdoc />
    public class Movies : IMovies
    {
        private readonly IXmlDatabase _xmlDatabase;
        private IMovieRecord _movieRecord;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="xmlDatabase"></param>
        public Movies(IXmlDatabase xmlDatabase)
        {
            _xmlDatabase = xmlDatabase ?? throw new ArgumentNullException(nameof(xmlDatabase));
        }

        /// <inheritdoc />
        public IMovieRecord GetMovieById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

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

        /// <inheritdoc />
        public IMovieRecord GetMovieByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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

        /// <inheritdoc />
        public IList MovieDataView()
        {
            var dataView = _xmlDatabase.SelectAll();
            return dataView;
        }

        /// <inheritdoc />
        public IList MovieDataView(string filter, string category)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            var dataView = _xmlDatabase.SelectFiltered(filter, category);
            return dataView;
        }

        /// <inheritdoc />
        public void Update(IMovieRecord movieRecord)
        {
            if (movieRecord == null)
            {
                throw new ArgumentNullException(nameof(movieRecord));
            }

            _xmlDatabase.Update(movieRecord);
        }

        /// <inheritdoc />
        public void Insert(IMovieRecord movieRecord)
        {
            if (movieRecord == null)
            {
                throw new ArgumentNullException(nameof(movieRecord));
            }

            _xmlDatabase.Insert(movieRecord);
        }

        /// <inheritdoc />
        public void Delete(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _xmlDatabase.Delete(id);
        }
    }
}