using System.Text;

namespace Movie.Core
{
    /// <summary>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Decodes a given string to UTF8
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DecodeString(this string input)
        {
            var bytes = Encoding.Default.GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}