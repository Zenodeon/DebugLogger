using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DebugLogger.Wpf
{
    public class AniScrollViewer : ScrollViewer
    {
        //Register a DependencyProperty which has a onChange callback
        public static DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(AniScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnVerticalChanged)));
        public static DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register("CurrentHorizontalOffsetOffset", typeof(double), typeof(AniScrollViewer), new PropertyMetadata(new PropertyChangedCallback(OnHorizontalChanged)));

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnVerticalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AniScrollViewer viewer = d as AniScrollViewer;
            viewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        //When the DependencyProperty is changed change the vertical offset, thus 'animating' the scrollViewer
        private static void OnHorizontalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AniScrollViewer viewer = d as AniScrollViewer;
            viewer.ScrollToHorizontalOffset((double)e.NewValue);
        }

        public double staticVerticalOffset
        {
            get { return vertAnim.To == null? 0 : (double)vertAnim.To; }
            private set { }
        }

        public double staticHorizontalOffset
        {
            get { return (double)this.GetValue(CurrentHorizontalOffsetProperty); }
            set { this.SetValue(CurrentHorizontalOffsetProperty, value); }
        }

        Storyboard sb = new Storyboard();

        DoubleAnimation vertAnim = new DoubleAnimation();
        DoubleAnimation horzAnim = new DoubleAnimation();

        public void Initialize()
        {
            sb.Children.Add(vertAnim);
            sb.Children.Add(horzAnim);

            Storyboard.SetTarget(vertAnim, this);
            Storyboard.SetTargetProperty(vertAnim, new PropertyPath(CurrentVerticalOffsetProperty));

            Storyboard.SetTarget(horzAnim, this);
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(CurrentHorizontalOffsetProperty));

            sb.Begin();
        }

        public void MoveToVerticalOffset(double offset)
        {
            offset = Math.Clamp(offset, 0, ScrollableHeight);

            vertAnim.From = staticVerticalOffset;
            vertAnim.To = offset;
            vertAnim.AccelerationRatio = 0.5;
            vertAnim.DecelerationRatio = 0.5;
            vertAnim.Duration = new Duration(TimeSpan.FromMilliseconds(300));

            sb.Children.Add(vertAnim);

            Storyboard.SetTarget(vertAnim, this);
            Storyboard.SetTargetProperty(vertAnim, new PropertyPath(CurrentVerticalOffsetProperty));

            sb.Begin();
        }

        /*
        public void ScrollToPosition(double x, double y)
        {
            x = Math.Clamp(x, 0, ScrollableHeight);
            y = Math.Clamp(y, 0, ScrollableWidth);

            vertAnim.From = VerticalOffset;
            vertAnim.To = x;
            vertAnim.AccelerationRatio = 0.5;
            vertAnim.DecelerationRatio = 0.5;
            vertAnim.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            horzAnim.From = HorizontalOffset;
            horzAnim.To = y;
            horzAnim.DecelerationRatio = .2;
            horzAnim.Duration = new Duration(TimeSpan.FromMilliseconds(100));


            sb.Children.Add(vertAnim);
            sb.Children.Add(horzAnim);

            Storyboard.SetTarget(vertAnim, this);
            Storyboard.SetTargetProperty(vertAnim, new PropertyPath(CurrentVerticalOffsetProperty));
            Storyboard.SetTarget(horzAnim, this);
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(CurrentHorizontalOffsetProperty));

            sb.Begin();
        }
        */
    }
}


