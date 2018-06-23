using Start9.Api.Programs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Start9.Api.Controls
{
    [TemplatePart(Name = PartButtonsStack, Type = typeof(StackPanel))]
    [TemplatePart(Name = PartGroupToggleButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PartSingleGroupTab, Type = typeof(Border))]
    [TemplatePart(Name = PartDoubleGroupTab, Type = typeof(Border))]

    public partial class TaskItemGroup : Control
    {
        const String PartButtonsStack = "PART_ButtonsStack";
        const String PartGroupToggleButton = "PART_GroupToggleButton";
        const String PartSingleGroupTab = "PART_SingleGroupTab";
        const String PartDoubleGroupTab = "PART_DoubleGroupTab";

        public String ProcessName
        {
            get => (String)GetValue(ProcessNameProperty);
            set => SetValue(ProcessNameProperty, (value));
        }

        public static readonly DependencyProperty ProcessNameProperty = DependencyProperty.Register("ProcessName", typeof(String), typeof(TaskItemGroup), new PropertyMetadata("", OnProcessNamePropertyChangedCallback));

        static void OnProcessNamePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public List<ProgramWindow> ProcessWindows
        {
            get => (List<ProgramWindow>)GetValue(ProcessWindowsProperty);
            set => SetValue(ProcessWindowsProperty, (value));
        }

        public static readonly DependencyProperty ProcessWindowsProperty = DependencyProperty.Register("ProcessWindows", typeof(List<ProgramWindow>), typeof(TaskItemGroup), new PropertyMetadata(new List<ProgramWindow>()));

        public Boolean CombineButtons
        {
            get => (Boolean)GetValue(CombineButtonsProperty);
            set => SetValue(CombineButtonsProperty, (value));
        }

        public static readonly DependencyProperty CombineButtonsProperty = DependencyProperty.Register("CombineButtons", typeof(Boolean), typeof(TaskItemGroup), new PropertyMetadata(true, OnCombineButtonsPropertyChangedCallback));

        static void OnCombineButtonsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public Boolean UseSmallButtons
        {
            get => (Boolean)GetValue(UseSmallButtonsProperty);
            set => SetValue(UseSmallButtonsProperty, (value));
        }

        public static readonly DependencyProperty UseSmallButtonsProperty = DependencyProperty.Register("UseSmallButtons", typeof(Boolean), typeof(TaskItemGroup), new PropertyMetadata(false));

        public Boolean IsVertical
        {
            get => (Boolean)GetValue(IsVerticalProperty);
            set => SetValue(IsVerticalProperty, (value));
        }

        public static readonly DependencyProperty IsVerticalProperty = DependencyProperty.Register("IsVertical", typeof(Boolean), typeof(TaskItemGroup), new PropertyMetadata(false));


        public TaskItemGroup(String programName)
        {
            ProcessName = programName;

            foreach (ProgramWindow p in ProgramWindow.ProgramWindows)
            {
                try
                {
                    if (p.Process.MainModule.FileName == ProcessName)
                    {
                        ProcessWindows.Add(p);
                    }
                }
                catch (Win32Exception ex) { Debug.WriteLine(ex); }
            }
        }

        StackPanel _buttonsStack;
        ToggleButton _groupToggleButton;
        Border _singleGroupTab;
        Border _doubleGroupTab;

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
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            var hwnds = (List<ProgramWindow>)value;
            var Buttons = new List<ToggleButton>();

            foreach (ProgramWindow hwnd in hwnds)
            {
                ToggleButton button = new ToggleButton()
                {
                    Tag = hwnd,
                };

                Buttons.Add(button);
            }
            
            return Buttons;
        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}