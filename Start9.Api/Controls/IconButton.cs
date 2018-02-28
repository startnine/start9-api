using System.Windows;
using System.Windows.Controls;

namespace Start9.Api.Controls
{
	public class IconButton : Button
	{
		public static readonly DependencyProperty IconProperty =
			DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(IconButton),
				new PropertyMetadata(null));

		public object Icon
		{
			get => GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}
	}


	public class IconTreeViewItem : TreeViewItem
	{
		public static readonly DependencyProperty IconProperty =
			DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(IconTreeViewItem),
				new PropertyMetadata(null));

		public object Icon
		{
			get => GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}
	}


	public class IconListViewItem : ListViewItem
	{
		public static readonly DependencyProperty IconProperty =
			DependencyProperty.RegisterAttached("Icon", typeof(object), typeof(IconListViewItem),
				new PropertyMetadata(null));

		public object Icon
		{
			get => GetValue(IconProperty);
			set => SetValue(IconProperty, value);
		}
	}
}