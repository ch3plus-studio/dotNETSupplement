using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Markup;
using System.Windows.Data;

namespace dotNETSupplement.MVVM.Convertors
{
    public class EnumValueToEnumNameConverter : MarkupExtension, IValueConverter
    {
        private static EnumValueToEnumNameConverter _converter;

        /// <summary>
        /// Convert value for binding from source object's enum field to enum name in XAML
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!value.GetType().IsEnum)
                throw new ArgumentException();

            return value.ToString();
        }

        /// <summary>
        /// ConvertBack value from binding back to source object
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Cant convert back from enum name to enum value");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new EnumValueToEnumNameConverter());
        }
    }
}
