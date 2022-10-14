using System.Runtime.Serialization;

namespace Movie.Core.Models;

/// <summary>
///     Description for MovieRecord.
/// </summary>
[DataContract]
public class MovieRecord : IMovieRecord
{
    /// <inheritdoc />
    [DataMember]
    public string Id { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string Name { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string Year { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string Format { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string Distributed { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string DistributedTo { get; set; }

    /// <inheritdoc />
    [DataMember]
    public string Watched { get; set; }
}