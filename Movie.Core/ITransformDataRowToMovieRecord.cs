using System.Data;
using EvilBaschdi.Core;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <inheritdoc />
    public interface ITransformDataRowToMovieRecord : IValueFor<DataRow, MovieRecord>
    {
    }
}