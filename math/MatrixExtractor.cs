using System;
using System.Collections.Generic;
using System.Globalization;

namespace StereoStructure
{
    public static class MatrixExtractor
    {
        public static Matrix GetMatrixFromExpression(string reg, Dictionary<string, double> mask = null)
        {
            string[] spl = reg.Split('|');
            int N = spl.Length;
            int M = spl[0].Split(',').Length;
            Matrix res = new Matrix(N, M);
            for (int y = 0; y < N; ++y)
            {
                string[] spl2 = spl[y].Split(',');
                if (M != spl2.Length) throw new Exception("Incorrect Matrix format");
                for (int x = 0; x < M; ++x)
                {
                    string cell = spl2[x];
                    res.data[y, x] = DoubleFromCell(cell, mask);
                }
            }
            return res;
        }

        public static double DoubleFromCell(string cell, Dictionary<string, double> mask = null)
        {
            cell = cell.Replace(" ", "");
            cell = cell.Replace("*", "");
            cell = cell.Replace("-", "=");
            if (cell == "") throw new Exception("Cell is empty");
            if (cell[0] == '=') cell = "-" + cell.Substring(1);
            bool isPlus = true;
            double a = 1, B = 1, c = 0;
            string[] spl = cell.Split('+');
            if (spl.Length == 1)
            {
                spl = cell.Split('=');
                isPlus = false;
            }
            if (spl.Length > 2) throw new Exception("Incorrect cell format (aB+c)");
            string left = spl[0];
            int i = 0;
            if (left[0] == '-') ++i;
            while (i < left.Length && (Char.IsDigit(left[i]) || left[i] == '.')) ++i;
            if (i > 0)
            {
                if (i == 1 && left[0] == '-') a = -1;
                else
                {
                    try
                    {
                        a = Double.Parse(left.Substring(0, i), CultureInfo.InvariantCulture);
                    } catch
                    {
                        throw new Exception("Can't parse string "+ left.Substring(0, i));
                    }
                }
            }
            if (i < left.Length && Char.IsLetter(left[i]))
            {
                string Bstr = left.Substring(i, left.Length - i);
                string sub = "";
                if (Bstr.Length > 5) sub = Bstr.Substring(0, 3).ToLower();
                if (sub == "sin" || sub == "cos" || sub == "tan")
                {
                    B = DoubleFromCell(Bstr.Substring(4, Bstr.Length - 5), mask);
                    if (sub == "sin") B = Math.Sin(B);
                    else if (sub == "cos") B = Math.Cos(B);
                    else if (sub == "tan") B = Math.Tan(B);
                }
                else if(mask != null)
                {
                    if (!mask.ContainsKey(Bstr)) throw new Exception("Cell doesn't contains '"+Bstr+"' key");
                    B = mask[Bstr];
                }
            }
            if(spl.Length > 1)
            {
                string right = spl[1];
                if (mask != null && mask.ContainsKey(right)) c = mask[right];
                else
                {
                    try
                    {
                        c = Double.Parse(right, CultureInfo.InvariantCulture);
                    } catch
                    {
                        throw new Exception("Can't parse string " + right);
                    }
                }
            }
            return isPlus ? a*B+c : a*B-c;
        }

        public static Matrix GetConvertByKernel(Matrix A, Matrix K, int paddingSizeX, int paddingSizeY, PaddingFill paddingFill, int stridingSizeX = 1, int stridingSizeY = 1)
        {
            if (K.N <= A.N && K.M <= A.M)
            {
                Matrix res = new Matrix(
                    (A.N - K.N + 2 * paddingSizeY) / stridingSizeY + 1,
                    (A.M - K.M + 2 * paddingSizeX) / stridingSizeX + 1
                );
                for (int y = 0; y < res.N; ++y)
                {
                    for (int x = 0; x < res.M; ++x)
                    {
                        double val = 0;
                        double sum = 0;
                        List<Pair<int, int>> paddingZones = new List<Pair<int, int>>();
                        for (int x1 = x * stridingSizeX - paddingSizeX;
                            x1 < x * stridingSizeX - paddingSizeX + K.M; ++x1)
                        {
                            for (int y1 = y * stridingSizeY - paddingSizeY;
                                y1 < y * stridingSizeY - paddingSizeY + K.N; ++y1)
                            {
                                if (x1 >= 0 && x1 < A.M && y1 >= 0 && y1 < A.N)
                                {
                                    val += A.Get(y1, x1) * K.Get(y1 - y * stridingSizeY + paddingSizeY,
                                        x1 - x * stridingSizeX + paddingSizeX);
                                    sum += A.Get(y1, x1);
                                }
                                else
                                {
                                    if (paddingFill == PaddingFill.BY_MEDIAN) paddingZones.Add(new Pair<int, int>(x1, y1));
                                }
                            }
                        }
                        sum /= (K.N * K.M - paddingZones.Count);
                        foreach (Pair<int, int> p in paddingZones)
                        {
                            val += sum * K.Get(p.second - y * stridingSizeY + paddingSizeY,
                                        p.first - x * stridingSizeX + paddingSizeX);
                        }
                        res.Set(y, x, val);
                    }
                }
                return res;
            }
            else throw new Exception("K.N > N or K.M > M");
        }

