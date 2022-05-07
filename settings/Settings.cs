using System;

namespace StereoStructure
{
    [Serializable]
    class Settings
    {
        public bool openLogs;
        public bool correspondences;
        public LANG lang;
        public ACCURACY accuracy;
        public int gridWidth;
        public int gridLength;
        public double gridThickness;
        public Color defaultColor;
        public double maxWidth;
        public int circleWidth;
        public double eps;
        public double gaussSigma;
        public Settings()
        {
            openLogs = true;
            correspondences = true;
            lang = LANG.EN;
            accuracy = ACCURACY.LOW;
            gridWidth = 500;
            gridLength = 500;
            gridThickness = 0.3;
            defaultColor = new Color(128, 128, 128);
            maxWidth = 50;
            circleWidth = 10;
            eps = 1e-9;
            gaussSigma = 50;
        }
    }
}
