using System;

namespace StereoStructure
{
    class FAST : KeyPoints
    {
        private enum State
        {
            LIGHTER,
            DARKER,
            SIMILAR,
            NONE
        }

        private int R;
        private int t;
        private int N;

        public FAST(Matrix I) : base(I) {
            R = SettingsListener.Get().fastRadius;
            t = SettingsListener.Get().fastT;
            N = SettingsListener.Get().fastN;
        }

        public override void Compute()
        {
            for (int y = R; y < I.N - R; ++y)
            {
                for(int x = R; x < I.M - R; ++x)
                {
                    int lighers = 0, darkers = 0;
                    State[] stms = new State[4];
                    stms[0] = Check(x, y, 0, -R);
                    stms[1] = Check(x, y, R, 0);
                    stms[2] = Check(x, y, 0, R);
                    stms[3] = Check(x, y, -R, 0);
                    foreach(State st in stms)
                    {
                        if (st == State.LIGHTER) ++lighers;
                        if (st == State.DARKER) ++darkers;
                    }
                    if(lighers >= 3 || darkers >= 3)
                    {
                        State current = State.NONE;
                        int count = 0;
                        int cells = 8 + 4 * (R - 1);
                        double phi = (2 * Math.PI) / cells;
                        if (cells < N) throw new Exception("Cells count on circle < fastN ("+cells+"<"+N);
                        for(int k = 0; k < cells; ++k)
                        {
                            int x_ = (int)Math.Round(R * Math.Cos(k*phi));
                            int y_ = (int)Math.Round(R * Math.Sin(k*phi));
                            State st = Check(x, y, x_, y_);
                            if (current == State.NONE)
                            {
                                current = st;
                                ++count;
                            }
                            else if (current == st) ++count;
                            else
                            {
                                current = st;
                                count = 1;
                            }

                            if(count >= N && (current == State.LIGHTER || current == State.DARKER))
                            {
                                points.Add(new Point(x, y));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private State Check(int cX, int cY, int x_, int y_)
        {
            try
            {
                double Ic = I.data[cY, cX];
                double Ip = I.data[cY + y_, cX + x_];
                if (Ip > Ic + t) return State.LIGHTER;
                if (Ip < Ic - t) return State.DARKER;
            }
            catch
            {
                throw new Exception("Your image sizes: ("+I.M+"x"+I.N+").\nCan't reach ("+(cX+x_)+","+(cY+y_)+") index\n");
            }
            return State.SIMILAR;
        }
    }
}
