using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StereoStructure
{
    [Serializable]
    public class Matrix
    {
        public int N, M;
        private double[,] data;
        private static Random rand = new Random();
        public Matrix(in Matrix a)
        {
            N = a.N;
            M = a.M;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = a.data[i, j];
                }
            }
        }
        public Matrix(int N, int M)
        {
            this.N = N;
            this.M = M;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = 0;
                }
            }
        }
        public Matrix(int N, int M, double value)
        {
            this.N = N;
            this.M = M;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = value;
                }
            }
        }
        public Matrix(List<double> v)
        {
            N = v.Count();
            M = 1;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                data[i, 0] = v[i];
            }
        }

        public Matrix(Point3D p)
        {
            N = 3;
            M = 1;
            data = new double[N, M];
            data[0, 0] = p.x;
            data[1, 0] = p.y;
            data[2, 0] = p.z;
        }

        public Matrix(Point p)
        {
            N = 2;
            M = 1;
            data = new double[N, M];
            data[0, 0] = p.x;
            data[1, 0] = p.y;
        }

        public Matrix(string reg)
        {
            string[] spl = reg.Split('|');
            N = spl.Length;
            M = spl[0].Split(',').Length;
            data = new double[N, M];
            for(int y = 0; y < N; ++y)
            {
                string[] spl2 = spl[y].Split(',');
                if (M != spl2.Length) throw new Exception("Incorrect Matrix format");
                for(int x = 0; x < M; ++x)
                {
                    try
                    {
                        double val = Double.Parse(spl2[x]);
                        data[y, x] = val;
                    } catch
                    {
                        throw new Exception("Incorrect double format");
                    }
                }
            }
        }

        public Matrix(Bitmap frame, ColorType color)
        {
            N = frame.Height;
            M = frame.Width;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    int pixel = frame.GetPixel(j, i).R;
                    if (color == ColorType.G) pixel = frame.GetPixel(j, i).G;
                    else if (color == ColorType.B) pixel = frame.GetPixel(j, i).B;
                    data[i, j] = pixel;
                }
            }
        }

        public Matrix(Bitmap frame)
        {
            N = frame.Height;
            M = frame.Width;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = frame.GetPixel(j, i).ToArgb();
                }
            }
        }

        public Matrix(int n, int m, double from, double to)
        {
            N = n;
            M = m;
            data = new double[N, M];
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = from + rand.NextDouble() * (to - from);
                }
            }
        }

        public Matrix(OperatorType operatorType, double arg = 1.0, int arg2 = 3)
        {
            double sum = 0;
            int cX, cY;
            switch (operatorType)
            {
                case OperatorType.ROTATION_2D:
                    N = 2;
                    M = 2;
                    data = new double[N, M];
                    data[0, 0] = Math.Cos(arg * Math.PI / 180);
                    data[0, 1] = -Math.Sin(arg * Math.PI / 180);
                    data[1, 0] = Math.Sin(arg * Math.PI / 180);
                    data[1, 1] = Math.Cos(arg * Math.PI / 180);
                    break;
                case OperatorType.ROTATION_3D_X:
                    N = 3;
                    M = 3;
                    data = new double[N, M];
                    data[0, 0] = 1;
                    data[0, 1] = 0; data[0, 2] = 0;
                    data[1, 0] = 0; data[2, 0] = 0;
                    data[1, 1] = Math.Cos(arg * Math.PI / 180);
                    data[1, 2] = -Math.Sin(arg * Math.PI / 180);
                    data[2, 1] = Math.Sin(arg * Math.PI / 180);
                    data[2, 2] = Math.Cos(arg * Math.PI / 180);
                    break;
                case OperatorType.ROTATION_3D_Y:
                    N = 3;
                    M = 3;
                    data = new double[N, M];
                    data[1, 1] = 1;
                    data[0, 1] = 0; data[1, 2] = 0;
                    data[1, 0] = 0; data[2, 1] = 0;
                    data[0, 0] = Math.Cos(arg * Math.PI / 180);
                    data[2, 0] = -Math.Sin(arg * Math.PI / 180);
                    data[0, 2] = Math.Sin(arg * Math.PI / 180);
                    data[2, 2] = Math.Cos(arg * Math.PI / 180);
                    break;
                case OperatorType.ROTATION_3D_Z:
                    N = 3;
                    M = 3;
                    data = new double[N, M];
                    data[2, 2] = 1;
                    data[0, 2] = 0; data[1, 2] = 0;
                    data[2, 0] = 0; data[2, 1] = 0;
                    data[0, 0] = Math.Cos(arg * Math.PI / 180);
                    data[0, 1] = -Math.Sin(arg * Math.PI / 180);
                    data[1, 0] = Math.Sin(arg * Math.PI / 180);
                    data[1, 1] = Math.Cos(arg * Math.PI / 180);
                    break;
                case OperatorType.GAUSS_2D:
                    if (arg2 % 2 == 0) throw new Exception("Gauss matrix dimension should be odd");
                    N = arg2;
                    M = arg2;
                    data = new double[N, M];
                    sum = 0;
                    cX = M / 2; cY = N / 2;
                    for(int y = 0; y < N; ++y)
                    {
                        for(int x = 0; x < M; ++x)
                        {
                            data[y, x] = Constants.Gauss(x-cX, y-cY, arg);
                            sum += data[y, x];
                        }
                    }
                    for(int y = 0; y < N; ++y)
                    {
                        for(int x = 0; x < M; ++x)
                        {
                            data[y, x] /= sum;
                        }
                    }
                    break;
                case OperatorType.GAUSS_1D_X:
                    if (arg2 % 2 == 0) throw new Exception("Gauss matrix dimension should be odd");
                    N = 1;
                    M = arg2;
                    data = new double[N, M];
                    sum = 0;
                    cX = M / 2;
                    for (int x = 0; x < M; ++x)
                    {
                        data[0, x] = Constants.Gauss1D(x - cX, arg);
                        sum += data[0, x];
                    }
                    for (int x = 0; x < M; ++x)
                    {
                        data[0, x] /= sum;
                    }
                    break;
                case OperatorType.GAUSS_1D_Y:
                    if (arg2 % 2 == 0) throw new Exception("Gauss matrix dimension should be odd");
                    N = arg2;
                    M = 1;
                    data = new double[N, M];
                    sum = 0;
                    cY = N / 2;
                    for (int y = 0; y < N; ++y)
                    {
                        data[y, 0] = Constants.Gauss1D(y - cY, arg);
                        sum += data[y, 0];
                    }
                    for (int y = 0; y < N; ++y)
                    {
                        data[y, 0] /= sum;
                    }
                    break;
            }
        }

        public Matrix()
        {
            N = 1;
            M = 1;
            data = new double[N, M];
            data[0, 0] = 1;
        }

        public double Get(int index)
        {
            return data[index, 0];
        }

        public double Get(int y, int x)
        {
            return data[y, x];
        }

        public void Set(int y, int x, double val)
        {
            data[y, x] = val;
        }

        public bool isVector()
        {
            return M == 1;
        }

        public List<double> ToList()
        {
            List<double> res = new List<double>();
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    res.Add(data[i, j]);
                }
            }
            return res;
        }

        public void Assign(ref Matrix m)
        {
            N = m.N;
            M = m.M;
            data = new double[N, M];
            for (int i = 0; i < m.N; ++i)
            {
                for (int j = 0; j < m.M; ++j)
                {
                    data[i, j] = m.Get(i, j);
                }
            }
        }

        public void Replace(int xStart, int yStart, Matrix m)
        {
            if (m.M + xStart > M || m.N + yStart > N) throw new Exception("Can't replace (different dimensions)");
            for (int x = 0; x < m.M; ++x)
            {
                for (int y = 0; y < m.N; ++y)
                {
                    Set(y + yStart, x + xStart, m.Get(y, x));
                }
            }
        }

        public void Replace(int xStart, int yStart, int width, int height, double value)
        {
            if (width + xStart > M || height + yStart > N) throw new Exception("Can't replace (different dimensions)");
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Set(y + yStart, x + xStart, value);
                }
            }
        }

        public void SwapLines(int i1, int i2)
        {
            double temp;
            for (int i = 0; i < M; ++i)
            {
                temp = data[i1, i];
                data[i1, i] = data[i2, i];
                data[i2, i] = temp;
            }
        }

        public Matrix Transpose()
        {
            Matrix temp = new Matrix(M, N);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    temp.data[j, i] = data[i, j];
                }
            }
            return temp;
        }

        public int ToTriangular(bool down = true)
        {
            int swaps = 0;
            int size = M < N ? M : N;
            double l1, l2;
            if (down)
            {
                for (int L1 = 0; L1 < size; ++L1)
                {
                    for (int L2 = L1 + 1; L2 < N; ++L2)
                    {
                        l1 = data[L1, L1];
                        l2 = data[L2, L1];

                        if (l2 == 0) continue;
                        else if (l1 == 0)
                        {
                            SwapLines(L1, L2);
                            ++swaps;
                            l1 = data[L1, L1];
                            l2 = data[L2, L1];
                        }
                        double x = -(l2 / l1);

                        for (int i = L1; i < M; ++i)
                        {
                            if (i == L1) data[L2, i] = 0;
                            else data[L2, i] = data[L2, i] + data[L1, i] * x;
                        }
                    }
                }
            }
            else
            {
                for (int L1 = 1; L1 <= size - 1; ++L1)
                {
                    for (int L2 = L1 - 1; L2 >= 0; --L2)
                    {
                        l1 = data[L1, L1];
                        l2 = data[L2, L1];

                        if (l2 == 0) continue;
                        else if (l1 == 0)
                        {
                            SwapLines(L1, L2);
                            ++swaps;
                            l1 = data[L1, L1];
                            l2 = data[L1, L2];
                        }
                        double x = -(l2 / l1);

                        for (int i = L1; i < M; ++i)
                        {
                            if (i == L1) data[L2, i] = 0;
                            else data[L2, i] = data[L2, i] + data[L1, i] * x;
                        }
                    }
                }
            }
            return swaps;
        }
        public double Determinant()
        {
            int swaps = ToTriangular();
            double ans = data[0, 0];
            for (int i = 1; i < N; ++i)
            {
                ans = ans * data[i, i];
            }
            return (swaps % 2 == 0) ? ans : -ans;
        }

        public void ToUnit()
        {
            int size = M < N ? M : N;
            ToTriangular();
            ToTriangular(false);
            for (int i = 0; i < size; ++i)
            {
                double x = data[i, i];
                for (int j = 0; j < M; ++j)
                {
                    data[i, j] = data[i, j] / x;
                }
            }
        }

        public Matrix SubMatrix(int fromCol, int toCol)
        {
            int cols = toCol - fromCol + 1;
            Matrix temp = new Matrix(N, cols);
            for (int i = 0; i < N; ++i)
            {
                for (int j = fromCol - 1; j < toCol; ++j)
                {
                    temp.data[i, j - fromCol + 1] = data[i, j];
                }
            }
            return temp;
        }

        public Matrix Unite(ref Matrix A)
        {
            if (N == A.N)
            {
                Matrix temp = new Matrix(N, M + A.M);
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < M; ++j)
                    {
                        temp.data[i, j] = data[i, j];
                    }
                }
                for (int i = 0; i < N; ++i)
                {
                    for (int j = M; j < M + A.M; ++j)
                    {
                        temp.data[i, j] = A.data[i, j - M];
                    }
                }
                return temp;
            }
            else
            {
                throw new Exception("Invalid matrix dimensions");
            }
        }

        public Matrix Invserse()
        {
            if (N == M)
            {
                Matrix E = new Matrix(N, N);
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < M; ++j)
                    {
                        if (i != j) E.data[i, j] = 0;
                        else E.data[i, j] = 1;
                    }
                }
                Matrix A = Unite(ref E);
                A.ToUnit();
                for (int i = 0; i < A.N; ++i)
                {
                    if (A.data[i, i] == 0) throw new Exception("Determinant = 0");
                }
                return A.SubMatrix(N + 1, 2 * N);
            }
            else
            {
                throw new Exception("Invalid matrix dimensions");
            }
        }

        public string ToString(int digits = 0)
        {
            string res = M + " " + N + "\n";
            for (int y = 0; y < N; ++y)
            {
                res += "|";
                for (int x = 0; x < M; ++x)
                {
                    if (digits > 0) res += Math.Round(data[y, x] * Math.Pow(10, digits)) / Math.Pow(10.0, digits) + "|";
                    else res += data[y, x] + "|";
                }
                res += "\n";
            }
            return res;
        }

        public void Print(bool round = true)
        {
            Console.WriteLine("(" + M + ";" + N + "):");
            for (int y = 0; y < N; ++y)
            {
                for (int x = 0; x < M; ++x)
                {
                    if (!round)
                    {
                        Console.Write(data[y, x] + "|");
                    }
                    else Console.Write(Math.Round(data[y, x] * 1000.0) / 1000.0 + "|");
                }
                Console.WriteLine();
            }
        }

        public Matrix GetNegative()
        {
            Matrix res = new Matrix(N, M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    res.Set(i, j, -Get(i, j));
                }
            }
            return res;
        }

        public void RemoveColumn(int index)
        {
            Matrix m = new Matrix(N, M - 1);
            int x = 0;
            int shift = 0;
            while (x < M)
            {
                if (x == index) shift = -1;
                else
                {
                    for (int y = 0; y < N; ++y)
                    {
                        m.Set(y, x + shift, Get(y, x));
                    }
                }
                ++x;
            }
            Assign(ref m);
        }

        public void RemoveRow(int index)
        {
            Matrix m = new Matrix(N - 1, M);
            int y = 0;
            int shift = 0;
            while (y < N)
            {
                if (y == index) shift = -1;
                else
                {
                    for (int x = 0; x < M; ++x)
                    {
                        m.Set(y + shift, x, Get(y, x));
                    }
                }
                ++y;
            }
            Assign(ref m);
        }

        public void JoinBottom(Matrix m)
        {
            if (M == m.M)
            {
                double[,] temp = new double[N + m.N, M];
                for (int y = 0; y < m.N; ++y)
                {
                    for (int x = 0; x < m.M; ++x)
                    {
                        temp[y + N, x] = m.Get(y, x);
                    }

                }
                for (int y = 0; y < N; ++y)
                {
                    for (int x = 0; x < M; ++x)
                    {
                        temp[y, x] = Get(y, x);
                    }
                }
                N += m.N;
                data = temp.Clone() as double[,];
            }
        }

        public void ConvertByKernel(Matrix K, int paddingSizeX, int paddingSizeY, PaddingFill paddingFill, int stridingSizeX = 1, int stridingSizeY = 1)
        {
            if (K.N <= N && K.M <= M)
            {
                Matrix res = new Matrix(
                    (N - K.N + 2*paddingSizeY) / stridingSizeY + 1,
                    (M - K.M + 2*paddingSizeX) / stridingSizeX + 1
                );
                for (int y = 0; y < res.N; ++y)
                {
                    for (int x = 0; x < res.M; ++x)
                    {
                        double val = 0;
                        double sum = 0;
                        List<Pair<int, int>> paddingZones = new List<Pair<int, int>>();
                        for (int x1 = x * stridingSizeX - paddingSizeX; 
                            x1 < x*stridingSizeX - paddingSizeX + K.M; ++x1)
                        {
                            for (int y1 = y * stridingSizeY - paddingSizeY; 
                                y1 < y*stridingSizeY - paddingSizeY + K.N; ++y1)
                            {
                                if (x1 >= 0 && x1 < M && y1 >= 0 && y1 < N)
                                {
                                    val += Get(y1, x1) * K.Get(y1 - y * stridingSizeY + paddingSizeY, 
                                        x1 - x * stridingSizeX + paddingSizeX);
                                    sum += Get(y1, x1);
                                }
                                else
                                {
                                    if(paddingFill == PaddingFill.BY_MEDIAN) paddingZones.Add(new Pair<int, int>(x1, y1));
                                }
                            }
                        }
                        sum /= (K.N * K.M - paddingZones.Count);
                        foreach(Pair<int, int> p in paddingZones)
                        {
                            val += sum * K.Get(p.second - y * stridingSizeY + paddingSizeY,
                                        p.first - x * stridingSizeX + paddingSizeX);
                        }
                        res.Set(y, x, val);
                    }
                }
                Assign(ref res);
            }
            else throw new Exception("K.N > N or K.M > M");
        }

        public void ConvertByKernel(Matrix K, int paddingSize = 0, PaddingFill paddingFill = PaddingFill.BY_ZEROES, int stridingSize = 1)
        {
            ConvertByKernel(K, paddingSize, paddingSize, paddingFill, stridingSize, stridingSize);
        }

        public void RemoveLastRow()
        {
            RemoveRow(N - 1);
        }

        public void RemoveLastColumn()
        {
            RemoveColumn(M - 1);
        }

        public double GetAverage()
        {
            double res = 0;
            for (int y = 0; y < N; ++y)
            {
                for (int x = 0; x < M; ++x)
                {
                    res += Get(y, x);
                }
            }
            res /= N * M;
            return res;
        }

        public Matrix Sum(double val)
        {
            Matrix res = new Matrix(N, M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    res.Set(i, j, Get(i, j) + val);
                }
            }
            return res;
        }

        public Matrix Sum(Matrix m)
        {
            Matrix res = new Matrix(N, M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    res.Set(i, j, Get(i, j) + m.Get(i, j));
                }
            }
            return res;
        }

        public Matrix Multiply(double val)
        {
            Matrix res = new Matrix(N, M);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    res.Set(i, j, Get(i, j) * val);
                }
            }
            return res;
        }

        public Matrix Multiply(Matrix m)
        {
            if (M == m.N)
            {
                Matrix res = new Matrix(N, m.M);
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < m.M; ++j)
                    {
                        for (int k = 0; k < M; ++k)
                        {
                            res.Set(i, j, res.Get(i, j) + data[i, k] * m.data[k, j]);
                        }
                    }
                }
                return res;
            }
            else if (N == m.N && M == m.M)
            {
                Matrix res = new Matrix(N, M);
                for (int i = 0; i < N; ++i)
                {
                    for (int j = 0; j < M; ++j)
                    {
                        res.data[i, j] = data[i, j] * m.data[i, j];
                    }
                }
                return res;
            }
            return null;
        }

        public Matrix Minus(double val)
        {
            return Sum(-val);
        }

        public Matrix Minus(Matrix m)
        {
            return Sum(m.GetNegative());
        }

        public void FillZeroes()
        {
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    Set(i, j, 0);
                }
            }
        }

        public override bool Equals(object obj)
        {
            Matrix O = obj as Matrix;
            if (N != O.N || M != O.M) return false;
            for(int y = 0; y < N; ++y)
            {
                for(int x = 0; x < M; ++x)
                {
                    if (Math.Abs(Get(y, x) - O.Get(y, x)) > 1e-6) return false;
                }
            }
            return true;
        }

        public override int GetHashCode() {
            return 0; 
        }
    }
}
