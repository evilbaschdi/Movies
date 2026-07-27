using Movie.Core.Models;

namespace Movie.Core;

/// <inheritdoc />
public class Movies : IMovies
{
    private readonly IJsonDatabase _jsonDatabase;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="jsonDatabase"></param>
    public Movies(IJsonDatabase jsonDatabase)
    {
        _jsonDatabase = jsonDatabase ?? throw new ArgumentNullException(nameof(jsonDatabase));
    }

    /// <inheritdoc />
    public IMovieRecord ValueById(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _jsonDatabase.ValueForId(id);
    }

    /// <inheritdoc />
    public IMovieRecord ValueByName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        return _jsonDatabase.ValueForName(name);
    }

    /// <inheritdoc />
    public void Update(IMovieRecord movieRecord)
    {
        ArgumentNullException.ThrowIfNull(movieRecord);

        _jsonDatabase.Update(movieRecord);
    }

    /// <inheritdoc />
    public void Create(IMovieRecord movieRecord)
    {
        ArgumentNullException.ThrowIfNull(movieRecord);

        _jsonDatabase.Create(movieRecord);
    }

    /// <inheritdoc />
    public void Delete(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        _jsonDatabase.Delete(id);
    }

    /// <inheritdoc />
    public List<MovieRecord> Value => _jsonDatabase.GetValue().Select(m => (MovieRecord)m).OrderBy(m => m.Name).ToList();
}