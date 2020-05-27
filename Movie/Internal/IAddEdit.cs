namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public interface IAddEdit
    {
        /// <summary>
        ///     Add or Edit.
        /// </summary>
        string Mode { set; }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="year"></param>
        /// <param name="format"></param>
        /// <param name="id"></param>
        void MovieData(string name, double? year, string format, string id);

        /// <summary>
        /// </summary>
        void Save();
    }
}