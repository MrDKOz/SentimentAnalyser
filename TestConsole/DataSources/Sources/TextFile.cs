using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace TestConsole.DataSources.Sources
{
    public class TextFile : IDataSource
    {
        private string _filePath;
        private bool _clean;

        public TextFile(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> FetchData(int batchSize = 5)
        {
            var records = new List<string>(batchSize);

            if (File.Exists(_filePath))
            {
                HandleZippedFiles();

                records = File.ReadLines(_filePath).Take(batchSize).ToList();

                if (_clean)
                    Directory.Delete("tmp", true);
            }

            return records;
        }

        public List<string> SearchData(string searchTerm, int batchSize = 5)
        {
            throw new System.NotImplementedException();
        }

        private void HandleZippedFiles()
        {
            if (Path.GetExtension(_filePath) == ".zip")
            {
                ZipFile.ExtractToDirectory(_filePath, "tmp", true);

                var files = Directory.GetFiles("tmp", "*.txt");

                if (files.Any())
                    _filePath = files.First();

                _clean = true;
            }
        }
    }
}
