using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Start9.Api.Controls
{
    public partial class IconButton : Button
    {
        public Object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Object), typeof(IconButton),
                new PropertyMetadata(null));

        public IconButton()
        {

        }
    }


    public partial class IconTreeViewItem : TreeViewItem
    {
        public Object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Object), typeof(IconTreeViewItem),
                new PropertyMetadata(null));

        public IconTreeViewItem()
        {

        }
    }


    public partial class IconListViewItem : ListViewItem
    {
        public Object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Object), typeof(IconListViewItem),
                new PropertyMetadata(null));

        public IconListViewItem()
        {

        }
    }
}
