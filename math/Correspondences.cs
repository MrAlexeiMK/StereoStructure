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
        private static List<List<Point>> points;

        public static void Clear()
        {
            frames = new List<Pair<int, int>>();
            points = new List<List<Point>>();
        }
        public static void Add(ref Bitmap image)
        {
            Matrix I = new Matrix(image, ColorType.GRAY);

            if (I.M <= 0) throw new Exception("Image size is too low");

            if (SettingsListener.Get().applyMedianFilterOnLoad)
            {
                Logs.WriteMainThread("Applying median filter to image...");
                I.ConvertByMedianFilter(SettingsListener.Get().medianFilterSize);
                Logs.WriteMainThread("Median filter applied successfully");
            }

            I.Resize(SettingsListener.Get().imageWidth);

            switch (SettingsListener.Get().rotationImages)
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

            KeyPoints keyPoints = null;

            switch (SettingsListener.Get().cAlg)
            {
                case CorrespondencesAlg.SIFT:
                    keyPoints = new SIFT(I);
                    break;
                case CorrespondencesAlg.ORB:
                    keyPoints = new ORB(I);
                    break;
                case CorrespondencesAlg.FAST:
                    keyPoints = new FAST(I);
                    break;
            }

            keyPoints.Compute();

            points.Add(keyPoints.GetPoints());
        }

        public static void Compute()
        {
            for(int index = 0; index < frames.Count; ++index)
            {
                int im1 = frames[index].first;
                int im2 = frames[index].second;
                Bitmap leftImageCopy = null, rightImageCopy = null;
                using (Bitmap leftImage = new Bitmap(BitmapPath(im1)))
                {
                    using (Bitmap rightImage = new Bitmap(BitmapPath(im2)))
                    {
                        List<Point> left = points[im1];
                        List<Point> right = points[im2];

                        Descriptor descriptor = null;
                        switch(SettingsListener.Get().dAlg)
                        {
                            case DescriptorAlg.BRIEF:
                                descriptor = new BRIEF(left, right);
                                break;
                        }

                        descriptor.Compute();

                        List<Pair<int, int>> pairs = descriptor.GetPairs();


                        foreach (Pair<int, int> pair in pairs)
                        {
                            Point p1 = left[pair.first];
                            Point p2 = right[pair.second];
                            DrawPoint(leftImage, rightImage, p1, p2);
                        }
                        leftImageCopy = new Bitmap(leftImage);
                        rightImageCopy = new Bitmap(rightImage);
                    }
                }
                leftImageCopy.Save(BitmapPath(im1));
                rightImageCopy.Save(BitmapPath(im2));
            }
        }

        private static void DrawPoint(Bitmap image1, Bitmap image2, Point p1, Point p2)
        {
            Random rnd = new Random();
            using (Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(rnd.Next(0, 256),
                rnd.Next(0, 256), rnd.Next(0, 256))))
            {
                using (Graphics g = Graphics.FromImage(image1))
                {
                    g.FillEllipse(brush, (int)Math.Round(p1.x) -
                            SettingsListener.Get().circleWidth, (int)Math.Round(p1.y) - SettingsListener.Get().circleWidth,
                            2 * SettingsListener.Get().circleWidth, 2 * SettingsListener.Get().circleWidth);
                }
                using (Graphics g = Graphics.FromImage(image2))
                {
                    g.FillEllipse(brush, (int)Math.Round(p2.x) -
                            SettingsListener.Get().circleWidth, (int)Math.Round(p2.y) - SettingsListener.Get().circleWidth,
                            2 * SettingsListener.Get().circleWidth, 2 * SettingsListener.Get().circleWidth);
                }
            }
        }

        private static void DrawPoints(Bitmap image, List<Point> points)
        {
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (Point p in points)
                {
                    using (Brush brush = new SolidBrush(System.Drawing.Color.Yellow))
                    {
                        g.FillEllipse(brush, (int)Math.Round(p.x) -
                            SettingsListener.Get().circleWidth, (int)Math.Round(p.y) - SettingsListener.Get().circleWidth,
                            2 * SettingsListener.Get().circleWidth, 2 * SettingsListener.Get().circleWidth);
                    }
                }
            }
        }

        public static void Init(int framesCount)
        {
            for(int i = 0; i < framesCount - 1; ++i)
            {
                for(int j = i + 1; j < i + 1 + SettingsListener.Get().framesStep && j < framesCount; ++j)
                {
                    frames.Add(new Pair<int, int>(i, j));
                }
            }
        }

        private static string BitmapPath(int index) {
            return SettingsListener.GetPath() + "frames\\frame_" + index + ".jpg";
        }

        public static BitmapImage GetLeft(int index)
        {
            BitmapImage left = new BitmapImage();
            using (var fs = new FileStream(BitmapPath(frames[index-1].first), FileMode.Open))
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
            using (var fs = new FileStream(BitmapPath(frames[index - 1].second), FileMode.Open))
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
