using System.Collections;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <summary>
    /// </summary>
    public interface IMovies
    {
        /// <summary>
        /// </summary>
        void Insert(IMovieRecord movieRecord);

        /// <summary>
        /// </summary>
        void Update(IMovieRecord movieRecord);

        /// <summary>
        /// </summary>
        void Delete(string id);

        /// <summary>
        /// </summary>
        IList MovieDataView();

        /// <summary>
        /// </summary>
        IList MovieDataView(string filter, string category);

        /// <summary>
        /// </summary>
        IMovieRecord GetMovieById(string id);

        /// <summary>
        /// </summary>
        IMovieRecord GetMovieByName(string name);
    }
}