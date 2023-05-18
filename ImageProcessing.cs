using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageProcessor
{
    /// <summary>
    /// Класс для обработки фона изображения
    /// </summary>
    public class ImageProcessing
    {
        private readonly Bitmap image;
        /// <summary>
        /// Чувствительность обработки изображения; отвечает за то, насколько цвет фона может отличаться от выбранного цвета.
        /// </summary>
        public readonly int threshold;

        
        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если threshold выходит из диапазона [0, 442].</exception>
        public ImageProcessing(Bitmap image, int threshold)
        {
            if (threshold < 0 || threshold > 442)
                throw new ArgumentOutOfRangeException("threshold", "Чувствительность должна быть в пределах от 0 до 442.");

            this.image = image;
            this.threshold = threshold;
        }

        /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если threshold выходит из диапазона [0, 442].</exception>
        public ImageProcessing(Image image, int threshold) : this(new Bitmap(image), threshold) { }


        /// <summary>
        /// Заменяет пиксели фона на прозрачные
        /// </summary>
        /// <param name="backgroundColor">Цвет фона, который надо убрать</param>
        /// <returns>Bitmap с прозрачными пикселями фона</returns>
        public Bitmap DeleteBackground(Color backgroundColor)
        {
            List<Pixel> pixels = GetPixels(); // список всех пикселей картинки
            List<Pixel> background = GetBackground(pixels, backgroundColor); // список пикселей заднего фона

            for (int i = 0; i < background.Count; i++)
            {
                image.SetPixel(background[i].Point.X, background[i].Point.Y, Color.Transparent);
            }

            return image;
        }


        /// <summary>
        /// Заменяет пиксели фона на colorChangeTo.
        /// </summary>
        /// <param name="backgroundColor">Цвет фона, который надо поменять</param>
        /// <param name="colorChangeTo">Цвет, на который надо поменять</param>
        /// <returns>Bitmap с измененными пикселями фона</returns>
        public Bitmap MakeSolidBackground(Color backgroundColor, Color colorChangeTo)
        {
            List<Pixel> pixels = GetPixels();
            List<Pixel> background = GetBackground(pixels, backgroundColor);

            for (int i = 0; i < background.Count; i++)
            {
                image.SetPixel(background[i].Point.X, background[i].Point.Y, colorChangeTo);
            }

            return image;
        }

        /// <summary>
        /// Осветляет пиксели фона.
        /// </summary>
        /// <param name="backgroundColor">Цвет фона, который надо осветлить</param>
        /// <returns>Bitmap с осветленными пикселями фона</returns>
        public Bitmap LightenBackground(Color backgroundColor)
        {
            List<Pixel> pixels = GetPixels();
            List<Pixel> background = GetBackground(pixels, backgroundColor);

            for (int i = 0; i < background.Count; i++)
            {
                var R = background[i].Color.R + (0.25 * (255 - background[i].Color.R));
                var G = background[i].Color.G + (0.25 * (255 - background[i].Color.G));
                var B = background[i].Color.B + (0.25 * (255 - background[i].Color.B));
                image.SetPixel(background[i].Point.X, background[i].Point.Y, Color.FromArgb((int)R, (int)G, (int)B));
            }

            return image;
        }
        private List<Pixel> GetPixels()
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

        private List<Pixel> GetBackground(List<Pixel> allPixels, Color backgroundColor)
        {
            return allPixels.Where(pixel => ColorDistance(pixel.Color, backgroundColor) < threshold).ToList();
        }

        private int ColorDistance(Color c1, Color c2)
        {
            int rDiff = c1.R - c2.R;
            int gDiff = c1.G - c2.G;
            int bDiff = c1.B - c2.B;

            return (int)Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
        }
    }
}
