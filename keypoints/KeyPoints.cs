using System.Collections.Generic;

namespace StereoStructure
{
    public abstract class KeyPoints
    {
        protected List<Point> points;
        protected Matrix I;
        
        public KeyPoints(Matrix I)
        {
            points = new List<Point>();
            this.I = I;
        }

        public List<Point> GetPoints()
        {
            return points;
        }
        
        public abstract void Compute();
    }
}
