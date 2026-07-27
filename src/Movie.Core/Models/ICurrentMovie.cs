namespace Movie.Core.Models;

/// <inheritdoc cref="IMovieRecord" />
public interface ICurrentMovie : IWritableValue<IMovieRecord>, IMovieRecord;