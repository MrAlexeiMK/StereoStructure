using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace StereoStructure
{
    static class Correspondences
    {
        private static CorrespondencesWindow correspondencesWindow;
        private static List<Pair<int, int>> frames;
        private static List<List<Point>> specialPoints;
        private static Matrix GAUSS_X, GAUSS_Y;

        public static int frameStep = 1;
        public static int pointsCount = 10;

        public static void Clear()
        {
            frames = new List<Pair<int, int>>();
            specialPoints = new List<List<Point>>();
            int k = (int)(SettingsListener.Get().gaussSigma * 6);
            GAUSS_X = new Matrix(OperatorType.GAUSS_1D_X, SettingsListener.Get().gaussSigma, k % 2 == 0 ? k + 1 : k);
            GAUSS_Y = new Matrix(OperatorType.GAUSS_1D_Y, SettingsListener.Get().gaussSigma, k % 2 == 0 ? k + 1 : k);
        }

        public static void Add(ref Bitmap image)
        {
            Matrix R = new Matrix(image, ColorType.R);
            Matrix G = new Matrix(image, ColorType.G);
            Matrix B = new Matrix(image, ColorType.B);
            List<Point> points = GetPoints(R, G, B);
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (Point p in points)
                {
                    using (Brush brush = new SolidBrush(System.Drawing.Color.Yellow))
                    {
                        g.FillEllipse(brush, ((int)p.x - 
                            SettingsListener.Get().circleWidth) / 2, ((int)p.y - SettingsListener.Get().circleWidth) / 2,
                            SettingsListener.Get().circleWidth, SettingsListener.Get().circleWidth);
                    }
                }
            }

            R.ConvertByKernel(GAUSS_X, (GAUSS_X.M - 1) / 2, 0, PaddingFill.BY_MEDIAN);
            R.ConvertByKernel(GAUSS_Y, 0, (GAUSS_Y.N - 1) / 2, PaddingFill.BY_MEDIAN);
            G.ConvertByKernel(GAUSS_X, (GAUSS_X.M - 1) / 2, 0, PaddingFill.BY_MEDIAN);
            G.ConvertByKernel(GAUSS_Y, 0, (GAUSS_Y.N - 1) / 2, PaddingFill.BY_MEDIAN);
            B.ConvertByKernel(GAUSS_X, (GAUSS_X.M - 1) / 2, 0, PaddingFill.BY_MEDIAN);
            B.ConvertByKernel(GAUSS_Y, 0, (GAUSS_Y.N - 1) / 2, PaddingFill.BY_MEDIAN);

            image = ToBitmap(R, G, B);
            specialPoints.Add(points);
        }

        public static List<Point> GetPoints(Matrix R, Matrix G, Matrix B)
        {
            List<Point> points = new List<Point>();
            
            return points;
        }

        public static void Init(int framesCount)
        {
            switch (SettingsListener.Get().accuracy)
            {
                case ACCURACY.LOW:
                    frameStep = 1;
                    pointsCount = 10;
                    break;
                case ACCURACY.MEDIUM:
                    frameStep = 2;
                    pointsCount = 15;
                    break;
                case ACCURACY.HIGH:
                    frameStep = 3;
                    pointsCount = 20;
                    break;
            }
            for(int i = 0; i < framesCount - 1; ++i)
            {
                for(int j = i+1; j < i + 1 + frameStep && j < framesCount; ++j)
                {
                    frames.Add(new Pair<int, int>(i, j));
                }
            }
        }

        public static BitmapImage GetLeft(int index)
        {
            BitmapImage left = new BitmapImage();
            using (var fs = new FileStream(SettingsListener.GetPath() + "frames\\frame_" + frames[index - 1].first + ".jpg", FileMode.Open))
            {
                left.BeginInit();
                left.CacheOption = BitmapCacheOption.OnLoad;
                left.StreamSource = fs;
                left.EndInit();
                left.Freeze();
            }
            return left;
        }

        public static BitmapImage GetRight(int index)
        {
            BitmapImage right = new BitmapImage();
            using (var fs = new FileStream(SettingsListener.GetPath() + "frames\\frame_" + frames[index - 1].second + ".jpg", FileMode.Open))
            {
                right.BeginInit();
                right.CacheOption = BitmapCacheOption.OnLoad;
                right.StreamSource = fs;
                right.EndInit();
                right.Freeze();
            }
            return right;
        }

        public static Bitmap ToBitmap(Matrix R, Matrix G, Matrix B)
        {
            if (R.N != G.N || G.N != B.N || R.M != G.M || G.M != B.M) throw new Exception("Different dimensions");
            Bitmap bmp = new Bitmap(R.M, R.N);
            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    int val = ((int)B.Get(y, x)) + ((int)G.Get(y, x) << 8) + ((int)R.Get(y, x) << 16) + (255 << 24);
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(val));
                }
            }
            return bmp;
        }

        public static Bitmap ToBitmap(Matrix I)
        {
            Bitmap bmp = new Bitmap(I.M, I.N);
            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb((int)I.Get(y, x)));
                }
            }
            return bmp;
        }

        public static int GetFramesCount()
        {
            return frames.Count;
        }

        public static void Show()
        {
            correspondencesWindow = new CorrespondencesWindow();
            correspondencesWindow.Show();
        }

        public static void Close()
        {
            correspondencesWindow.Close();
        }

        public static bool IsOpened()
        {
            if (correspondencesWindow == null) return false;
            return correspondencesWindow.IsLoaded;
        }
    }
}
