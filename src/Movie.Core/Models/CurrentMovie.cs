namespace Movie.Core.Models;

/// <inheritdoc cref="ICurrentMovie" />
public class CurrentMovie : CachedWritableValue<IMovieRecord>, ICurrentMovie
{
    private IMovieRecord _movieRecord;

    /// <inheritdoc />
    protected override IMovieRecord NonCachedValue => _movieRecord;

    /// <inheritdoc />
    protected override void SaveValue([NotNull] IMovieRecord movieRecord)
    {
        _movieRecord = movieRecord ?? throw new ArgumentNullException(nameof(movieRecord));
    }

    /// <inheritdoc />
    public string Id
    {
        get => Value?.Id;
        set => Value.Id = value;
    }

    /// <inheritdoc />
    public string Name
    {
        get => Value?.Name;
        set => Value.Name = value;
    }

    /// <inheritdoc />
    public int Year
    {
        get => Value?.Year ?? 0;
        set => Value.Year = value;
    }

    /// <inheritdoc />
    public string Format
    {
        get => Value?.Format;
        set => Value.Format = value;
    }

    /// <inheritdoc />
    public bool Distributed
    {
        get => Value?.Distributed ?? false;
        set => Value.Distributed = value;
    }

    /// <inheritdoc />
    public string DistributedTo
    {
        get => Value?.DistributedTo;
        set => Value.DistributedTo = value;
    }

    /// <inheritdoc />
    public string Watched
    {
        get => Value?.Watched;
        set => Value.Watched = value;
    }
}