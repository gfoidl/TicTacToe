using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using TicTacToe.Engine;

namespace TicTacToe.Converters
{
    [ValueConversion(typeof(FieldState), typeof(string))]
    public class FieldStateToSignConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        //---------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((FieldState)value)
            {
                case FieldState.Empty:
                    return " ";
                case FieldState.Machine:
                    return "O";
                case FieldState.User:
                    return "X";
                default:
                    throw new NotSupportedException();
            }
        }
        //---------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
