namespace Movie.Core.Models;

/// <summary>
///     Description for MovieRecord.
/// </summary>
public interface IMovieRecord
{
    // ReSharper disable UnusedMemberInSuper.Global
    /// <remarks />
    string Distributed { get; set; }

    /// <remarks />
    string DistributedTo { get; set; }

    /// <remarks />
    string Format { get; set; }

    /// <remarks />
    string Id { get; set; }

    /// <remarks />
    string Name { get; set; }

    /// <remarks />
    string Watched { get; set; }

    /// <remarks />
    string Year { get; set; }
    // ReSharper restore UnusedMemberInSuper.Global
}