using System.Windows.Input;

namespace Movie.Model
{
    /// <summary>
    /// </summary>
    public class Commands
    {
        /// <summary>
        /// </summary>
        public static readonly RoutedCommand SelectToday = new RoutedCommand("Today", typeof(Commands));
    }
}