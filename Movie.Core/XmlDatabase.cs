using System;
using System.Data;
using Movie.Core.Models;

namespace Movie.Core
{
    /// <inheritdoc />
    public class XmlDatabase : IXmlDatabase
    {
        private readonly DataSet _dataSet = new DataSet();
        private readonly IXmlSettings _xmlSettings;
        private DataView _dataView = new DataView();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="xmlSettings"></param>
        public XmlDatabase(IXmlSettings xmlSettings)
        {
            _xmlSettings = xmlSettings ?? throw new ArgumentNullException(nameof(xmlSettings));
        }

        /// <inheritdoc />
        public void Insert(IMovieRecord movieRecord)
        {
            var dataRow = _dataView.Table.NewRow();
            dataRow["Id"] = Guid.NewGuid();
            dataRow["Name"] = movieRecord.Name;
            dataRow["Year"] = movieRecord.Year;
            dataRow["Format"] = movieRecord.Format;
            dataRow["Distributed"] = movieRecord.Distributed;
            dataRow["DistributedTo"] = movieRecord.DistributedTo;
            dataRow["Watched"] = movieRecord.Watched;
            _dataView.Table.Rows.Add(dataRow);
            Save();
        }

        /// <inheritdoc />
        public void Update(IMovieRecord movieRecord)
        {
            var dataRow = SelectById(movieRecord.Id);
            dataRow["Name"] = movieRecord.Name;
            dataRow["Year"] = movieRecord.Year;
            dataRow["Format"] = movieRecord.Format;
            dataRow["Distributed"] = movieRecord.Distributed;
            dataRow["DistributedTo"] = movieRecord.DistributedTo;
            dataRow["Watched"] = movieRecord.Watched;
            Save();
        }

        /// <summary>
        ///     Deletes a record from the movie table by a composite primary key.
        /// </summary>
        public void Delete(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _dataView.RowFilter = $"Id='{id}'";
            _dataView.Sort = "Id";
            _dataView.Delete(0);
            _dataView.RowFilter = "";
            Save();
        }

        /// <summary>
        ///     Selects a single record from the movie table by a composite primary key.
        /// </summary>
        public DataRow SelectById(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            _dataView.RowFilter = $"Id='{id}'";
            _dataView.Sort = "Id";
            DataRow dataRow = null;
            if (_dataView.Count > 0)
            {
                dataRow = _dataView[0].Row;
            }

            _dataView.RowFilter = "";
            return dataRow;
        }

        /// <summary>
        ///     Selects a single record from the movie table by a movie name.
        /// </summary>
        public DataRow SelectByName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            _dataView.RowFilter = $"Name='{name}'";
            _dataView.Sort = "Name";
            DataRow dataRow = null;
            if (_dataView.Count > 0)
            {
                dataRow = _dataView[0].Row;
            }

            _dataView.RowFilter = "";
            return dataRow;
        }

        /// <summary>
        ///     Selects all records from the movie table.
        /// </summary>
        public DataView SelectAll()
        {
            _dataSet.Clear();
            _dataSet.ReadXml(_xmlSettings.FilePath, XmlReadMode.ReadSchema);
            _dataView = _dataSet.Tables[0].DefaultView;
            return _dataView;
        }

        /// <inheritdoc />
        public DataView SelectFiltered(string filter, string category)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }

            _dataSet.Clear();
            _dataSet.ReadXml(_xmlSettings.FilePath, XmlReadMode.ReadSchema);
            _dataView = _dataSet.Tables[0].DefaultView;
            _dataView.RowFilter = $"{category} LIKE '%{filter}%'";
            return _dataView;
        }

        /// <summary>
        ///     Inserts a record into the Movie table.
        /// </summary>
        private void Save()
        {
            _dataSet.WriteXml(_xmlSettings.FilePath, XmlWriteMode.WriteSchema);
        }
    }
}