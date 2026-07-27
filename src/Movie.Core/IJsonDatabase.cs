using Movie.Core.Models;

namespace Movie.Core;

/// <summary>
/// </summary>
public interface IJsonDatabase
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
    IMovieRecord ValueForId(string id);

    /// <summary>
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    IMovieRecord ValueForName(string name);
    
    /// <summary>
    /// </summary>
    /// <returns></returns>
    List<IMovieRecord> GetValue();
}