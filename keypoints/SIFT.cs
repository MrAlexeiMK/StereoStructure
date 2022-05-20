using System;
using System.Collections.Generic;

namespace StereoStructure
{
    class SIFT : KeyPoints
    {
        private int scalesCount;
        private double sigmaMax;
        private double sigmaMin;
        private double sigmaStep;
        private bool applyHessian;
        private double hessianR;
        private Pair<OperatorType, OperatorType> bordersOperator;
        public SIFT(Matrix I) : base(I) {
            scalesCount = SettingsListener.Get().siftScalesCount;
            sigmaMax = SettingsListener.Get().siftSigmaMax;
            sigmaMin = SettingsListener.Get().siftSigmaMin;
            sigmaStep = SettingsListener.Get().siftSigmaStep;
            applyHessian = SettingsListener.Get().applyHessianOperator;
            hessianR = SettingsListener.Get().siftHessianR;
            bordersOperator = SettingsListener.Get().bordersOperator;
        }
        public override void Compute()
        {
            if (sigmaMax * 6 >= Math.Min(I.N, I.M)) throw new Exception("SIFT 'sigma max value' is too high or 'image width' is too low");

            List<List<Matrix>> P = new List<List<Matrix>>();
            Matrix A, B;
            for (int scale = 0; scale < scalesCount; ++scale)
            {
                double mult = 1.0 / Math.Pow(2, scale - 1);
                List<Matrix> DoG = new List<Matrix>();
                Matrix scaled = MatrixExtractor.GetResized(I, (int)(I.M * mult));
                Logs.WriteMainThread("SIFT adding (" + scaled.M + "," + scaled.N + ") scale");
                A = scaled.Clone();
                for (double k = sigmaMin; k <= sigmaMax; k += sigmaStep)
                {
                    B = scaled.Clone();
                    B.ConvertByKernel(new Matrix(OperatorType.GAUSS_1D_X, k * mult), PaddingFill.BY_MEDIAN);
                    B.ConvertByKernel(new Matrix(OperatorType.GAUSS_1D_Y, k * mult), PaddingFill.BY_MEDIAN);
                    DoG.Add(MatrixExtractor.GetMinus(B, A));
                    A = B;
                    Logs.WriteMainThread("SIFT Gauss Blur has been applied with " + (int)(k * mult * 6) + " kernel size");
                }
                P.Add(DoG);
                Logs.WriteMainThread("SIFT scale added: (" + scaled.M + "," + scaled.N + ")");
            }

            int scales = P.Count;
            int octaves = P[0].Count;

            if (scales < 3 || octaves <= 0) throw new Exception("We don't have enough scales or octaves");

            int c = 0;
            for (int j = 0; j < octaves; ++j)
            {
                for (int i = 1; i < scales - 1; ++i)
                {
                    AddPoints(P[i][j], P[i + 1][j], P[i - 1][j], Math.Pow(2, i - 1));
                    ++c;
                    Logs.WriteMainThread("SIFT added points: " + c + "/" + octaves * (scales - 2));
                }
            }
        }

        private void AddPoints(Matrix im, Matrix imUp, Matrix imDown, double mult = 1.0)
        {
            Matrix D = null, Dx = null, Dy = null, Dxx = null, Dyy = null, Dxy = null;
            OperatorType op1 = OperatorType.SOBEL_X, op2 = OperatorType.SOBEL_Y;
            if (applyHessian)
            {
                if (bordersOperator.first == OperatorType.SHAR_X)
                {
                    op1 = OperatorType.SHAR_X;
                    op2 = OperatorType.SHAR_Y;
                }
                D = MatrixExtractor.GetMultiply(im, 1.0 / 255);
                Dx = MatrixExtractor.GetConvertByKernel(D, MatrixExtractor.GetOperator(op1), PaddingFill.BY_MEDIAN);
                Dy = MatrixExtractor.GetConvertByKernel(D, MatrixExtractor.GetOperator(op2), PaddingFill.BY_MEDIAN);
                Dxy = MatrixExtractor.GetConvertByKernel(Dx, MatrixExtractor.GetOperator(op2), PaddingFill.BY_MEDIAN);
                Dxx = MatrixExtractor.GetConvertByKernel(Dx, MatrixExtractor.GetOperator(op1), PaddingFill.BY_MEDIAN);
                Dyy = MatrixExtractor.GetConvertByKernel(Dy, MatrixExtractor.GetOperator(op2), PaddingFill.BY_MEDIAN);
            }
            for (int y = 1; y < im.N - 1; ++y)
            {
                for (int x = 1; x < im.M - 1; ++x)
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
                        int X = (int)Math.Round(x * mult);
                        int Y = (int)Math.Round(y * mult);
                        double x_ = -(1.0 / Dxx.data[y, x]) * Dx.data[y, x];
                        double y_ = -(1.0 / Dyy.data[y, x]) * Dy.data[y, x];
                        double dx = D.data[y, x] + (Dx.data[y, x] * (x - x_)) / 2.0;
                        double dy = D.data[y, x] + (Dy.data[y, x] * (y - y_)) / 2.0;
                        if (dx < 0.03 && dy < 0.03) continue;
                        if (applyHessian)
                        {
                            Matrix H = new Matrix(2, 2);
                            H.data[0, 0] = Dxx.data[y, x];
                            H.data[1, 1] = Dyy.data[y, x];
                            H.data[0, 1] = Dxy.data[y, x];
                            H.data[1, 0] = Dxy.data[y, x];
                            if (Math.Pow(H.GetTrace(), 2) / H.Determinant() < Math.Pow((hessianR + 1), 2) / hessianR)
                            {
                                points.Add(new Point(X, Y));
                            }
                        }
                        else
                        {
                            points.Add(new Point(X, Y));
                        }
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
                    double v = im.data[(int)((y + v1) * mult), (int)((x + v2) * mult)];
                    if (max && val <= v) return false;
                    if (!max && val >= v) return false;
                }
            }
            return true;
        }
    }
}
