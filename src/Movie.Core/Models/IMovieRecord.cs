namespace Movie.Core.Models;

/// <summary>
///     Description for MovieRecord.
/// </summary>
public interface IMovieRecord
{
    // ReSharper disable UnusedMemberInSuper.Global
    /// <remarks />
    bool Lent { get; set; }

    /// <remarks />
    string LentTo { get; set; }

    /// <remarks />
    string Format { get; set; }

    /// <remarks />
    string Id { get; set; }

    /// <remarks />
    string Name { get; set; }

    /// <remarks />
    string Watched { get; set; }

    /// <remarks />
    int Year { get; set; }
    // ReSharper restore UnusedMemberInSuper.Global
}