using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Start9.Api.Controls
{
    public class TouchableContextMenu : ContextMenu
    {
        public bool OpenedWithTouch
        {
            get => (bool)GetValue(OpenedWithTouchProperty);
            set => SetValue(OpenedWithTouchProperty, value);
        }

        public static readonly DependencyProperty OpenedWithTouchProperty = DependencyProperty.Register("OpenedWithTouch",
            typeof(bool), typeof(TouchableContextMenu),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public TouchableContextMenu()
        {
            Loaded += TouchableContextMenu_Loaded;
        }

        private void TouchableContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            var source = ContextMenuService.GetPlacementTarget(this);
            var placeTarget = PlacementTarget as UIElement;
            if (source != null)
            {
                (source as UIElement).TouchDown += Source_TouchDown;
            }
            else
            if (placeTarget != null)
            {
                placeTarget.TouchDown += Source_TouchDown;
            }
        }

        private void Source_TouchDown(object sender, TouchEventArgs e)
        {
            //TouchStarted = DateTime.Now;
            Timer touchTimer = new Timer(1);
            touchTimer.Elapsed += delegate
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    if (IsOpen)
                    {
                        OpenedWithTouch = true;
                        Debug.WriteLine("T O C C H");
                    }
                    else if (!((e.OriginalSource as UIElement).AreAnyTouchesOver))
                    {
                        touchTimer.Stop();
                        Debug.WriteLine("S T O N P");
                    }
                }));
            };
            touchTimer.Start();
        }
    }
}
