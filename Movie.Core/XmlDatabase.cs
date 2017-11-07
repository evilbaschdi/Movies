using System;
using System.Data;

namespace Movie.Core
{
    public class XmlDatabase : IXmlDatabase
    {
        private readonly IXmlSettings _xmlSettings;
        private readonly DataSet _dataSet = new DataSet();
        private DataView _dataView = new DataView();

        public XmlDatabase(IXmlSettings xmlSettings)
        {
            _xmlSettings = xmlSettings ?? throw new ArgumentNullException(nameof(xmlSettings));
        }

        /// <summary>
        ///     Inserts a record into the Movie table.
        /// </summary>
        private void Save()
        {
            _dataSet.WriteXml(_xmlSettings.FilePath, XmlWriteMode.WriteSchema);
        }

        public void Insert(string name, string year, string format, string distributed, string distributedTo,
                           string watched)
        {
            var dataRow = _dataView.Table.NewRow();
            dataRow["Id"] = Guid.NewGuid();
            dataRow["Name"] = name;
            dataRow["Year"] = year;
            dataRow["Format"] = format;
            dataRow["Distributed"] = distributed;
            dataRow["DistributedTo"] = distributedTo;
            dataRow["Watched"] = watched;
            _dataView.Table.Rows.Add(dataRow);
            Save();
        }

        /// <summary>
        ///     Updates a record in the movie table.
        /// </summary>
        public void Update(string id, string name, string year, string format, string distributed, string distributedTo,
                           string watched)
        {
            var dataRow = SelectById(id);
            dataRow["Name"] = name;
            dataRow["Year"] = year;
            dataRow["Format"] = format;
            dataRow["Distributed"] = distributed;
            dataRow["DistributedTo"] = distributedTo;
            dataRow["Watched"] = watched;
            Save();
        }

        /// <summary>
        ///     Deletes a record from the movie table by a composite primary key.
        /// </summary>
        public void Delete(string id)
        {
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

        public DataView SelectFiltered(string filter, string category)
        {
            _dataSet.Clear();
            _dataSet.ReadXml(_xmlSettings.FilePath, XmlReadMode.ReadSchema);
            _dataView = _dataSet.Tables[0].DefaultView;
            _dataView.RowFilter = $"{category} LIKE '%{filter}%'";
            return _dataView;
        }
    }
}