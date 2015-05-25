using System.IO;

namespace Movie.Core
{
    public class Tools : ITools
    {
        public string GetResourceStreamText(string filename)
        {
            string result;

            using (var stream = GetType().Assembly.
                GetManifestResourceStream($"Movie.Core.{filename}"))
            {
                // ReSharper disable AssignNullToNotNullAttribute
                using (var sr = new StreamReader(stream))
                // ReSharper restore AssignNullToNotNullAttribute
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}