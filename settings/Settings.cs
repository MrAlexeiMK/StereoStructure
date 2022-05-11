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
        public Rotation rotationImages;
        public int gridWidth;
        public int gridLength;
        public double gridThickness;
        public Color defaultColor;
        public double maxWidth;
        public int circleWidth;
        public int skipFrames;
        public double siftSigmaMin;
        public double siftSigmaMax;
        public double siftSigmaStep;
        public int siftScalesCount;
        public Settings()
        {
            openLogs = true;
            correspondences = true;
            lang = LANG.EN;
            accuracy = ACCURACY.LOW;
            rotationImages = Rotation.NONE;
            gridWidth = 500;
            gridLength = 500;
            gridThickness = 0.3;
            defaultColor = new Color(128, 128, 128);
            maxWidth = 50;
            circleWidth = 10;
            skipFrames = 3;
            siftSigmaMin = 2;
            siftSigmaMax = 5;
            siftSigmaStep = 6;
            siftScalesCount = 4;
        }
    }
}
