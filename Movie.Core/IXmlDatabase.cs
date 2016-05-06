using System.Data;

namespace Movie.Core
{
    public interface IXmlDatabase
    {
        void Insert(string name, string year, string format, string distributed, string distributedTo, string watched);

        void Update(string id, string name, string year, string format, string distributed, string distributedTo,
                    string watched);

        void Delete(string id);

        DataRow SelectById(string id);

        DataRow SelectByName(string name);

        DataView SelectAll();

        DataView SelectFiltered(string filter, string category);
    }
}