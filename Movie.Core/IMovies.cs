using EvilBaschdi.Core;
using Movie.Core.Models;

namespace Movie.Core;

/// <summary>
/// </summary>
public interface IMovies : IValueOfList<MovieRecord>
{
    /// <summary>
    /// </summary>
    void Create(IMovieRecord movieRecord);

    /// <summary>
    /// </summary>
    void Update(IMovieRecord movieRecord);

    /// <summary>
    /// </summary>
    void Delete(string id);

    /// <summary>
    /// </summary>
    IMovieRecord ValueById(string id);

    /// <summary>
    /// </summary>
    IMovieRecord ValueByName(string name);
}