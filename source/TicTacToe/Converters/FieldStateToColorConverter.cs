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
        private static readonly SolidColorBrush s_machineBrush = new SolidColorBrush(Colors.Yellow);
        private static readonly SolidColorBrush s_userBrush    = new SolidColorBrush(Colors.LightBlue);
        //---------------------------------------------------------------------
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        //---------------------------------------------------------------------
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((FieldState)value) switch
            {
                FieldState.Empty   => SystemColors.ControlBrush,
                FieldState.Machine => s_machineBrush,
                FieldState.User    => s_userBrush,
                _                  => throw new NotSupportedException(),
            };
        //---------------------------------------------------------------------
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
