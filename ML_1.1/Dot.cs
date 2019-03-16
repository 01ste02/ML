using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;



namespace ML_1._1
{
    class Dot
    {
        private bool reachedGoal = false;
        private bool isAlive = true;
        private bool isBest = false;

        private float fitness = 0;

        private int radius;
        private Vector2 vel;
        private Vector2 acc;
        private Vector2 position;

        private int maxMovements;

        private int width;
        private int height;

        private int startX;
        private int startY;
        private int startRadius;

        private Brain brain;
        private NoObs sender;
        private Random random;

        private Color color;

        public Dot (NoObs sender, int xInit, int yInit, int radiusInit, int maxSteps, int width, int height, Random random)
        {            
            position = new Vector2(xInit, yInit);
            vel = new Vector2(0, 0);
            acc = new Vector2(0, 0);

            startX = xInit;
            startY = yInit;
            startRadius = radiusInit;

            this.width = width;
            this.height = height;

            radius = radiusInit;

            this.random = random;

            brain = new Brain(maxSteps, random);
            maxMovements = maxSteps;

            this.sender = sender;

            color = Color.FromArgb(1, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)); //Random colors to distinguish different dots from each other
        }

        public void Draw (Graphics g)
        {
            //distinguish the best dot from the others
            if (isBest)
            {
                g.FillEllipse(Brushes.Green, position.X - (radius / 2), position.Y - (radius / 2), (radius + 2) * 2, (radius + 2) * 2);
            }
            else
            {
                Brush brush = new SolidBrush(Color.Black);
                g.FillEllipse(brush, position.X - (radius), position.Y - (radius), radius * 2, radius * 2);
            }
        }

        public void Move ()
        {
            //If there are more steps, set the acceleration to the current step. If not, you are dead
            if (brain.Directions.Length > brain.Step)
            {
                acc = brain.Directions[brain.Step];
                brain.Step++;
                //Console.WriteLine(brain.Step);
            }
            else
            {
                isAlive = false;
            }
            
            //Apply the acceleration to the velocity
            vel = Vector2.Add(vel, acc);

            //If the velocity is too great, limit it to five
            if (vel.Length() > 5)
            {
                vel = Vector2.Multiply(Vector2.Normalize(vel), 5);
            }

            //Apply the velocity to the position
            position = Vector2.Add(position, vel);
        }

        public void Update ()
        {
           if (isAlive && !reachedGoal)
            {
                Move();
                //If touching edges, you are dead. If you touched the goal, you won!
                if (width - (position.X + radius) <= 0 | position.X - radius <= 0 | height - (position.Y + radius) <= 0 | position.Y - radius <= 0)
                {
                    isAlive = false;
                }
                else if (IsTouchingGoal(sender.target, this, out double depth))
                {
                    reachedGoal = true;
                }
                
            }
        }

        private bool IsTouchingGoal (Target target, Dot dot, out double depth)
        {
            //Use the pythagorean theorem (or what it is called in English...) a squared plus b squared equals c squared...
            double distance = Math.Sqrt((target.Position.X - dot.Position.X) * (target.Position.X - dot.Position.X) + (target.Position.Y - dot.Position.Y) * (target.Position.Y - dot.Position.Y));
            double sumOfRadii = target.Radius + dot.Radius;

            //If the distance between the center points of the target and the dot is greater than the sum of the two radii, you are not touching it...
            depth = sumOfRadii - distance;
            if (depth < 0)
            {
                return false;
            }

            return true;
        }

        public void CalculateFitness ()
        {
            //If you reached the goal, you are more suitable for evolving than those who did not. The number of steps it took to touch the goal also matter in your score (lower is better)
            if (reachedGoal)
            {
                fitness = (float)(1.0 / 16.0 + 10000.0 / (float)(brain.Step * brain.Step));
            }
            else
            {
                //If you did not touch the goal, check how far from it you were and calculate the fitness based upon that.
                IsTouchingGoal(sender.target, this, out double distanceToGoal);
                fitness = (float)(1.0 / (float)(distanceToGoal * distanceToGoal));
            }
        }

        public Dot Baby ()
        {
            //Make an identical clone
            Dot clone = new Dot(sender, startX, startY, startRadius, maxMovements, width, height, random)
            {
                brain = brain.Clone()
            };
            return clone;
        }

        public int Radius
        {
            get
            {
                return radius;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Brain Brain
        {
            get
            {
                return brain;
            }
        }

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }

            set
            {
                isAlive = value;
            }
        }

        public bool ReachedGoal
        {
            get
            {
                return reachedGoal;
            }
        }

        public bool IsBest
        {
            get
            {
                return isBest;
            }

            set
            {
                isBest = value;
            }
        }

        public float Fitness
        {
            get
            {
                return fitness;
            }
        }
    }
}
