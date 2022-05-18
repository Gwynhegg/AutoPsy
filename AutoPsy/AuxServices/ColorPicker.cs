using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AutoPsy.AuxServices
{
    public static class ColorPicker     // Статический вспомогательный класс для взаимодействия с колористикой приложения
    {
        public static SKColor GetRandomColor()      // метод для получения случайного цвета для Microcharts
        {
            Random random = new Random();
            var red = random.Next(140, 200);
            var green = random.Next(140, 200);
            var blue = random.Next(140, 200);
            return SKColor.FromHsv(red, green, blue);
        }

        public static Dictionary<float, Color> MatrixCriticityColor = new Dictionary<float, Color>()
        {
            {0, Color.Gray },
            {0.05f, Color.FromRgb(40, 0, 200) },
            {0.1f, Color.FromRgb(0, 80, 200) },
            {0.15f, Color.FromRgb(0, 150, 200) },
            {0.2f, Color.FromRgb(0, 200, 200) },
            {0.25f, Color.FromRgb(0, 200, 100) },
            {0.3f, Color.FromRgb(0, 200, 20) },
            {0.35f, Color.FromRgb(100, 200, 0) },
            {0.4f, Color.FromRgb(170, 200, 0) },
            {0.45f, Color.FromRgb(255, 255, 0) },
            {0.5f, Color.FromRgb(255, 240, 20) },
            {0.55f, Color.FromRgb(255, 200, 20) },
            {0.6f, Color.FromRgb(255, 180, 20) },
            {0.65f, Color.FromRgb(255, 140, 20) },
            {0.7f, Color.FromRgb(255, 120, 20) },
            {0.75f, Color.FromRgb(255, 75, 20) },
            {0.8f, Color.FromRgb(255, 20, 20) },
            {0.85f, Color.FromRgb(150, 15, 0) },
            {0.9f, Color.FromRgb(100, 10, 0) },
            {0.95f, Color.FromRgb(50, 0 ,0) },
            {1, Color.Black }
        };


        public static Dictionary<byte, Brush> ColorBrushScheme = new Dictionary<byte, Brush>()       // Словарь для отображения критичности параметров состояния и соблюдения рекомендаций
        {
            { 0, Brush.Gray},
            { 1, Brush.Lime},
            { 2, Brush.Green},
            { 3, Brush.Yellow},
            { 4, Brush.Orange},
            { 5, Brush.Red}
        };

        public static Dictionary<byte, Brush> CriticalBrushScheme = new Dictionary<byte, Brush>()        // Аналогичный словарь для триггеров
        {
            { 0, Brush.Gray },
            { 1, Brush.Red }
        };

        public static Dictionary<byte, Color> ColorScheme = new Dictionary<byte, Color>()       // Словарь для отображения критичности параметров состояния и соблюдения рекомендаций
        {
            { 0, Color.Gray},
            { 1, Color.Lime},
            { 2, Color.Green},
            { 3, Color.Yellow},
            { 4, Color.Orange},
            { 5, Color.Red}
        };

        public static Dictionary<byte, Color> CriticalScheme = new Dictionary<byte, Color>()        // Аналогичный словарь для триггеров
        {
            { 0, Color.Gray },
            { 1, Color.Red }
        };

        public static Dictionary<byte, SKColor> SKColorScheme = new Dictionary<byte, SKColor>()
        {
            { 0, SKColor.Parse("#808080")},
            { 1, SKColor.Parse("#00FF00")},
            { 2, SKColor.Parse("#008000")},
            { 3, SKColor.Parse("#FFFF00")},
            { 4, SKColor.Parse("#FFA500")},
            { 5, SKColor.Parse("#FF0000")}
        };

        public static Dictionary<byte, SKColor> SKColorCriticalScheme = new Dictionary<byte, SKColor>()
        {
            { 0, SKColor.Parse("#808080") },
            { 1, SKColor.Parse("#FF0000") }
        };
    }

}
