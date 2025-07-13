using System.IO;
using System.Text.Json;

namespace SQE_PROJECT.Helpers
{
    public static class TestDataLoader
    {
        public static T Load<T>(string filename)
        {
            var path = Path.Combine("TestData", filename);
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}