        public static Matrix GetConvertByKernel(Matrix A, Matrix K, PaddingFill paddingFill)
        {
            if (K.N % 2 == 0 || K.M % 2 == 0) throw new Exception("Kernel size should be odd");
            return GetConvertByKernel(A, K, (K.M - 1) / 2, (K.N - 1) / 2, paddingFill, 1, 1);
        }

        public static Matrix GetConvertByKernel(Matrix A, Matrix K, int paddingSize = 0, PaddingFill paddingFill = PaddingFill.BY_ZEROES, int stridingSize = 1)
        {
            return GetConvertByKernel(A, K, paddingSize, paddingSize, paddingFill, stridingSize, stridingSize);
        }

        public static Matrix GetScale(Matrix A, double scale)
        {
            if (scale <= 0 || scale > 1) throw new Exception("Scale should be on (0; 1)");
            if (scale == 1) return A;
            int size = (int)(1 / scale);
            Matrix K = new Matrix(size, size, 1.0 / (size * size));
            return GetConvertByKernel(A, K, 0, PaddingFill.BY_ZEROES, size);
        }

        public static Matrix GetSubMatrix(Matrix A, int fromCol, int toCol)
        {
            int cols = toCol - fromCol + 1;
            Matrix temp = new Matrix(A.N, cols);
            for (int i = 0; i < A.N; ++i)
            {
                for (int j = fromCol - 1; j < toCol; ++j)
                {
                    temp.data[i, j - fromCol + 1] = A.data[i, j];
                }
            }
            return temp;
        }

        public static Matrix Unite(Matrix A, Matrix B)
        {
            if (A.N != B.N) throw new Exception("Invalid matrix dimensions");
            Matrix temp = new Matrix(A.N, A.M + B.M);
            for (int i = 0; i < A.N; ++i)
            {
                for (int j = 0; j < A.M; ++j)
                {
                    temp.data[i, j] = A.data[i, j];
                }
            }
            for (int i = 0; i < A.N; ++i)
            {
                for (int j = A.M; j < A.M + B.M; ++j)
                {
                    temp.data[i, j] = B.data[i, j - A.M];
                }
            }
            return temp;
        }

        public static Matrix GetTranspose(Matrix A)
        {
            Matrix temp = new Matrix(A.M, A.N);
            for (int i = 0; i < A.N; ++i)
            {
                for (int j = 0; j < A.M; ++j)
                {
                    temp.data[j, i] = A.data[i, j];
                }
            }
            return temp;
        }

        public static Matrix GetPaddingLayersMatrix(Matrix A, int paddingSize, PaddingFill paddingFill = PaddingFill.BY_MEDIAN, int window = 3)
        {
            if (paddingSize <= 0) throw new Exception("PaddingSize should be > 0");
            if (window % 2 == 0) throw new Exception("Window size should be odd");
            Matrix res = new Matrix(A.N+2*paddingSize, A.M+2*paddingSize, 0);
            res.Replace(paddingSize, paddingSize, A);
            if(paddingFill == PaddingFill.BY_MEDIAN)
            {
                while (paddingSize > 0)
                {
                    int x = paddingSize - 1, y = paddingSize - 1;
                    while (x <= res.M - paddingSize)
                    {
                        res.SetByAveragedValues(x, y, window);
                        ++x;
                    }
                    --x;
                    ++y;
                    while (y <= res.N - paddingSize)
                    {
                        res.SetByAveragedValues(x, y, window);
                        ++y;
                    }
                    --y;
                    --x;
                    while (x >= paddingSize - 1)
                    {
                        res.SetByAveragedValues(x, y, window);
                        --x;
                    }
                    ++x;
                    --y;
                    while (y >= paddingSize - 1)
                    {
                        res.SetByAveragedValues(x, y, window);
                        --y;
                    }
                    --paddingSize;
                }
            }
            return res;
        }

