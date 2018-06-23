using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Start9.Api.Controls
{
    [TemplatePart(Name = PartIcon, Type = typeof(Object))]
    [TemplatePart(Name = PartHeader, Type = typeof(String))]

    public class CommandLinkButton : Button
    {
        const String PartIcon = "PART_Icon";
        const String PartHeader = "PART_Header";

        public Object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Object), typeof(CommandLinkButton),
                new PropertyMetadata(null));

        public String Header
        {
            get => (String)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.RegisterAttached("Header", typeof(String), typeof(CommandLinkButton),
                new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get => (Brush)GetValue(HeaderForegroundProperty);
            set => SetValue(HeaderForegroundProperty, value);
        }

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.RegisterAttached("HeaderForeground", typeof(Brush), typeof(CommandLinkButton),
                new PropertyMetadata(new SolidColorBrush(Colors.Black))); //(Brush)Resources["CommandLinkButtonHeaderForegroundBrush"];

        /*new private object Content
        {
            get => null;
            set => SetValue(ContentProperty, null);
        }

        new private static readonly DependencyProperty ContentProperty =
            DependencyProperty.RegisterAttached("Content", typeof(object), typeof(CommandLinkButton),
                new PropertyMetadata(null));*/

        Object _icon;
        String _header;

        static CommandLinkButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommandLinkButton), new FrameworkPropertyMetadata(typeof(CommandLinkButton)));
        }

        public CommandLinkButton()
        {
            /*ResourceDictionary plexStyles = new ResourceDictionary()
            {
                Source = new Uri("/Start9.Api;component/Themes/Generic.xaml", UriKind.Relative)
            };

            if (!(Resources.MergedDictionaries.Contains(plexStyles)))
            {
                Resources.MergedDictionaries.Add(plexStyles);
                //Style = (Style)Resources["PlexWindowStyle"];
            }*/
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if ((GetTemplateChild(PartIcon) as Object) != null)
            {
                _icon = GetTemplateChild(PartIcon) as Object;
            }

            if (Content is TextBlock)
            {
                (Content as TextBlock).TextAlignment = TextAlignment.Left;
                (Content as TextBlock).TextWrapping = TextWrapping.Wrap;
                //(Content as TextBlock).Foreground = (SolidColorBrush)Resources["CommandLinkButtonBodyForegroundBrush"];
                _header = (Content as TextBlock).Text;
            }
            else if (Content is String)
            {
                _header = (Content as String);
                Content = new TextBlock()
                {
                    Text = _header,
                    TextAlignment = TextAlignment.Left,
                    TextWrapping = TextWrapping.Wrap
                    //Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x50, 0x50, 0x50))
                    //Foreground = (SolidColorBrush)Resources["CommandLinkButtonBodyForegroundBrush"]
                };
            }
        }
    }
}
