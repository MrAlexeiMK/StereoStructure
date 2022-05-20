using System.Collections.Generic;

namespace StereoStructure
{
    public abstract class Descriptor
    {
        protected List<Point> left, right;
        protected List<Pair<int, int>> pairs;
        
        public Descriptor(List<Point> left, List<Point> right)
        {
            this.left = left;
            this.right = right;
            pairs = new List<Pair<int, int>>();
        }

        public List<Pair<int, int>> GetPairs()
        {
            return pairs;
        }

        public abstract void Compute();
    }
}
