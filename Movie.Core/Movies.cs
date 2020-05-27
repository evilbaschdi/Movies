using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <inheritdoc />
    public class Movies : IMovies
    {
        private readonly ITransformDataRowToMovieRecord _transformDataRowToMovieRecord;
        private readonly IXmlDatabase _xmlDatabase;


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="xmlDatabase"></param>
        /// <param name="transformDataRowToMovieRecord"></param>
        public Movies(IXmlDatabase xmlDatabase, [NotNull] ITransformDataRowToMovieRecord transformDataRowToMovieRecord)
        {
            _xmlDatabase = xmlDatabase ?? throw new ArgumentNullException(nameof(xmlDatabase));
            _transformDataRowToMovieRecord = transformDataRowToMovieRecord ?? throw new ArgumentNullException(nameof(transformDataRowToMovieRecord));
        }

        /// <inheritdoc />
        public IMovieRecord ValueById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var dataRow = _xmlDatabase.ValueForId(id);
            return dataRow != null ? _transformDataRowToMovieRecord.ValueFor(dataRow) : null;
        }

        /// <inheritdoc />
        public IMovieRecord ValueByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var dataRow = _xmlDatabase.ValueForName(name);
            return dataRow != null ? _transformDataRowToMovieRecord.ValueFor(dataRow) : null;
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
        public void Create(IMovieRecord movieRecord)
        {
            if (movieRecord == null)
            {
                throw new ArgumentNullException(nameof(movieRecord));
            }

            _xmlDatabase.Create(movieRecord);
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

        /// <inheritdoc />
        public List<MovieRecord> Value => (from DataRowView dataRowView in _xmlDatabase.Value select _transformDataRowToMovieRecord.ValueFor(dataRowView.Row)).ToList();
    }

    /// <inheritdoc />
    public interface ITransformDataRowToMovieRecord : IValueFor<DataRow, MovieRecord>
    {
    }

    /// <inheritdoc />
    public class TransformDataRowToMovieRecord : ITransformDataRowToMovieRecord
    {
        /// <inheritdoc />
        public MovieRecord ValueFor([NotNull] DataRow dataRow)
        {
            if (dataRow == null)
            {
                throw new ArgumentNullException(nameof(dataRow));
            }

            return new MovieRecord
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
    }
}