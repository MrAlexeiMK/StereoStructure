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
        public double[,] data;
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

        public Matrix(string reg, Dictionary<string, double> mask = null)
        {
            Matrix res = MatrixExtractor.GetMatrixFromExpression(reg, mask);
            Assign(res);
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
                    double pixel = 0;
                    if(color == ColorType.GRAY)
                    {
                        pixel = (frame.GetPixel(j, i).R*0.299 + frame.GetPixel(j, i).G*0.587 + frame.GetPixel(j, i).B*0.114);
                    }
                    else if (color == ColorType.RED) pixel = frame.GetPixel(j, i).R;
                    else if (color == ColorType.GREEN) pixel = frame.GetPixel(j, i).G;
                    else if (color == ColorType.BLUE) pixel = frame.GetPixel(j, i).B;
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

        public Matrix(OperatorType operatorType, double arg = 1.0)
        {
            Matrix res = MatrixExtractor.GetOperator(operatorType, arg);
            Assign(res);
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

        public Matrix Clone()
        {
            return new Matrix(this);
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

        public void Assign(Matrix m)
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

        public void Transpose()
        {
            Matrix res = MatrixExtractor.GetTranspose(this);
            Assign(res);
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

        public void Unite(Matrix A)
        {
            Matrix res = MatrixExtractor.Unite(this, A);
            Assign(res);
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
            Assign(m);
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
            Assign(m);
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

        public void Rotate(Rotation rotation)
        {
            Matrix res = new Matrix();
            if(rotation == Rotation.ROTATION_90)
            {
                res = new Matrix(M, N);
                for(int y = 0; y < N; ++y)
                {
                    for(int x = 0; x < M; ++x)
                    {
                        res.data[x, y] = data[y, M - x - 1];
                    }
                }
            }
            else if (rotation == Rotation.ROTATION_180)
            {
                res = new Matrix(N, M);
                for (int y = 0; y < N; ++y)
                {
                    for (int x = 0; x < M; ++x)
                    {
                        res.data[y, x] = data[N - y - 1, M - x - 1];
                    }
                }
            }
            else if (rotation == Rotation.ROTATION_270)
            {
                res = new Matrix(M, N);
                for (int y = 0; y < N; ++y)
                {
                    for (int x = 0; x < M; ++x)
                    {
                        res.data[x, y] = data[N-y-1, x];
                    }
                }
            }
            Assign(res);
        }

        public void Scale(double scale)
        {
            Matrix res = MatrixExtractor.GetScale(this, scale);
            Assign(res);
        }

        public void Resize(int width, int height)
        {
            Matrix res = MatrixExtractor.GetResized(this, width, height);
            Assign(res);
        }

        public void Resize(int width)
        {
            Matrix res = MatrixExtractor.GetResized(this, width);
            Assign(res);
        }

        public void ConvertByMedianFilter(int K)
        {
            Matrix res = MatrixExtractor.GetMedianFiltered(this, K);
            Assign(res);
        }

        public void AddPaddingLayers(int paddingSize, PaddingFill paddingFill = PaddingFill.BY_MEDIAN, int window = 3)
        {
            Matrix res = MatrixExtractor.GetPaddingLayersMatrix(this, paddingSize, paddingFill, window);
            Assign(res);
        }

        public void SetByAveragedValues(int x_center, int y_center, int K)
        {
            double sum = 0.0;
            int count = 0;
            for (int x = Math.Max(0, x_center-K/2); x <= Math.Min(M-1, x_center+K/2); ++x)
            {
                for (int y = Math.Max(0, y_center-K/2); y <= Math.Min(N-1, y_center+K/2); ++y)
                {
                    double val = data[y, x];
                    if (val > 0)
                    {
                        sum += val;
                        ++count;
                    }
                }
            }
            if (count > 0)
            {
                data[y_center, x_center] = sum / count;
            }
        }

        public void ConvertByKernel(Matrix K, int paddingSizeX, int paddingSizeY, PaddingFill paddingFill, int stridingSizeX = 1, int stridingSizeY = 1)
        {
            Matrix res = MatrixExtractor.GetConvertByKernel(this, K, paddingSizeX, paddingSizeY, paddingFill, stridingSizeX, stridingSizeY);
            Assign(res);
        }

        public void ConvertByKernel(Matrix K, PaddingFill paddingFill)
        {
            Matrix res = MatrixExtractor.GetConvertByKernel(this, K, (K.M - 1) / 2, (K.N - 1) / 2, paddingFill, 1, 1);
            Assign(res);
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

        public double GetTrace()
        {
            double res = 0;
            for(int i = 0; i < Math.Min(N, M); ++i)
            {
                res += data[i, i];
            }
            return res;
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
            Assign(res);
            return this;
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
            Assign(res);
            return this;
        }

        public Matrix Sum(Matrix m)
        {
            Matrix res = MatrixExtractor.GetSum(this, m);
            Assign(res);
            return this;
        }

        public Matrix Multiply(double val)
        {
            Matrix res = MatrixExtractor.GetMultiply(this, val);
            Assign(res);
            return this;
        }

        public Matrix Multiply(Matrix m)
        {
            Matrix res = MatrixExtractor.GetMultiply(this, m);
            Assign(res);
            return this;
        }

        public Matrix Minus(double val)
        {
            return Sum(-val);
        }

        public Matrix Minus(Matrix m)
        {
            return Sum(m.GetNegative());
        }

        public Matrix Invserse()
        {
            if (N != M) throw new Exception("Invalid matrix dimensions");
            Matrix E = new Matrix(N, N);
            for (int i = 0; i < N; ++i)
            {
                for (int j = 0; j < M; ++j)
                {
                    if (i != j) E.data[i, j] = 0;
                    else E.data[i, j] = 1;
                }
            }
            Matrix A = MatrixExtractor.Unite(this, E);
            A.ToUnit();
            for (int i = 0; i < A.N; ++i)
            {
                if (A.data[i, i] == 0) throw new Exception("Determinant = 0");
            }
            A = MatrixExtractor.GetSubMatrix(A, N+1, 2*N);
            Assign(A);
            return this;
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
