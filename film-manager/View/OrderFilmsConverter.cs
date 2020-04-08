using film_manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace film_manager.View
{
    class OrderFilmsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var option = values[1].ToString().ToLower();
            var enumerable = (IEnumerable<FilmObjectViewModel>)values[0];
            if (option == "size")
                return enumerable.OrderBy(o => o.Size);
            else
                return enumerable.OrderBy(o => o.Name);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
