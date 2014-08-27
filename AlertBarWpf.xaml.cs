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
            // grdWrapper.DataContext = this;
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

        private void TransformStage(string msg, int secs, string colorhex, string iconsrc)
        {


            SolidColorBrush bg = new SolidColorBrush();
            bg = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorhex));

            Grid grdParent;
            switch (_Theme)
            {
                case ThemeType.Standard:
                    spStandard.Visibility = System.Windows.Visibility.Visible;
                    spOutline.Visibility = System.Windows.Visibility.Collapsed;

                    grdParent = FindVisualChildren<Grid>(spStandard).FirstOrDefault();
                    grdParent.Background = bg;
                    break;
                case ThemeType.Outline:
                default:
                    spStandard.Visibility = System.Windows.Visibility.Collapsed;
                    spOutline.Visibility = System.Windows.Visibility.Visible;

                    grdParent = FindVisualChildren<Grid>(spOutline).FirstOrDefault();
                    bdr.BorderBrush = bg;
                    break;
            }

            TextBlock lblMessage = FindVisualChildren<TextBlock>(grdParent).FirstOrDefault();
            List<Image> imgs = FindVisualChildren<Image>(grdParent).ToList();
            Image imgStatusIcon = imgs[0];
            Image imgCloseIcon = imgs[1];


            if (_IconVisibility == false)
            {
                imgStatusIcon.Visibility = System.Windows.Visibility.Collapsed;
                grdParent.ColumnDefinitions.RemoveAt(0);
                lblMessage.SetValue(Grid.ColumnProperty, 0);
                imgCloseIcon.SetValue(Grid.ColumnProperty, 1);
                lblMessage.Margin = new Thickness(10, 4, 0, 4);
                lblMessage.Height = 16;
            }
            else
            {
                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/" + iconsrc, UriKind.Relative));
            }




            lblMessage.Text = msg;
            grdWrapper.Visibility = System.Windows.Visibility.Visible;
            key1.KeyTime = new TimeSpan(0, 0, (secs == 0 ? 0 : secs - 1));
            key2.KeyTime = new TimeSpan(0, 0, secs);
            RaiseShowEvent();
        }



        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }


        /// <summary>
        /// Shows a Danger Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetDangerAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#D9534F";
            string icon = "danger_16.png";
            TransformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Shows a warning Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetWarningAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#F0AD4E";
            string icon = "warning_16.png";

            TransformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Shows a Success Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetSuccessAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#5CB85C";
            string icon = "success_16.png";
            TransformStage(message, timeoutInSeconds, color, icon);
        }


        /// <summary>
        /// Shows an Information Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetInformationAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#5BC0DE";
            string icon = "information_16.png";
            TransformStage(message, timeoutInSeconds, color, icon);
        }


        public enum ThemeType
        {
            Standard = 0,
            Outline = 1
        }

        private ThemeType _Theme = ThemeType.Standard;
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



        public ThemeType? Theme
        {
            set
            {
                if (value == null)
                {
                    return;
                }
                if (Enum.IsDefined(typeof(ThemeType), value))
                {
                    _Theme = value ?? ThemeType.Standard;
                }
            }

            get
            {
                return _Theme;
            }
        }



        /// <summary>
        /// Remove a message if one is currently being shown.
        /// </summary>
        public void Clear()
        {
            grdWrapper.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clear();

        }

        private void AnimationObject_Completed(object sender, EventArgs e)
        {
            if (grdWrapper.Opacity == 0)
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
