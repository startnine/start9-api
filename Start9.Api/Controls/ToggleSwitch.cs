using Start9.Api.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static Start9.Api.SystemScaling;

namespace Start9.Api.Controls
{
    [TemplatePart(Name = PartGrip, Type = typeof(Button))]
    [TemplatePart(Name = PartOffsetter, Type = typeof(Canvas))]
    [TemplatePart(Name = PartStateText, Type = typeof(TextBlock))]

    public partial class ToggleSwitch : CheckBox
    {
        const String PartGrip = "PART_Grip";
        const String PartOffsetter = "PART_Offsetter";
        const String PartStateText = "PART_StateText";

        public String TrueText
        {
            get => (String)GetValue(TrueTextProperty);
            set => SetValue(TrueTextProperty, value);
        }

        public static readonly DependencyProperty TrueTextProperty =
            DependencyProperty.RegisterAttached("TrueText", typeof(String), typeof(ToggleSwitch),
                new PropertyMetadata("True"));

        public String FalseText
        {
            get => (String)GetValue(FalseTextProperty);
            set => SetValue(FalseTextProperty, value);
        }

        public static readonly DependencyProperty FalseTextProperty =
            DependencyProperty.RegisterAttached("FalseText", typeof(String), typeof(ToggleSwitch),
                new PropertyMetadata("False"));

        public String NullText
        {
            get => (String)GetValue(NullTextProperty);
            set => SetValue(NullTextProperty, value);
        }

        public static readonly DependencyProperty NullTextProperty =
            DependencyProperty.RegisterAttached("NullText", typeof(String), typeof(ToggleSwitch),
                new PropertyMetadata("Null"));

        public Double OffsetterWidth
        {
            get => (Double)GetValue(OffsetterWidthProperty);
            set => SetValue(OffsetterWidthProperty, value);
        }

        public static readonly DependencyProperty OffsetterWidthProperty =
            DependencyProperty.RegisterAttached("OffsetterWidth", typeof(Double), typeof(ToggleSwitch),
                new PropertyMetadata((Double)0));

