using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPsy.AuxServices
{
    public static class ColorPicker
    {
        public static SKColor GetRandomColor()
        {
            Random random = new Random();
            var red = random.Next(140, 200);
            var green = random.Next(140, 200);
            var blue = random.Next(140, 200);
            return SKColor.FromHsv(red, green, blue);
        }
    }
}
