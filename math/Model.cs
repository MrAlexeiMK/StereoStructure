using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
namespace StereoStructure
{
    public class Model
    {
        private List<Pair<Color, int>> colors;
        private List<Polygon> polygons;
        private List<Point3D> nodes;
        private string colorsFile;

        public Model()
        {
            polygons = new List<Polygon>();
            nodes = new List<Point3D>();
            colors = new List<Pair<Color, int>>();
            
        }
        public List<Polygon> GetPolygons()
        {
            return polygons;
        }
        public List<Point3D> GetNodes()
        {
            return nodes;
        }
        public List<Pair<Color, int>> getColors()
        {
            return colors;
        }
        public string GetColorsFile()
        {
            return colorsFile;
        }
        public bool IsLoaded()
        {
            return nodes != null && nodes.Count != 0;
        }
        public void ApplyOperator(Matrix A)
        {
            Point3D center = new Point3D(0, 0, 0);
            double yMin = Double.MaxValue;
            for (int i = 0; i < nodes.Count; ++i)
            {
                Point3D point = nodes[i];
                Matrix v = new Matrix(point);
                v = MatrixExtractor.GetMultiply(A, v);
                point.x = v.Get(0); point.y = v.Get(1); point.z = v.Get(2);
                yMin = Math.Min(yMin, point.y);
                center.Sum(point);
                nodes[i] = point;
            }
            center.Mult(1.0/nodes.Count);
            yMin -= center.y;
            ApplySum(-center.x, -center.y-yMin, -center.z);
        }
        public void ApplyMultiple(double xMult, double yMult, double zMult)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                Point3D point = nodes[i];
                point.x *= xMult;
                point.y *= yMult;
                point.z *= zMult;
                nodes[i] = point;
            }
        }
        public void ApplySum(double xShift, double yShift, double zShift)
        {
            for (int i = 0; i < nodes.Count; ++i)
            {
                Point3D point = nodes[i];
                point.x += xShift;
                point.y += yShift;
                point.z += zShift;
                nodes[i] = point;
            }
        }
        public void Save(string path)
        {
            string[] spl = path.Split('.');
            string[] splBySlesh = path.Split('\\');
            if (spl[spl.Length - 1] == "obj")
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    if (colorsFile == null) colorsFile = splBySlesh[splBySlesh.Length - 1].Split('.')[0] + ".mtl";
                    using (StreamWriter mtlWriter = new StreamWriter(path.Replace(splBySlesh[splBySlesh.Length - 1], colorsFile)))
                    {
                        for(int i = 0; i < colors.Count; ++i)
                        {
                            Color color = colors[i].first;
                            mtlWriter.WriteLine("newmtl "+i);
                            mtlWriter.WriteLine("\tKd " + color.R/255.0 + " " + color.G/255.0 + " " + color.B/255.0);
                        }
                    }
                    writer.WriteLine("mtllib " + colorsFile);
                    foreach(Point3D point in nodes)
                    {
                        writer.WriteLine("v " + point.x + " " + point.z + " " + point.y);
                    }
                    int colorKey = 0;
                    int lastIndex = 0;
                    for(int i = 0; i < polygons.Count; ++i)
                    {
                        if(i >= lastIndex)
                        { 
                            writer.WriteLine("usemtl " + colorKey);
                            lastIndex += colors[colorKey].second;
                            ++colorKey;
                        }
                        writer.Write("f ");
                        List<int> indexes = polygons[i].indexes;
                        for (int j = 0; j < indexes.Count-1; ++j)
                        {
                            writer.Write(indexes[j] + " ");
                        }
                        writer.WriteLine(indexes[indexes.Count-1]);
                    }
                }
            }
            else throw new Exception("Not .obj file");
        }
        
        public void Add(string path)
        {
            if (File.Exists(path))
            {
                string[] spl = path.Split('.');
                if (spl[spl.Length - 1] == "obj")
                {
                    string[] lines = File.ReadAllLines(path);
                    string key;
                    bool mtlExist = false;
                    Dictionary<string, Color> colorByKey = new Dictionary<string, Color>();
                    int last = -1;
                    int count = nodes.Count;
                    double yMin = Double.MaxValue;
                    Point3D center = new Point3D(0, 0, 0);
                    foreach (string line in lines)
                    {
                        if (!line.Contains("#"))
                        {
                            string[] splitted = line.Replace("  ", " ").Split(' ');
                            if (splitted[0] == "mtllib")
                            {
                                string[] pathSplit = path.Split('\\');
                                string mtlPath = path;
                                mtlPath = mtlPath.Replace(pathSplit[pathSplit.Length - 1], splitted[1]);
                                if (File.Exists(mtlPath))
                                {
                                    mtlExist = true;
                                    if(colorsFile == null) colorsFile = splitted[1];
                                    string[] mtlLines = File.ReadAllLines(mtlPath);
                                    string currentKey = "";
                                    foreach (string mtlLine in mtlLines)
                                    {
                                        if (!mtlLine.Contains("#"))
                                        {
                                            string[] mtlSplitted = mtlLine.Split(' ');
                                            if (mtlSplitted[0] == "newmtl")
                                            {
                                                currentKey = mtlSplitted[1];
                                            }
                                            else if (mtlSplitted[0].Contains("Kd"))
                                            {
                                                byte R = (byte)(255 * Double.Parse(mtlSplitted[1]));
                                                byte G = (byte)(255 * Double.Parse(mtlSplitted[2]));
                                                byte B = (byte)(255 * Double.Parse(mtlSplitted[3]));
                                                Color color = new Color(R, G, B);
                                                colorByKey.Add(currentKey, color);
                                            }
                                        }
                                    }
                                }
                            }
                            else if (splitted[0] == "v")
                            {
                                double x = Double.Parse(splitted[1]);
                                double y = Double.Parse(splitted[3]);
                                double z = Double.Parse(splitted[2]);
                                Point3D point = new Point3D(x, y, z);
                                center.Sum(point);
                                yMin = Math.Min(yMin, point.y);
                                nodes.Add(point);
                            }
                            else if (splitted[0] == "usemtl")
                            {
                                if (mtlExist)
                                {
                                    key = splitted[1];
                                    ++last;
                                    if (!colorByKey.ContainsKey(key)) throw new Exception("Color " + key + " doesn't exist");
                                    colors.Add(new Pair<Color, int>(colorByKey[key], 0));
                                }
                            }
                            else if (splitted[0] == "f")
                            {
                                Polygon polygon = new Polygon();
                                if (last == -1)
                                {
                                    colors.Add(new Pair<Color, int>(SettingsListener.Get().defaultColor, 0));
                                    ++last;
                                }
                                colors[last].second++;

                                for (int i = 1; i < splitted.Length; ++i)
                                {
                                    string s = splitted[i].Split('/')[0];
                                    if (s != "") polygon.AddIndex(count+Int32.Parse(s));
                                }
                                polygons.Add(polygon);
                            }
                        }
                    }

                    //Shift points to mid
                    center.Mult(1.0/nodes.Count);
                    Logs.WriteMainThread("Center of Model: ("+center.x+", "+center.y+", "+center.z+")");
                    ApplySum(-center.x, -center.y, -center.z);
                    Logs.WriteMainThread("Model was centered to (0, 0, 0)");

                    //Scale points
                    double maxWidth = 0;
                    foreach (Point3D p in nodes)
                    {
                        if (p.x > maxWidth) maxWidth = p.x;
                        if (p.y > maxWidth) maxWidth = p.y;
                        if (p.z > maxWidth) maxWidth = p.z;
                    }
                    if (maxWidth > 0)
                    {
                        double K = SettingsListener.Get().maxWidth / maxWidth;
                        ApplyMultiple(K, K, K);
                        Logs.WriteMainThread("Model was scaled by " + K);

                        //Shift points above axis y = 0
                        yMin -= center.y;
                        yMin *= K;
                        ApplySum(0, -yMin, 0);
                        Logs.WriteMainThread("Model was raised on " + (-yMin));
                    }
                }
                else throw new Exception("Not .obj file");

            }
            else throw new Exception(path + " doesn't exist");
        }
        public void Load(string path)
        {
            polygons = new List<Polygon>();
            nodes = new List<Point3D>();
            colors = new List<Pair<Color, int>>();
            colorsFile = null;
            Add(path);
        }
    }
}
