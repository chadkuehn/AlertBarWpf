using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlertBarWpf
{
    /// <summary>
    /// The kind of alert currently displayed. Drives the icon and accent color via
    /// <see cref="Converters.AlertTypeToIconGeometryConverter"/> and <see cref="Converters.AlertTypeToBrushConverter"/>.
    /// </summary>
    public enum AlertType
    {
        None,
        Danger,
        Warning,
        Success,
        Information,
        /// <summary>A non-severity, iconless alert for generic messages that shouldn't imply status.</summary>
        Neutral
    }

    public enum BarStyleType
    {
        Standard = 0,
        Outline = 1
    }

    /// <summary>
    /// Controls the bar's overall scale (icon/text/close-glyph size and spacing).
    /// </summary>
    public enum DensityType
    {
        /// <summary>Larger icon/text/close-glyph sizing, easier to read next to typical modern-themed controls.</summary>
        Comfortable = 0,
        /// <summary>The original, more tightly-packed sizing.</summary>
        Compact = 1
    }

    /// <summary>
    /// A WPF UserControl for displaying user updates through an alert bar.
    /// </summary>
    public partial class AlertBarWpf : UserControl
    {
        public AlertBarWpf()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent ShowEvent = EventManager.RegisterRoutedEvent(
            "Show", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AlertBarWpf));

        public event RoutedEventHandler Show
        {
            add { AddHandler(ShowEvent, value); }
            remove { RemoveHandler(ShowEvent, value); }
        }

        private void RaiseShowEvent()
        {
            RaiseEvent(new RoutedEventArgs(ShowEvent));
        }

        #region Dependency properties

        public static readonly DependencyProperty BarStyleProperty = DependencyProperty.Register(
            nameof(BarStyle), typeof(BarStyleType), typeof(AlertBarWpf), new PropertyMetadata(BarStyleType.Standard));

        /// <summary>
        /// Adjusts the look of the bar. See the <see cref="BarStyleType"/> options.
        /// </summary>
        public BarStyleType BarStyle
        {
            get => (BarStyleType)GetValue(BarStyleProperty);
            set => SetValue(BarStyleProperty, value);
        }

        public static readonly DependencyProperty DensityProperty = DependencyProperty.Register(
            nameof(Density), typeof(DensityType), typeof(AlertBarWpf), new PropertyMetadata(DensityType.Comfortable));

        /// <summary>
        /// Adjusts the bar's overall scale. See the <see cref="DensityType"/> options.
        /// </summary>
        public DensityType Density
        {
            get => (DensityType)GetValue(DensityProperty);
            set => SetValue(DensityProperty, value);
        }

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(
            nameof(IconVisibility), typeof(bool), typeof(AlertBarWpf), new PropertyMetadata(true));

        /// <summary>
        /// Hide or show icons in the messages.
        /// </summary>
        public bool IconVisibility
        {
            get => (bool)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        private static readonly DependencyPropertyKey CurrentAlertTypePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(CurrentAlertType), typeof(AlertType), typeof(AlertBarWpf), new PropertyMetadata(AlertType.None));

        public static readonly DependencyProperty CurrentAlertTypeProperty = CurrentAlertTypePropertyKey.DependencyProperty;

        /// <summary>
        /// The alert currently shown by the bar. Set internally by the Set*Alert methods and bound to
        /// in the XAML to pick the status icon and accent color.
        /// </summary>
        public AlertType CurrentAlertType => (AlertType)GetValue(CurrentAlertTypeProperty);

        private static readonly DependencyPropertyKey MessagePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Message), typeof(string), typeof(AlertBarWpf), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty MessageProperty = MessagePropertyKey.DependencyProperty;

        /// <summary>
        /// The text currently shown by the bar. Set internally by the Set*Alert methods.
        /// </summary>
        public string Message => (string)GetValue(MessageProperty);

        #endregion

        private void TransformStage(string message, int timeoutInSeconds, AlertType alertType)
        {
            SetValue(MessagePropertyKey, message);
            SetValue(CurrentAlertTypePropertyKey, alertType);

            grdWrapper.Visibility = Visibility.Visible;
            key1.KeyTime = new TimeSpan(0, 0, timeoutInSeconds == 0 ? 0 : timeoutInSeconds - 1);
            key2.KeyTime = new TimeSpan(0, 0, timeoutInSeconds);
            RaiseShowEvent();
        }

        /// <summary>
        /// Shows an alert of the given <see cref="AlertType"/>. Use this instead of the named Set*Alert
        /// methods when the type is only known at runtime (e.g. mapped from another enum or a validation
        /// result) rather than a compile-time constant.
        /// </summary>
        /// <param name="alertType">The type of alert to show</param>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetAlert(AlertType alertType, string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, alertType);

        /// <summary>
        /// Shows a Danger Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetDangerAlert(string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, AlertType.Danger);

        /// <summary>
        /// Shows a warning Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetWarningAlert(string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, AlertType.Warning);

        /// <summary>
        /// Shows a Success Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetSuccessAlert(string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, AlertType.Success);

        /// <summary>
        /// Shows an Information Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetInformationAlert(string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, AlertType.Information);

        /// <summary>
        /// Shows a Neutral Alert (no icon, no severity color) for generic messages.
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetNeutralAlert(string message, int timeoutInSeconds = 0)
            => TransformStage(message, timeoutInSeconds, AlertType.Neutral);

        /// <summary>
        /// Remove a message if one is currently being shown.
        /// </summary>
        public void Clear()
        {
            grdWrapper.Visibility = Visibility.Collapsed;
            SetValue(CurrentAlertTypePropertyKey, AlertType.None);
        }

        private void CloseButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clear();
        }

        private void AnimationObject_Completed(object sender, EventArgs e)
        {
            if (grdWrapper.Opacity == 0)
            {
                // If you call msgbar.SetXAlert("Whateva") in the constructor of your window, the window isn't
                // rendered yet, so opacity is still 0. If the timeout is 0 this fires immediately.
                if (key1.KeyTime.TimeSpan.Seconds > 0)
                {
                    Clear();
                }
            }
        }
    }
}
