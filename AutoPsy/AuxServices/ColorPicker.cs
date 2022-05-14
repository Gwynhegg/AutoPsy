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

        public static Dictionary<byte, Color> ColorScheme = new Dictionary<byte, Color>()       // Словарь для отображения критичности параметров состояния и соблюдения рекомендаций
        {
            { 0, UNKNOWN},
            { 1, MINIMAL},
            { 2, LOW},
            { 3, MEDIUM},
            { 4, HIGH},
            { 5, CRITICAL}
        };

        public static Dictionary<byte, Color> CriticalScheme = new Dictionary<byte, Color>()        // Аналогичный словарь для триггеров
        {
            { 0, UNKNOWN },
            { 1, CRITICAL }
        };

        private static Color CRITICAL = Color.Red;
        private static Color HIGH = Color.Orange;
        private static Color MEDIUM = Color.Yellow;
        private static Color LOW = Color.Green;
        private static Color MINIMAL = Color.Lime;
        private static Color UNKNOWN = Color.Gray;
    }
}