        static ToggleSwitch()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(typeof(ToggleSwitch)));
            IsCheckedProperty.OverrideMetadata(typeof(ToggleSwitch), new FrameworkPropertyMetadata(false, OnIsCheckedChanged));
        }

        public ToggleSwitch()
        {
            //Click += delegate { OnClick(); };
        }

        /*protected override void OnChildDesiredSizeChanged(UIElement child)
        {
            base.OnChildDesiredSizeChanged(child);
            HalfWidth = Width / 2;
        }*/

        static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var toggle = (d as ToggleSwitch);

            toggle.AnimateGripPosition();

            try
            {
                if (toggle.IsChecked == true)
                {
                    toggle._stateText.Text = toggle.TrueText;
                }
                else if (toggle.IsChecked == false)
                {
                    toggle._stateText.Text = toggle.FalseText;
                }
                else
                {
                    toggle._stateText.Text = toggle.NullText;
                }
            } catch { }
        }

        public void AnimateGripPosition()
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromMilliseconds(125)
            };

            Double targetWidth = 0;

            if ((IsChecked == null) & (IsThreeState))
            {
                //toggle.OffsetterWidth
                targetWidth = 16;
                //animation.To = (toggle.ActualWidth / 2) - 18;

            }
            else if (IsChecked == false)
            {
                targetWidth = 0;
            }
            else
            {
                targetWidth = 32;
                //animation.To = toggle.ActualWidth - 18;
            }

            animation.To = targetWidth;

            animation.Completed += delegate
            {
                OffsetterWidth = targetWidth;
                BeginAnimation(ToggleSwitch.OffsetterWidthProperty, null);
                try
                {
                    Debug.WriteLine(IsChecked.Value.ToString());
                } catch { }
            };

            BeginAnimation(ToggleSwitch.OffsetterWidthProperty, animation);
        }

        Button _grip = new Button();
        Canvas _offsetter = new Canvas();
        TextBlock _stateText = new TextBlock();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _grip = GetTemplateChild(PartGrip) as Button;
            _grip.PreviewMouseLeftButtonDown += (sendurr, args) => ToggleSwitch_PreviewMouseLeftButtonDown(this, args);
            _offsetter = GetTemplateChild(PartOffsetter) as Canvas;
            _stateText = GetTemplateChild(PartStateText) as TextBlock;
            OnIsCheckedChanged(this, new DependencyPropertyChangedEventArgs());
        }

        private void ToggleSwitch_PreviewMouseLeftButtonDown(Object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Boolean? originalValue = (sender as ToggleSwitch).IsChecked;
            //var toggleSwitch = (sender as ToggleSwitch);

            var isDragging = false;
            var offsetter = OffsetterWidth;
            //var grip = toggleSwitch._grip;

            var toggleX = RealPixelsToWpfUnits((sender as ToggleSwitch).PointToScreen(new System.Windows.Point(0, 0)).X);
            var gripInitialX = RealPixelsToWpfUnits((sender as ToggleSwitch)._grip.PointToScreen(new System.Windows.Point(0, 0)).X);
            var gripX = RealPixelsToWpfUnits((sender as ToggleSwitch)._grip.PointToScreen(new System.Windows.Point(0, 0)).X);

            Double cursorStartX = RealPixelsToWpfUnits(System.Windows.Forms.Cursor.Position.X);
            Double cursorCurrentX = RealPixelsToWpfUnits(System.Windows.Forms.Cursor.Position.X);
            var cursorChange = (cursorCurrentX - cursorStartX);
            var offset = (gripX - toggleX) + (cursorCurrentX - cursorStartX);
            //System.Windows.Point cursorStartOffsetPoint = new System.Windows.Point(toggleSwitch.Margin.Left, grip.Margin.Top);

            var timer = new System.Timers.Timer(1);

            timer.Elapsed += delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (Mouse.LeftButton == MouseButtonState.Pressed)
                    {
                        //toggleX = DpiManager.ConvertPixelsToWpfUnits((sender as ToggleSwitch).PointToScreen(new System.Windows.Point(0, 0)).X);
                        cursorCurrentX = RealPixelsToWpfUnits(System.Windows.Forms.Cursor.Position.X);

                        cursorChange = (cursorCurrentX - cursorStartX);

                        offset = cursorChange + (gripX - toggleX);
                        Debug.WriteLine(cursorChange.ToString() + "," + offset.ToString());

                        if ((cursorChange > 2) | (cursorChange < -2))
                        {
                            isDragging = true;
                        }

                        OffsetterWidth = offsetter + cursorChange;
                    }
                    else
                    {
                        timer.Stop();
                        //offset = (cursorCurrentX - cursorStartX);
                        if (isDragging)
                        {
                            Double isCheckedOffset = 0;
                            if (IsChecked == true)
                            {
                                isCheckedOffset = 32;
                            }
                            else if (IsChecked == null)
                            {
                                isCheckedOffset = 16;
                            }

                            var toggleChange = cursorChange + isCheckedOffset;
                            if (IsThreeState)
                            {
                                if (toggleChange < 10.666666666666666666666666666667)
                                {
                                    IsChecked = false;
                                    Debug.WriteLine("VERTICT: false");
                                }
                                else if (toggleChange > 21.333333333333333333333333333333)
                                {
                                    IsChecked = true;
                                    Debug.WriteLine("VERTICT: true");
                                }
                                else
                                {
                                    IsChecked = null;
                                    Debug.WriteLine("VERTICT: null");
                                }
                            }
                            else
                            {
                                if (toggleChange < 16)
                                {
                                    IsChecked = false;
                                    Debug.WriteLine("VERTICT: false");
                                }
                                else
                                {
                                    IsChecked = true;
                                    Debug.WriteLine("VERTICT: true");
                                }
                            }
                        }
                        else
                        {
                            base.OnClick();
                        }
                        if (originalValue == IsChecked)
                        {
                            AnimateGripPosition();
                        }
                    }
                }));
            };
            timer.Start();
        }

        protected override void OnClick()
        {
            Debug.WriteLine("C L I C C");
            base.OnClick();
        }
    }
}