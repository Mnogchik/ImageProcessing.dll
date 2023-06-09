﻿using System.Drawing;

namespace ImageProcessor
{
    /// <summary>
    /// Класс, служащий для хранения координаты и цвета пикселя.
    /// </summary>
    internal class Pixel
    {
        public Point Point { get; set; }
        public Color Color { get; set; }
    }
}
