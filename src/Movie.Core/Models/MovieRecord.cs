using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Movie.Core.Models;

/// <summary>
///     Description for MovieRecord.
/// </summary>
[DataContract]
public class MovieRecord : IMovieRecord
{
    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("year")]
    public int Year { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("format")]
    public string Format { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("lent")]
    public bool Lent { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("lentTo")]
    public string LentTo { get; set; }

    /// <inheritdoc />
    [DataMember]
    [JsonPropertyName("watched")]
    public string Watched { get; set; }
}