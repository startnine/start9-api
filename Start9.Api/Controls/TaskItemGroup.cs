using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Start9.Api.Programs;

namespace Start9.Api.Controls
{
	[TemplatePart(Name = PartButtonsStack, Type = typeof(StackPanel))]
	[TemplatePart(Name = PartGroupToggleButton, Type = typeof(ToggleButton))]
	[TemplatePart(Name = PartSingleGroupTab, Type = typeof(Border))]
	[TemplatePart(Name = PartDoubleGroupTab, Type = typeof(Border))]
	public class TaskItemGroup : Control
	{
		private const string PartButtonsStack = "PART_ButtonsStack";
		private const string PartGroupToggleButton = "PART_GroupToggleButton";
		private const string PartSingleGroupTab = "PART_SingleGroupTab";
		private const string PartDoubleGroupTab = "PART_DoubleGroupTab";

		public static readonly DependencyProperty ProcessNameProperty = DependencyProperty.Register("ProcessName",
			typeof(string), typeof(TaskItemGroup), new PropertyMetadata("", OnProcessNamePropertyChangedCallback));

		public static readonly DependencyProperty ProcessWindowsProperty = DependencyProperty.Register("ProcessWindows",
			typeof(List<ProgramWindow>), typeof(TaskItemGroup), new PropertyMetadata(new List<ProgramWindow>()));

		public static readonly DependencyProperty CombineButtonsProperty = DependencyProperty.Register("CombineButtons",
			typeof(bool), typeof(TaskItemGroup), new PropertyMetadata(true, OnCombineButtonsPropertyChangedCallback));

		public static readonly DependencyProperty UseSmallButtonsProperty =
			DependencyProperty.Register("UseSmallButtons", typeof(bool), typeof(TaskItemGroup), new PropertyMetadata(false));

		public static readonly DependencyProperty IsVerticalProperty =
			DependencyProperty.Register("IsVertical", typeof(bool), typeof(TaskItemGroup), new PropertyMetadata(false));

		private StackPanel _buttonsStack;
		private Border _doubleGroupTab;
		private ToggleButton _groupToggleButton;
		private Border _singleGroupTab;


		public TaskItemGroup(string programName)
		{
			ProcessName = programName;

			foreach (var p in ProgramWindow.ProgramWindows)
				try
				{
					if (p.Process.MainModule.FileName == ProcessName) ProcessWindows.Add(p);
				}
				catch (Win32Exception ex)
				{
					Debug.WriteLine(ex);
				}
		}

		public string ProcessName
		{
			get => (string) GetValue(ProcessNameProperty);
			set => SetValue(ProcessNameProperty, value);
		}

		public List<ProgramWindow> ProcessWindows
		{
			get => (List<ProgramWindow>) GetValue(ProcessWindowsProperty);
			set => SetValue(ProcessWindowsProperty, value);
		}

		public bool CombineButtons
		{
			get => (bool) GetValue(CombineButtonsProperty);
			set => SetValue(CombineButtonsProperty, value);
		}

		public bool UseSmallButtons
		{
			get => (bool) GetValue(UseSmallButtonsProperty);
			set => SetValue(UseSmallButtonsProperty, value);
		}

		public bool IsVertical
		{
			get => (bool) GetValue(IsVerticalProperty);
			set => SetValue(IsVerticalProperty, value);
		}

		private static void OnProcessNamePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		private static void OnCombineButtonsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_buttonsStack = GetTemplateChild(PartButtonsStack) as StackPanel;

			_groupToggleButton = GetTemplateChild(PartGroupToggleButton) as ToggleButton;

			_singleGroupTab = GetTemplateChild(PartSingleGroupTab) as Border;

			_doubleGroupTab = GetTemplateChild(PartDoubleGroupTab) as Border;
		}
	}

	public class ListIntPtrToListTaskItemButtonConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var hwnds = (List<ProgramWindow>) value;
			var Buttons = new List<ToggleButton>();

			foreach (var hwnd in hwnds)
			{
				var button = new ToggleButton
				{
					Tag = hwnd
				};

				Buttons.Add(button);
			}

			return Buttons;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}