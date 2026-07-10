using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AlertBarWpf.Converters
{
    /// <summary>
    /// Maps an <see cref="AlertType"/> to its 16x16 status icon under Resources.
    /// Used instead of building the BitmapImage by hand in code-behind on every alert call.
    /// </summary>
    public sealed class AlertTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value switch
            {
                AlertType.Danger => "danger_16.png",
                AlertType.Warning => "warning_16.png",
                AlertType.Success => "success_16.png",
                AlertType.Information => "information_16.png",
                _ => null,
            };

            return fileName is null
                ? null
                : new BitmapImage(new Uri("/AlertBarWpf;component/Resources/" + fileName, UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Maps an <see cref="AlertType"/> to its accent color. The same color is used as the background
    /// (Standard theme) or the border (Outline theme) — see the DataTriggers in AlertBarWpf.xaml.
    /// </summary>
    public sealed class AlertTypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string hex = value switch
            {
                AlertType.Danger => "#D9534F",
                AlertType.Warning => "#F0AD4E",
                AlertType.Success => "#5CB85C",
                AlertType.Information => "#5BC0DE",
                _ => "#FFFFFF",
            };

            return new BrushConverter().ConvertFromString(hex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Collapses the status-icon column to zero width when IconVisibility is false, replacing the old
    /// approach of removing/re-adding the ColumnDefinition and re-indexing sibling elements in code-behind.
    /// Relies on ColumnDefinition.Width being bindable, which requires WPF on .NET Core 3.0 or newer.
    /// </summary>
    public sealed class BoolToIconColumnWidthConverter : IValueConverter
    {
        private static readonly GridLength Shown = new GridLength(26);
        private static readonly GridLength Hidden = new GridLength(0);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool visible && visible ? Shown : Hidden;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Picks the stroke color for the vector close glyph from whether it's hovered and which theme is
    /// active. Replaces the old close.png / close-hover.png / closeBL.png raster trio — the glyph is now
    /// drawn with a Path, so color is just a brush, not a separate image asset per state.
    /// </summary>
    public sealed class CloseGlyphStrokeConverter : IMultiValueConverter
    {
        private static readonly Brush HoverBrush = new SolidColorBrush(Color.FromRgb(0xD5, 0xD5, 0xD5));
        private static readonly Brush StandardBrush = Brushes.White;
        private static readonly Brush OutlineBrush = Brushes.Black;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMouseOver = values.Length > 0 && values[0] is bool b && b;
            if (isMouseOver)
            {
                return HoverBrush;
            }

            ThemeType theme = values.Length > 1 && values[1] is ThemeType t ? t : ThemeType.Standard;
            return theme == ThemeType.Outline ? OutlineBrush : StandardBrush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
