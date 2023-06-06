using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageProcessor
{
    /// <summary>
    /// Класс для обработки фона изображения
    /// </summary>
    public static class ImageProcessing
    {
        /// <summary>
        /// Заменяет пиксели фона на прозрачные
        /// </summary>
        /// <param name="image">Изображение для обработки</param>
        /// <param name="backgroundColor">Цвет фона, который надо убрать</param>
        /// <param name="threshold">Чувствительность обработки (от 0 до 442 включительно)</param>
        /// <returns>Bitmap с прозрачными пикселями фона</returns>
        public static Bitmap DeleteBackground(Bitmap image, Color backgroundColor, int threshold)
        {
            Bitmap _image = new Bitmap(image);

            List<Pixel> background = GetBackground(_image, backgroundColor, threshold);

            for (int i = 0; i < background.Count; i++)
            {
                _image.SetPixel(background[i].Point.X, background[i].Point.Y, Color.Transparent);
            }

            return _image;
        }


        /// <summary>
        /// Заменяет пиксели фона на <paramref name="colorChangeTo"/>.
        /// </summary>
        /// <param name="image">Изображение для обработки</param>
        /// <param name="backgroundColor">Цвет фона, который надо поменять</param>
        /// <param name="colorChangeTo">Цвет, на который надо поменять</param>
        /// <param name="threshold">Чувствительность обработки (от 0 до 442 включительно)</param>
        /// <returns>Bitmap с измененными пикселями фона</returns>
        public static Bitmap MakeSolidBackground(Bitmap image, Color backgroundColor, Color colorChangeTo, int threshold)
        {
            Bitmap _image = new Bitmap(image);

            List<Pixel> background = GetBackground(_image, backgroundColor, threshold);

            for (int i = 0; i < background.Count; i++)
            {
                _image.SetPixel(background[i].Point.X, background[i].Point.Y, colorChangeTo);
            }

            return _image;
        }

        /// <summary>
        /// Осветляет пиксели фона.
        /// </summary>
        /// <param name="image">Изображение для обработки</param>
        /// <param name="backgroundColor">Цвет фона, который надо осветлить</param>
        /// <param name="threshold">Чувствительность обработки (от 0 до 442 включительно)</param>
        /// <returns>Bitmap с осветленными пикселями фона</returns>
        public static Bitmap LightenBackground(Bitmap image, Color backgroundColor, int threshold)
        {
            // создание нового объекта для исключения ошибок ссылочных типов
            Bitmap _image = new Bitmap(image); 

            List<Pixel> background = GetBackground(_image, backgroundColor, threshold);

            for (int i = 0; i < background.Count; i++)
            {
                var R = background[i].Color.R + (0.25 * (255 - background[i].Color.R));
                var G = background[i].Color.G + (0.25 * (255 - background[i].Color.G));
                var B = background[i].Color.B + (0.25 * (255 - background[i].Color.B));
                _image.SetPixel(background[i].Point.X, background[i].Point.Y, Color.FromArgb((int)R, (int)G, (int)B));
            }

            return _image;
        }
        private static List<Pixel> GetPixels(Bitmap image)
        {
            var pixels = new List<Pixel>(image.Width * image.Height);

            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    pixels.Add(new Pixel()
                    {
                        Color = image.GetPixel(x, y),
                        Point = new Point() { X = x, Y = y }
                    });
            return pixels;
        }

        private static List<Pixel> GetBackground(List<Pixel> allPixels, Color backgroundColor, int threshold)
        {
            return allPixels.Where(pixel => ColorDistance(pixel.Color, backgroundColor) < threshold).ToList();
        }

        private static List<Pixel> GetBackground(Bitmap image, Color backgroundColor, int threshold)
        {
            List<Pixel> pixels = GetPixels(image);

            return GetBackground(pixels, backgroundColor, threshold);
        }

        private static int ColorDistance(Color c1, Color c2)
        {
            int rDiff = c1.R - c2.R;
            int gDiff = c1.G - c2.G;
            int bDiff = c1.B - c2.B;

            return (int)Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
        }
    }
}
