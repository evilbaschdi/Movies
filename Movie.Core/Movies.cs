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
        ArgumentNullException.ThrowIfNull(id);

        var dataRow = _xmlDatabase.ValueForId(id);
        return dataRow != null ? _transformDataRowToMovieRecord.ValueFor(dataRow) : null;
    }

    /// <inheritdoc />
    public IMovieRecord ValueByName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var dataRow = _xmlDatabase.ValueForName(name);
        return dataRow != null ? _transformDataRowToMovieRecord.ValueFor(dataRow) : null;
    }

    /// <inheritdoc />
    public void Update(IMovieRecord movieRecord)
    {
        ArgumentNullException.ThrowIfNull(movieRecord);

        _xmlDatabase.Update(movieRecord);
    }

    /// <inheritdoc />
    public void Create(IMovieRecord movieRecord)
    {
        ArgumentNullException.ThrowIfNull(movieRecord);

        _xmlDatabase.Create(movieRecord);
    }

    /// <inheritdoc />
    public void Delete(string id)
    {
        ArgumentNullException.ThrowIfNull(id);

        _xmlDatabase.Delete(id);
    }

    /// <inheritdoc />
    public List<MovieRecord> Value => (from DataRowView dataRowView in _xmlDatabase.Value select _transformDataRowToMovieRecord.ValueFor(dataRowView.Row))
                                      .OrderBy(m => m.Name).ToList();
}