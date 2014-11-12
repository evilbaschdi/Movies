using System;
using System.Data;

namespace Movie.Core
{
    public class XmlDatabase
    {
        private static readonly DataSet DataSet = new DataSet();
        private static DataView _dataView = new DataView();

        /// <summary>
        ///     Inserts a record into the Movie table.
        /// </summary>
        private static void Save()
        {
            var settings = new SettingsStore();
            settings.CheckRegistrySettings();
            DataSet.WriteXml(settings.XmlFilePath, XmlWriteMode.WriteSchema);
        }

        public void Insert(string name, string year, string format, string distributed)
        {
            var dataRow = _dataView.Table.NewRow();
            dataRow[0] = Guid.NewGuid();
            dataRow[1] = name;
            dataRow[2] = year;
            dataRow[3] = format;
            dataRow[4] = distributed;
            _dataView.Table.Rows.Add(dataRow);
            Save();
        }

        /// <summary>
        ///     Updates a record in the movie table.
        /// </summary>
        public void Update(string id, string name, string year, string format, string distributed)
        {
            var dataRow = SelectById(id);
            dataRow[1] = name;
            dataRow[2] = year;
            dataRow[3] = format;
            dataRow[4] = distributed;
            Save();
        }

        /// <summary>
        ///     Deletes a record from the movie table by a composite primary key.
        /// </summary>
        public void Delete(string id)
        {
            _dataView.RowFilter = string.Format("Id='{0}'", id);
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
            _dataView.RowFilter = string.Format("Id='{0}'", id);
            _dataView.Sort = "Id";
            DataRow dataRow = null;
            if(_dataView.Count > 0)
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
            _dataView.RowFilter = string.Format("Name='{0}'", name);
            _dataView.Sort = "Name";
            DataRow dataRow = null;
            if(_dataView.Count > 0)
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
            DataSet.Clear();
            var settings = new SettingsStore();
            settings.CheckRegistrySettings();
            DataSet.ReadXml(settings.XmlFilePath, XmlReadMode.ReadSchema);
            _dataView = DataSet.Tables[0].DefaultView;
            return _dataView;
        }

        public DataView SelectFiltered(string filter, string category)
        {
            DataSet.Clear();
            var settings = new SettingsStore();
            settings.CheckRegistrySettings();
            DataSet.ReadXml(settings.XmlFilePath, XmlReadMode.ReadSchema);
            _dataView = DataSet.Tables[0].DefaultView;
            _dataView.RowFilter = string.Format("{0} LIKE '%{1}%'", category, filter);
            return _dataView;
        }
    }
}