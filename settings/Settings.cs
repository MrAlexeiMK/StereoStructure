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
        public int framesStep;
        public int skipFrames;
        public double siftSigmaMin;
        public double siftSigmaMax;
        public double siftSigmaStep;
        public int siftScalesCount;
        public bool applyMedianFilterOnLoad;
        public int medianFilterSize;
        public bool applyHessianOperator;
        public double siftHessianR;
        public Pair<OperatorType, OperatorType> bordersOperator;
        public int imageWidth;
        public CorrespondencesAlg cAlg;
        public int fastRadius;
        public int fastT;
        public int fastN;
        public DescriptorAlg dAlg;
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
            circleWidth = 3;
            framesStep = 1;
            skipFrames = 3;
            siftSigmaMin = 3;
            siftSigmaMax = 21;
            siftSigmaStep = 6;
            siftScalesCount = 4;
            applyMedianFilterOnLoad = true;
            medianFilterSize = 25;
            applyHessianOperator = true;
            siftHessianR = 3;
            bordersOperator = new Pair<OperatorType, OperatorType>(OperatorType.SOBEL_X, OperatorType.SOBEL_Y);
            imageWidth = 640;
            cAlg = CorrespondencesAlg.ORB;
            fastRadius = 3;
            fastT = 50;
            fastN = 12;
            dAlg = DescriptorAlg.BRIEF;
        }
    }
}
