using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Movie.AppCore
{
    /// <summary>
    /// </summary>
    public class RowToIndexConverter : MarkupExtension, IValueConverter
    {
        private static RowToIndexConverter _converter;

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var row = value as DataGridRow;
            if(row == null)
            {
                throw new InvalidOperationException("This converter class can only be used with DataGridRow elements.");
            }
            return row.GetIndex() + 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
            => _converter ?? (_converter = new RowToIndexConverter());
    }
}