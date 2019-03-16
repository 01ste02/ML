using System.Drawing;

namespace ML_1._1
{
    public class Target
    {
        private int radius;

        private Point position;

        public Target (int radiusInit, int xInit, int yInit)
        {
            radius = radiusInit;
            position = new Point(xInit, yInit);
        }

        public void Draw (Graphics g)
        {
            g.FillEllipse(Brushes.Blue, position.X - (radius), position.Y - (radius), radius * 2, radius * 2);
        }

        public Point Position
        {
            get
            {
                return position;
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }
        }
    }
}
