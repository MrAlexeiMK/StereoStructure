using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace StereoStructure
{
    static class Correspondences
    {
        private static CorrespondencesWindow correspondencesWindow;
        private static List<Pair<int, int>> frames;
        private static List<Point> points;

        public static int frameStep = 1;
        public static int pointsCount = 10;

        public static void Clear()
        {
            frames = new List<Pair<int, int>>();
            points = new List<Point>();
        }

        public static void Add(ref Bitmap image)
        {
            points = new List<Point>();
            Matrix I = new Matrix(image, ColorType.GRAY);

            //Operations with frame
            switch(SettingsListener.Get().rotationImages)
            {
                case Rotation.ROTATION_90:
                    I.Rotate(Rotation.ROTATION_90);
                    break;
                case Rotation.ROTATION_180:
                    I.Rotate(Rotation.ROTATION_180);
                    break;
                case Rotation.ROTATION_270:
                    I.Rotate(Rotation.ROTATION_270);
                    break;
            }

            image = ToBitmap(I);

            I.ConvertByKernel(new Matrix(OperatorType.SOBEL_X), 1);
            I.ConvertByKernel(new Matrix(OperatorType.SOBEL_Y), 1);
            I.ConvertByMedianFilter(7);


            //Operations to find special points
            List<List<Matrix>> pyramid = new List<List<Matrix>>();
            Matrix A, B;
            for (int scale = 1; scale <= SettingsListener.Get().siftScalesCount; ++scale)
            {
                double mult = 1.0 / Math.Pow(2, scale-1);
                List<Matrix> DoG = new List<Matrix>();
                Matrix scaled = MatrixExtractor.GetScale(new Matrix(I), mult);
                A = scaled.Clone();
                for(double k = SettingsListener.Get().siftSigmaMin; k <= SettingsListener.Get().siftSigmaMax; 
                    k+=SettingsListener.Get().siftSigmaStep)
                {
                    B = scaled.Clone();
                    B.ConvertByKernel(new Matrix(OperatorType.GAUSS_1D_X, k * mult), PaddingFill.BY_MEDIAN);
                    B.ConvertByKernel(new Matrix(OperatorType.GAUSS_1D_Y, k * mult), PaddingFill.BY_MEDIAN);
                    DoG.Add(MatrixExtractor.GetMinus(B, A));
                    A = B;
                }
                pyramid.Add(DoG);
            }

            int scales = pyramid.Count;
            int octaves = pyramid[0].Count;

            if (scales - 2 <= 1 || octaves <= 0) throw new Exception("We don't have enough scales or octaves");

            for(int j = 0; j < octaves; ++j)
            {
                for(int i = 1; i < scales-1; ++i)
                {
                    AddPoints(pyramid[i][j], pyramid[i+1][j], pyramid[i-1][j], Math.Pow(2, i+1));
                }
            }

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
        }

        private static void AddPoints(Matrix im, Matrix imUp, Matrix imDown, double mult)
        {
            for(int y = 1; y < im.N-1; ++y)
            {
                for(int x = 1; x < im.M-1; ++x)
                {
                    double val = im.data[y, x];
                    bool c1 = CheckPoint(true, val, x, y, im) && 
                        CheckPoint(true, val, x, y, imUp, 0.5) && 
                        CheckPoint(true, val, x, y, imDown, 2);
                    bool c2 = CheckPoint(false, val, x, y, im) &&
                        CheckPoint(false, val, x, y, imUp, 0.5) &&
                        CheckPoint(false, val, x, y, imDown, 2);
                    if (c1 || c2)
                    {
                        points.Add(new Point(x * mult, y * mult));
                    }
                }
            }
        }

        private static bool CheckPoint(bool max, double val, int x, int y, Matrix im, double mult = 1)
        {
            for (int v1 = -1; v1 <= 1; ++v1)
            {
                for (int v2 = -1; v2 <= 1; ++v2)
                {
                    if (mult == 1 && v1 == 0 && v2 == 0) continue;
                    double v = im.data[(int)((y + v1)*mult), (int)((x + v2)*mult)];
                    if (max && val <= v) return false;
                    if (!max && val >= v) return false;
                }
            }
            return true;
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
                    int val = Correct((int)B.Get(y, x)) +
                        Correct((int)G.Get(y, x) << 8) +
                        Correct((int)R.Get(y, x) << 16) + 
                        (255 << 24);
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
                    int val = Correct((int)I.Get(y, x));
                    bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(val, val, val));
                }
            }
            return bmp;
        }

        private static int Correct(int pixel)
        {
            return Math.Min(255, Math.Max(0, pixel));
        }

        public static int GetFramesCount()
        {
            return frames.Count;
        }
        public static Bitmap ToGrayscale(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
                     new float[] {.3f, .3f, .3f, 0, 0},
                     new float[] {.59f, .59f, .59f, 0, 0},
                     new float[] {.11f, .11f, .11f, 0, 0},
                     new float[] {0, 0, 0, 1, 0},
                     new float[] {0, 0, 0, 0, 1}
                   });
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
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
