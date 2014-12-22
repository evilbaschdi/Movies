using System.Collections;

namespace Movie.Core
{
    public interface IMovies
    {
        void Insert(IMovieRecord movieRecord);

        void Update(IMovieRecord movieRecord);

        void Delete(string id);

        IList MovieDataView();

        IList MovieDataView(string filter, string category);

        IMovieRecord GetMovieById(string id);

        IMovieRecord GetMovieByName(string name);
    }
}