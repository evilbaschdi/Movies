using System.Data;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <summary>
    /// </summary>
    public interface IXmlDatabase
    {
        /// <summary>
        ///     Inserts a record in the movie table.
        /// </summary>
        void Insert(IMovieRecord movieRecord);

        /// <summary>
        ///     Updates a record in the movie table.
        /// </summary>
        void Update(IMovieRecord movieRecord);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        void Delete(string id);

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataRow SelectById(string id);

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DataRow SelectByName(string name);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        DataView SelectAll();

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        DataView SelectFiltered(string filter, string category);
    }
}