using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text;

namespace DebugLogger.Wpf
{
    public static class DefaultLogStructure
    {
        public static SolidColorBrush GetDefaultLogColor(Enum type, double brightness = 100)
        {
            switch (type)
            {
                case DefaultLogType.Master:
                    return new SolidColorBrush(Color.FromArgb(255, 45, 45, 48)).Brightness(brightness); //White

                case DefaultLogType.Warning:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)).Brightness(brightness); //Yellow

                case DefaultLogType.Error:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)).Brightness(brightness); //Red

                default:
                    return new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)).Brightness(brightness); //White
            }
        }
    }

    public enum DefaultLogType
    {
        Master,
        Warning,
        Error
    }

    public static class SolidColorBrushExtentsion
    {
        public static SolidColorBrush Brightness(this SolidColorBrush brush, double percentage)
        {
            if (percentage == 100)
                return brush;

            percentage = Math.Clamp(percentage, 0, 100) / 100;
            byte r = (byte)(int)(brush.Color.R * percentage);
            byte g = (byte)(int)(brush.Color.G * percentage);
            byte b = (byte)(int)(brush.Color.B * percentage);

            return new SolidColorBrush(Color.FromArgb(brush.Color.A, r, g, b));
        }
    }
}
