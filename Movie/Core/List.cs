using System;
using System.Collections;

namespace Movie.Core
{
    public class List
    {
        private static MovieRecord _movieRecord;

        public static MovieRecord GetMovieById(string id)
        {
            _movieRecord = null;

            var dataRow = XmlDatabase.SelectById(id);
            if(dataRow != null)
            {
                _movieRecord = new MovieRecord
                {
                    Id = dataRow[0] != DBNull.Value ? dataRow[0].ToString() : string.Empty,
                    Name = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty,
                    Year = dataRow[2] != DBNull.Value ? dataRow[2].ToString() : string.Empty,
                    Format = dataRow[3] != DBNull.Value ? dataRow[3].ToString() : string.Empty
                };
            }
            return _movieRecord;
        }

        public static MovieRecord GetMovieByName(string name)
        {
            _movieRecord = null;

            var dataRow = XmlDatabase.SelectByName(name);
            if(dataRow != null)
            {
                _movieRecord = new MovieRecord
                {
                    Id = dataRow[0] != DBNull.Value ? dataRow[0].ToString() : string.Empty,
                    Name = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty,
                    Year = dataRow[2] != DBNull.Value ? dataRow[2].ToString() : string.Empty,
                    Format = dataRow[3] != DBNull.Value ? dataRow[3].ToString() : string.Empty
                };
            }
            return _movieRecord;
        }

        public static IList MovieList()
        {
            var dataView = XmlDatabase.SelectAll();
            return dataView;
        }

        public static void Update(MovieRecord movieRecord)
        {
            XmlDatabase.Update(movieRecord.Id, movieRecord.Name, movieRecord.Year, movieRecord.Format);
        }

        public static void Insert(MovieRecord movieRecord)
        {
            XmlDatabase.Insert(movieRecord.Id, movieRecord.Name, movieRecord.Year, movieRecord.Format);
        }

        public static void Delete(string id)
        {
            XmlDatabase.Delete(id);
        }
    }
}