using System;
using System.IO;

namespace Movie.Core
{
    /// <inheritdoc />
    public class ResourceStreamText : IResourceStreamText
    {
        /// <inheritdoc />
        public string ValueFor(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            using var stream = GetType().Assembly.GetManifestResourceStream($"Movie.Core.{filename}");
            // ReSharper disable AssignNullToNotNullAttribute
            using var sr = new StreamReader(stream);
            var result = sr.ReadToEnd();

            return result;
        }
    }
}