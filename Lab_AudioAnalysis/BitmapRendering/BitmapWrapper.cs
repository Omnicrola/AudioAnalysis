using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab_AudioAnalysis.BitmapRendering
{
    public class BitmapWrapper
    {
        public int Width { get; }
        public int Height { get; }

        public BitmapSource Bitmap
        {
            get
            {
                return BitmapSource.Create(Width, Height, 96, 96, _pixelFormat, null, _pixelData, _rowStride); ;
            }
        }
        private readonly byte[] _pixelData;
        private readonly int _rowStride;
        private PixelFormat _pixelFormat;

        public BitmapWrapper(double width, double height) : this((int)width, (int)height) { }
        public BitmapWrapper(int width, int height)
        {
            Width = width;
            Height = height;
            _pixelFormat = PixelFormats.Rgb24;
            _rowStride = (width * _pixelFormat.BitsPerPixel + 7) / 8;
            _pixelData = new byte[_rowStride * height];
        }

        public void SetPixel(double x, double y, Color color)
        {
            SetPixel((int)x, (int)y, color);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (x > Width || y > Height)
            {
                return;
            }
            int xIndex = x * 3;
            int yIndex = y * _rowStride;
            _pixelData[xIndex + yIndex] = color.R;
            _pixelData[xIndex + yIndex + 1] = color.G;
            _pixelData[xIndex + yIndex + 2] = color.B;

        }
    }
}