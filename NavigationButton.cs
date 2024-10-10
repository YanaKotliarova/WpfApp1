using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfApp1
{
    public class NavigationButton : ButtonBase
    {
        static NavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigationButton), new FrameworkPropertyMetadata(typeof(NavigationButton)));
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(NavigationButton), new PropertyMetadata(null));
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NavigationButton), new PropertyMetadata(null));
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty NavigationUriProperty = DependencyProperty.Register("NavigationUri", typeof(Uri), typeof(NavigationButton), new PropertyMetadata(null));
        public Uri NavigationUri
        {
            get { return (Uri)GetValue(NavigationUriProperty); }
            set { SetValue(NavigationUriProperty, value); }
        }
    }
}
