using System;
using System.Data;
using JetBrains.Annotations;
using Movie.Core.Models;

namespace Movie.Core
{
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

            return new()
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