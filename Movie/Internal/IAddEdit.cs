namespace Movie.Internal
{
    /// <summary>
    /// </summary>
    public interface IAddEdit
    {
        /// <summary>
        /// </summary>
        string CurrentId { set; }

        /// <summary>
        ///     Add or Edit.
        /// </summary>
        string Mode { set; }

        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="year"></param>
        /// <param name="format"></param>
        void MovieData(string name, double? year, string format);

        /// <summary>
        /// </summary>
        /// <param name="addNew"></param>
        void SaveAndAddNew(bool addNew);
    }
}