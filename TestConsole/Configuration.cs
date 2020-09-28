using Newtonsoft.Json;
using System.IO;

namespace TestConsole
{
    public static class Configuration
    {
        public static T FetchConfig<T>()
        {
            var configFromFile = FetchConfig($"{typeof(T).Name}.json");

            dynamic config = JsonConvert.DeserializeObject<T>(configFromFile);

            return config;
        }

        private static string FetchConfig(string fileName)
        {
            var config = string.Empty;
            var path = $@"Config\{fileName}";

            if (File.Exists(path))
                config = File.ReadAllText(path);

            return config;
        }
    }
}
