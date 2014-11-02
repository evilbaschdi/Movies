using System.Linq;
using Microsoft.Win32;
using TMDbLib.Client;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;

namespace Movie.TMDbAPI
{
    public class TmDb
    {
        private readonly TMDbClient _client;

        public TmDb()
        {
            _client = new TMDbClient(GetApiKeyFromRegistry())
            {
                DefaultCountry = "de",
                DefaultLanguage = "de"
            };
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

        public AlternativeTitles GetGermanTitleById(int id)
        {
            return _client.GetMovieAlternativeTitles(id, "de");
        }

        internal string GetApiKeyFromRegistry()
        {
            var movieKey = Registry.CurrentUser.OpenSubKey(@"Software\EvilBaschdi\Movie",
                RegistryKeyPermissionCheck.ReadSubTree);

            if(movieKey == null)
            {
                return "";
            }
            using(
                var settingsKey = movieKey.OpenSubKey("Program Settings",
                    RegistryKeyPermissionCheck.ReadSubTree))
            {
                return settingsKey != null ? settingsKey.GetValue("ApiKey", "").ToString() : "";
            }
        }
    }
}