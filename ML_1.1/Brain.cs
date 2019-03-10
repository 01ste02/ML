using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace ML_1._1
{
    class Brain
    {
        private Vector2[] directions;
        private int step = 0;

        public Brain (int maxDirections)
        {
            directions = new Vector2[maxDirections];
            randomize();

        }

        private void randomize ()
        {
            Random random = new Random();
            //Create random instructions of in which direction to go. Then convert the angle from radians to an X and Y value using trigonometry. Seems to be unique between all dots.
            double[] doubles = new double[directions.Length];
            for (int i = 0; i < directions.Length; i++)
            {
                double randAngle = (double)(random.Next(0, 1000000000) / (double)1000000000) * 2 * Math.PI;
                directions[i] = new Vector2((float)(Math.Cos(randAngle)), (float)(Math.Sin(randAngle)));
            }
        }

        public Brain Clone ()
        {
            //Make a new, identical, brain
            Brain clone = new Brain(directions.Length);
            for (int i = 0; i < directions.Length; i++)
            {
                clone.directions[i] = directions[i];
            }

            return clone;
        }

        public void Mutate ()
        {
            //Mutate the babies in some cases. Randomize some new directions.
            double mutationRate = 0.01;
            Random random = new Random();

            for (int i = 0; i < directions.Length; i++)
            {
                double rand = random.NextDouble();
                if (rand < mutationRate)
                {
                    double randomAngle = (double)(random.NextDouble() * 2 * Math.PI);
                    directions[i] = new Vector2((float)(Math.Cos(randomAngle)), (float)(Math.Sin(randomAngle)));
                }
            }
        }

        public Vector2[] Directions
        {
            get
            {
                return directions;
            }
        }

        public int Step
        {
            get
            {
                return step;
            }
            
            set
            {
                //if (value > 0 && value < directions.Length)
                //{
                    step = value;
                //}
            }
        }
    }
}
