namespace Movie.Core.Models
{
    /// <summary>
    ///     Description for MovieRecord.
    /// </summary>
    public interface IMovieRecord
    {
        string Id { get; set; }

        string Name { get; set; }

        string Year { get; set; }

        string Format { get; set; }

        string Distributed { get; set; }

        string DistributedTo { get; set; }

        string Watched { get; set; }
    }
}