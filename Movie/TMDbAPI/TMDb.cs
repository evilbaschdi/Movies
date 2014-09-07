using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace Movie.TMDbAPI
{
    public class TmDb
    {
        private readonly TMDbClient _client;

        public TmDb()
        {
            _client = new TMDbClient("c6b31d1cdad6a56a23f0c913e2482a31"); //Todo: eigenen Key anfordern
        }

        //public Part GetMoviePart(string query)
        //{
        //    var collection = GetMovieSearchCollection(query);

        //    foreach (var part in collection.Parts)
        //        Console.WriteLine(part.Title);
        //}

        public Collection GetMovieSearchCollection(string query)
        {
            var searchCollection = _client.SearchCollection(query);

            return _client.GetCollection(searchCollection.Results.First().Id);
        }

        public SearchContainer<SearchMovie> GetMovieByTitle(string title)
        {
            return _client.SearchMovie(title);
        }
    }
}