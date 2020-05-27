using System.Data;
using EvilBaschdi.Core;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <summary>
    /// </summary>
    public interface IXmlDatabase : IValue<DataView>
    {
        /// <summary>
        ///     Inserts a record in the movie table.
        /// </summary>
        void Create(IMovieRecord movieRecord);

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
        DataRow ValueForId(string id);

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        DataRow ValueForName(string name);
    }
}