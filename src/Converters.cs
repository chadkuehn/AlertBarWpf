using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AlertBarWpf.Converters
{
    /// <summary>
    /// Maps an <see cref="AlertType"/> to a stroke-only 16x16 vector glyph (same drawing technique as the
    /// close button's X). Vector instead of raster so the icon stays crisp at any rendered size and the
    /// library doesn't need to bundle/pack PNG assets.
    /// </summary>
    public sealed class AlertTypeToIconGeometryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = value switch
            {
                AlertType.Danger => "M8,1 C4.13,1 1,4.13 1,8 C1,11.87 4.13,15 8,15 C11.87,15 15,11.87 15,8 C15,4.13 11.87,1 8,1 Z M8,5 L8,9.5 M8,11.3 L8,11.4",
                AlertType.Warning => "M8,1.5 L15,14.5 L1,14.5 Z M8,6 L8,10 M8,11.8 L8,11.9",
                AlertType.Success => "M3,8.5 L6.5,12 L13,4",
                AlertType.Information => "M8,1 C4.13,1 1,4.13 1,8 C1,11.87 4.13,15 8,15 C11.87,15 15,11.87 15,8 C15,4.13 11.87,1 8,1 Z M8,4.3 L8,4.4 M8,7 L8,11.5",
                _ => null,
            };

            return data is null ? null : Geometry.Parse(data);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Looks up the shared accent color for an <see cref="AlertType"/>. Used for the Standard theme's
    /// background fill and the Outline theme's border/text/close-glyph color, so both themes stay in sync
    /// and neither needs a background/foreground that assumes a light or dark host.
    /// </summary>
    internal static class AlertAccentColors
    {
        /// <summary>Dark text/close-glyph color for Neutral, since its pale accent can't carry white text.</summary>
        public static readonly Brush NeutralForeground = (Brush)new BrushConverter().ConvertFromString("#333333");

        public static Brush GetBrush(AlertType alertType)
        {
            string hex = alertType switch
            {
                AlertType.Danger => "#D9534F",
                AlertType.Warning => "#F0AD4E",
                AlertType.Success => "#5CB85C",
                AlertType.Information => "#5BC0DE",
                AlertType.Neutral => "#DCDCDC",
                _ => "#FFFFFF",
            };

            return (Brush)new BrushConverter().ConvertFromString(hex);
        }
    }

    /// <summary>
    /// Maps an <see cref="AlertType"/> to its accent color. The same color is used as the background
    /// (Standard theme) or the border/text (Outline theme) — see the DataTriggers in AlertBarWpf.xaml.
    /// </summary>
    public sealed class AlertTypeToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => AlertAccentColors.GetBrush(value is AlertType alertType ? alertType : AlertType.None);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Collapses the status-icon column to zero width when IconVisibility is false, replacing the old
    /// approach of removing/re-adding the ColumnDefinition and re-indexing sibling elements in code-behind.
    /// Relies on ColumnDefinition.Width being bindable, which requires WPF on .NET Core 3.0 or newer.
    /// Also factors in Density, since the icon column needs to be wider in Comfortable than Compact.
    /// </summary>
    public sealed class IconColumnWidthConverter : IMultiValueConverter
    {
        private static readonly GridLength Hidden = new GridLength(0);

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool visible = values.Length > 0 && values[0] is bool b && b;
            if (!visible)
            {
                return Hidden;
            }

            DensityType density = values.Length > 1 && values[1] is DensityType d ? d : DensityType.Comfortable;
            return new GridLength(density == DensityType.Compact ? 26 : 32);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Maps Density to the close button column's width (the close glyph itself shrinks via DataTriggers
    /// in AlertBarWpf.xaml; this keeps the column it sits in matched to that size).
    /// </summary>
    public sealed class DensityToCloseColumnWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => new GridLength(value is DensityType d && d == DensityType.Compact ? 20 : 24);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    /// <summary>
    /// Picks the stroke color for the vector close glyph from whether it's hovered, which theme is
    /// active, and (for Outline) the current alert's accent color. Replaces the old close.png /
    /// close-hover.png / closeBL.png raster trio — the glyph is now drawn with a Path, so color is just a
    /// brush, not a separate image asset per state. Outline uses the accent color instead of a fixed
    /// black/white so the glyph stays visible against both light and dark host backgrounds.
    /// </summary>
    public sealed class CloseGlyphStrokeConverter : IMultiValueConverter
    {
        private static readonly Brush HoverBrush = new SolidColorBrush(Color.FromRgb(0xD5, 0xD5, 0xD5));
        private static readonly Brush StandardBrush = Brushes.White;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMouseOver = values.Length > 0 && values[0] is bool b && b;
            if (isMouseOver)
            {
                return HoverBrush;
            }

            AlertType alertType = values.Length > 2 && values[2] is AlertType a ? a : AlertType.None;
            if (alertType == AlertType.Neutral)
            {
                return AlertAccentColors.NeutralForeground;
            }

            ThemeType theme = values.Length > 1 && values[1] is ThemeType t ? t : ThemeType.Standard;
            if (theme != ThemeType.Outline)
            {
                return StandardBrush;
            }

            return AlertAccentColors.GetBrush(alertType);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