        public static Matrix GetMedianFiltered(Matrix A, int K)
        {
            if (K % 2 == 0) throw new Exception("K should be odd");
            Matrix res = new Matrix(A.N, A.M);
            Matrix I = GetPaddingLayersMatrix(A, K/2);
            for(int x = 0; x < A.M; ++x)
            {
                for(int y = 0; y < A.N; ++y)
                {
                    List<double> list = new List<double>();
                    for(int x_ = x - K/2; x_ <= x + K/2; ++x_)
                    {
                        for (int y_ = y - K / 2; y_ <= y + K / 2; ++y_)
                        {
                            list.Add(I.data[y_ + K/2, x_ + K/2]);
                        }
                    }
                    list.Sort();
                    int k = (K*K+1)/2;
                    if (Math.Abs(list[k] - I.data[y, x]) < Math.Abs(list[K * K - k + 1]) - I.data[y, x])
                    {
                        res.data[y, x] = list[k];
                    }
                    else res.data[y, x] = list[K*K-k+1];
                }
            }
            return res;
        }

        public static Matrix GetSum(Matrix A, Matrix B)
        {
            if (A.N != B.N || A.M != B.M) throw new Exception("Different dimensions");
            Matrix res = new Matrix(A.N, A.M);
            for (int i = 0; i < A.N; ++i)
            {
                for (int j = 0; j < A.M; ++j)
                {
                    res.Set(i, j, A.Get(i, j) + B.Get(i, j));
                }
            }
            return res;
        }

        public static Matrix GetMinus(Matrix A, Matrix B)
        {
            return GetSum(A, B.GetNegative());
        }

        public static Matrix GetOperator(OperatorType operatorType, double a = 1.0)
        {
            Dictionary<string, double> mask = new Dictionary<string, double>();
            mask["a"] = a;
            mask["rad"] = a * Math.PI / 180;

            Matrix res;
            double sum;
            int dim = (int)(a * 6);
            if (dim % 2 == 0) ++dim;

            switch (operatorType)
            {
                case OperatorType.ROTATION_2D:
                    return new Matrix("cos(rad),-sin(rad)|sin(rad),cos(rad)", mask);
                case OperatorType.ROTATION_3D_X:
                    return new Matrix("1,0,0|0,cos(rad),sin(rad)|0,-sin(rad),cos(rad)", mask);
                case OperatorType.ROTATION_3D_Y:
                    return new Matrix("cos(rad),0,-sin(rad)|0,1,0|sin(rad),0,cos(rad)", mask);
                case OperatorType.ROTATION_3D_Z:
                    return new Matrix("cos(rad),sin(rad),0|-sin(rad),cos(rad),0|0,0,1", mask);
                case OperatorType.GAUSS_2D:
                    res = new Matrix(dim, dim);
                    sum = 0;
                    for (int y = 0; y < dim; ++y)
                    {
                        for (int x = 0; x < dim; ++x)
                        {
                            res.data[y, x] = Constants.Gauss(x - dim / 2, y - dim / 2, a);
                            sum += res.data[y, x];
                        }
                    }
                    res.Multiply(1.0 / sum);
                    return res;
                case OperatorType.GAUSS_1D_X:
                    res = new Matrix(1, dim);
                    sum = 0;
                    for (int x = 0; x < dim; ++x)
                    {
                        res.data[0, x] = Constants.Gauss1D(x - dim / 2, a);
                        sum += res.data[0, x];
                    }
                    res.Multiply(1.0 / sum);
                    return res;
                case OperatorType.GAUSS_1D_Y:
                    res = new Matrix(dim, 1);
                    sum = 0;
                    for (int y = 0; y < dim; ++y)
                    {
                        res.data[y, 0] = Constants.Gauss1D(y - dim / 2, a);
                        sum += res.data[y, 0];
                    }
                    res.Multiply(1.0 / sum);
                    return res;
                case OperatorType.SOBEL_X:
                    return new Matrix("1,0,-1|2,0,-2|1,0,-1");
                case OperatorType.SOBEL_Y:
                    return new Matrix("1,2,1|0,0,0|-1,-2,-1");
            }

            throw new Exception("Operator not found");
        }
    }
}
