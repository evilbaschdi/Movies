namespace Movie.Core
{
    /// <summary>
    ///     Description for MovieRecord.
    /// </summary>
    public class MovieRecord : IMovieRecord
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Year { get; set; }

        public string Format { get; set; }

        public string Distributed { get; set; }
    }
}