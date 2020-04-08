using film_manager.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace film_manager.View
{
    class SearchTextBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var searchText = values[0].ToString().ToLower();
            var name = values[1].ToString().ToLower();
            var label = values[2].ToString().ToLower();

            // nl command
            if (searchText == ">nl")
                return label == "" ? Visibility.Visible : Visibility.Collapsed;

            // search by hashtag mode
            if (searchText.StartsWith("#"))
            {
                var labels = searchText.Replace("#", "").Split(' ').Where(o => o != "");
                foreach (var i in labels)
                    if (i == label)
                        return Visibility.Visible;
                return Visibility.Collapsed;
            }

            // search by name
            return name.Contains(searchText) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
