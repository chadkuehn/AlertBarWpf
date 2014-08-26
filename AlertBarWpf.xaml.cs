using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlertBarWpf
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class AlertBarWpf : UserControl
    {
        public AlertBarWpf()
        {
            InitializeComponent();
            tbWrapper.DataContext = this;
        }


        public static readonly RoutedEvent ShowEvent = EventManager.RegisterRoutedEvent("Show", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AlertBarWpf));

        public event RoutedEventHandler Show
        {
            add { AddHandler(ShowEvent, value); }
            remove { RemoveHandler(ShowEvent, value); }
        }

        private void RaiseShowEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(AlertBarWpf.ShowEvent);
            RaiseEvent(newEventArgs);
        }

        private void houseKeeping(string msg, int secs)
        {
            tbMessage.Text = msg;
            tbWrapper.Visibility = System.Windows.Visibility.Visible;
            key1.KeyTime = new TimeSpan(0, 0, (secs == 0 ? 0 : secs - 1));
            key2.KeyTime = new TimeSpan(0, 0, secs);
            RaiseShowEvent();
        }

        private void hideIcon()
        {
            imgStatusIcon.Visibility = System.Windows.Visibility.Collapsed;
            tbWrapper.ColumnDefinitions.RemoveAt(0);
            tbMessage.SetValue(Grid.ColumnProperty, 0);
            imgCloseIcon.SetValue(Grid.ColumnProperty, 1);
            tbMessage.Margin = new Thickness(10, 4, 0, 4);
        }


        /// <summary>
        /// Shows a Danger Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetDangerAlert(string message, int timeoutInSeconds = 0)
        {
            SolidColorBrush bg = new SolidColorBrush();
            bg = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D9534F"));
            tbWrapper.Background = bg;
            tbMessage.Foreground = new SolidColorBrush(Colors.White);
            if (_IconVisibility == false)
            {
                hideIcon();
            }
            else
            {
                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/danger_16.png", UriKind.Relative));
            }
            houseKeeping(message, timeoutInSeconds);
        }

        /// <summary>
        /// Shows a warning Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetWarningAlert(string message, int timeoutInSeconds = 0)
        {
            //  tbWrapper.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            SolidColorBrush bg = new SolidColorBrush();
            bg = (SolidColorBrush)(new BrushConverter().ConvertFrom("#F0AD4E"));
            tbWrapper.Background = bg;
            tbMessage.Foreground = new SolidColorBrush(Colors.White);
            if (_IconVisibility == false)
            {
                hideIcon();
            }
            else
            {
                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/warning_16.png", UriKind.Relative));
            }
            houseKeeping(message, timeoutInSeconds);

        }

        /// <summary>
        /// Shows a Success Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetSuccessAlert(string message, int timeoutInSeconds = 0)
        {
            tbWrapper.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            SolidColorBrush bg = new SolidColorBrush();
            bg = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5CB85C"));
            tbWrapper.Background = bg;
            tbMessage.Foreground = new SolidColorBrush(Colors.White);
            if (_IconVisibility == false)
            {
                hideIcon();
            }
            else
            {
                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/success_16.png", UriKind.Relative));
            }
            houseKeeping(message, timeoutInSeconds);
        }


        /// <summary>
        /// Shows an Information Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetInformationAlert(string message, int timeoutInSeconds = 0)
        {
            SolidColorBrush bg = new SolidColorBrush();
            bg = (SolidColorBrush)(new BrushConverter().ConvertFrom("#5BC0DE"));
            tbWrapper.Background = bg;
            tbMessage.Foreground = new SolidColorBrush(Colors.White);
            if (_IconVisibility == false)
            {
                hideIcon();
            }
            else
            {
                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/information_16.png", UriKind.Relative));
            }
            houseKeeping(message, timeoutInSeconds);
        }


        private bool _IconVisibility = true;

        /// <summary>
        /// Hide or show icons in the messages.
        /// </summary>
        public bool? IconVisibility
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                _IconVisibility = value ?? false;
            }
            get
            {
                return _IconVisibility;
            }
        }



        /// <summary>
        /// Remove a message if one is currently being shown.
        /// </summary>
        public void Clear()
        {
            tbWrapper.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clear();

        }

        private void AnimationObject_Completed(object sender, EventArgs e)
        {
            if (tbWrapper.Opacity == 0)
            {
                //If you call msgbar.setErrorMessage("Whateva") in MainWindow() of your WPF the window is not rendered yet.  So opacity is 0.  If you have a timeout of 0 then it would call this immediately
                if (key1.KeyTime.TimeSpan.Seconds > 0)
                {
                    Clear();
                }
            }
        }

    }
}
