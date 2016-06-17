using System.Runtime.Serialization;

namespace Movie.Core
{
    /// <summary>
    ///     Description for MovieRecord.
    /// </summary>
    [DataContract]
    public class MovieRecord : IMovieRecord
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Year { get; set; }

        [DataMember]
        public string Format { get; set; }

        [DataMember]
        public string Distributed { get; set; }

        [DataMember]
        public string DistributedTo { get; set; }

        [DataMember]
        public string Watched { get; set; }
    }
}