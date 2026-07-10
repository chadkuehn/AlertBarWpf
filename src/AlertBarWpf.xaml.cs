using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlertBarWpf
{
    /// <summary>
    /// The kind of alert currently displayed. Drives the icon and accent color via
    /// <see cref="Converters.AlertTypeToIconConverter"/> and <see cref="Converters.AlertTypeToBrushConverter"/>.
    /// </summary>
    public enum AlertType
    {
        None,
        Danger,
        Warning,
        Success,
        Information
    }

    public enum ThemeType
    {
        Standard = 0,
        Outline = 1
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

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
            nameof(Theme), typeof(ThemeType), typeof(AlertBarWpf), new PropertyMetadata(ThemeType.Standard));

        /// <summary>
        /// Adjusts the look of the bar. See the <see cref="ThemeType"/> options.
        /// </summary>
        public ThemeType Theme
        {
            get => (ThemeType)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
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
