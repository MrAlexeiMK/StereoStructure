using System;
using System.Collections.Generic;

namespace StereoStructure
{
    public class Polygon
    {
        public List<int> indexes;
        public Polygon()
        {
            indexes = new List<int>();
        }
        public void AddIndex(int v)
        {
            indexes.Add(v);
        }
    }
    public struct Color
    {
        public Color(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public override string ToString()
        {
            return R + "," + G + "," + B;
        }
        public byte R;
        public byte G;
        public byte B;
    }
    public struct Point3D
    {
        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public void Sum(double val)
        {
            x += val;
            y += val;
            z += val;
        }
        public void Sum(Point3D p)
        {
            x += p.x;
            y += p.y;
            z += p.z;
        }
        public void Mult(double val)
        {
            x *= val;
            y *= val;
            z *= val;
        }
        public double x;
        public double y;
        public double z;
    }
    public struct Point
    {
        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double x;
        public double y;
    }
    public class Triple<T>
    {
        public T first, second, third;
        public Triple(T first, T second, T third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }
    public class Pair<T, V>
    {
        public T first;
        public V second;
        public Pair(T first, V second)
        {
            this.first = first;
            this.second = second;
        }
    }
    public enum LANG
    {
        RU,
        EN
    }
    public enum ACCURACY
    {
        LOW,
        MEDIUM,
        HIGH
    }
    public enum OperatorType
    {
        ROTATION_2D,
        ROTATION_3D_X,
        ROTATION_3D_Y,
        ROTATION_3D_Z,
        GAUSS_2D,
        GAUSS_1D_X,
        GAUSS_1D_Y
    }
    public enum ColorType
    {
        R,
        G,
        B
    }
    public enum PaddingFill
    {
        BY_ZEROES,
        BY_MEDIAN
    }
    enum LogType
    {
        INFO,
        WARNING,
        ERROR
    }
    static class Constants
    {
        public static string videoFormats = "All Videos Files |*.dat; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; " +
              " *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm";
        public static string modelFormtats = "3D Objects |*.obj";
        public static List<OperatorType> operatorTypes = new List<OperatorType> { 
            OperatorType.ROTATION_3D_X,
            OperatorType.ROTATION_3D_Y
        };
        public static double Gauss(int x, int y, double k)
        {
            return 1.0 / (2 * Math.PI * k*k) * Math.Exp(-(x*x + y*y) / (2*k*k));
        }
        public static double Gauss1D(int i, double k)
        {
            return 1.0 / (Math.Sqrt(2 * Math.PI) * k) * Math.Exp(-(i*i) / (2 * k * k));
        }
    }
}
