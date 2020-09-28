using System.Collections.Generic;

namespace TestConsole.DataSources
{
    public interface IDataSource
    {
        List<string> FetchData(int batchSize = 5);
        List<string> SearchData(string searchTerm, int batchSize = 5);
    }
}