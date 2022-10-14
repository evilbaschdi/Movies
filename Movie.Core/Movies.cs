using System.Data;
using JetBrains.Annotations;
using Movie.Core.Models;

namespace Movie.Core;

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
    public List<MovieRecord> Value => (from DataRowView dataRowView in _xmlDatabase.Value select _transformDataRowToMovieRecord.ValueFor(dataRowView.Row))
                                      .OrderBy(m => m.Name).ToList();
}