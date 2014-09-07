using System;
using System.Collections;
using System.Data;

namespace Movie.Core
{
    public class List
    {
        public static MovieRecord GetMovie(string id)
        {
            DataRow dataRow = XmlDatabase.Select(id);
            MovieRecord movieRecord = null;
            if (dataRow != null)
            {
                movieRecord = new MovieRecord
                {
                    Id = dataRow[0] != DBNull.Value ? dataRow[0].ToString() : string.Empty,
                    Name = dataRow[1] != DBNull.Value ? dataRow[1].ToString() : string.Empty,
                    Year = dataRow[2] != DBNull.Value ? dataRow[2].ToString() : string.Empty,
                    Format = dataRow[3] != DBNull.Value ? dataRow[3].ToString() : string.Empty
                };
            }
            return movieRecord;
        }

        public static IList MovieList()
        {
            DataView dataView = XmlDatabase.SelectAll();
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