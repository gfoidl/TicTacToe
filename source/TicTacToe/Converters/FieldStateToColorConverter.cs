using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using TicTacToe.Engine;

namespace TicTacToe.Converters
{
    public class FieldStateToColorConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        //---------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((FieldState)value)
            {
                case FieldState.Empty:
                    return SystemColors.ControlBrush;
                case FieldState.Machine:
                    return new SolidColorBrush(Colors.Yellow);
                case FieldState.User:
                    return new SolidColorBrush(Colors.LightBlue);
                default:
                    throw new NotSupportedException();
            }
        }
        //---------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
