using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace ML_1._1
{
    class Population
    {
        private Dot[] dots;

        private float fitnessSum;
        private int generation = 1;

        private int bestDotIndex = 0;

        int minStep = 1000;

        public Population (int size, Form1 sender, int x, int y, int radius, int maxSteps, int width, int height)
        {
            //Make an array with dots and initialize all of them
            dots = new Dot[size];
            for (int i = 0; i < size; i++)
            {
                dots[i] = new Dot(sender, x, y, radius, maxSteps, width, height);
            }
        }

        public void draw (Graphics g)
        {
            //Draw each and every dot. Dot 0 is drawn by itself for no particular reason.
            for (int i = 1; i < dots.Length; i++)
            {
                dots[i].Draw(g);
            }
            dots[0].Draw(g);
        }

        public void update ()
        {
            //For all dots, check if the dot has taken more steps to get to the goal than the previously lowest number of steps. If so, kill it (because it is worse than the last generation)
            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].Brain.Step > minStep)
                {
                    dots[i].IsAlive = false;
                }
                else
                {
                    //Update the dot if not.
                    dots[i].update();
                }
            }
            //Console.WriteLine("X-coord: " + dots[0].Position.X);
            //Console.WriteLine("Y-coord: " + dots[0].Position.Y);
            //Console.WriteLine(dots[0].Brain.Directions[dots[0].Brain.Step].Length());
            //Console.WriteLine(dots[0].Brain.Step);

            //Debug function to see how many unique values there are between all of the dots
            occurances();
        }

        public void calculateFitness ()
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].calculateFitness();
            }
        }

        //If not all dots are dead, there is at least one that has not reached the goal or died. Return false.
        public bool allDotsDead ()
        {
            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].IsAlive && !dots[i].ReachedGoal)
                {
                    return false;
                }
            }

            return true;
        }


        //Self-explanatory. Make a new generation and keep the best one from the previous generation unchanged to keep track of wether they got worse or not.
        public void naturalSelection ()
        {
            Dot[] newDots = new Dot[dots.Length];
            setBestDot();
            calculateFitnessSum();

            //Best dot
            newDots[0] = dots[bestDotIndex].Baby();
            newDots[0].IsBest = true;
            
            for (int i = 1; i < newDots.Length; i++)
            {
                //Mutate the others after cloning them.
                Dot parent = selectParent();

                newDots[i] = parent.Baby();
            }

            dots = newDots;
            generation++;
        }

        public void calculateFitnessSum ()
        {
            //Sum the fitnessSums
            fitnessSum = 0;
            for (int i = 0; i < dots.Length; i++)
            {
                fitnessSum += dots[i].Fitness;
            }
        }

        public Dot selectParent ()
        {
            //Choose the best dots to be cloned into a new generation
            Random random = new Random();
            float rand = (float)random.NextDouble() * fitnessSum;

            float runningSum = 0;

            for (int i = 0; i < dots.Length; i++)
            {
                runningSum += dots[i].Fitness;
                if (runningSum > rand)
                {
                    return dots[i];
                }
            }

            return null;
        }

        public void mutateBabies ()
        {
            for (int i = 1; i < dots.Length; i++)
            {
                dots[i].Brain.Mutate();
            }
        }

        public void setBestDot ()
        {
            //Return the dot with the highest fitness
            float max = 0;
            int maxIndex = 0;

            for (int i = 0; i < dots.Length; i++)
            {
                if (dots[i].Fitness > max)
                {
                    max = dots[i].Fitness;
                    maxIndex = i;
                }
            }

            bestDotIndex = maxIndex;

            if (dots[bestDotIndex].ReachedGoal)
            {
                minStep = dots[bestDotIndex].Brain.Step;
                Console.WriteLine("Steps: " + minStep.ToString());
            }
        }

        public void occurances ()
        {
            int uCountX = 0;
            int uCountY = 0;

            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();

            foreach (Dot a in dots)
            {
                bool xExists = false;
                for (int i = 0; i < xValues.Count; i++)
                {
                    if (a.Brain.Step < a.Brain.Directions.Length)
                    {
                        if (a.Brain.Directions[a.Brain.Step].X == xValues[i])
                        {
                            xExists = true;
                        }
                    }
                }

                if (!xExists)
                {
                    if (a.Brain.Step < a.Brain.Directions.Length)
                    {
                        xValues.Add(a.Brain.Directions[a.Brain.Step].X);
                        uCountX++;
                    }
                }

                bool yExists = false;
                for (int i = 0; i < yValues.Count; i++)
                {
                    if (a.Brain.Step < a.Brain.Directions.Length)
                    {
                        if (a.Brain.Directions[a.Brain.Step].Y == yValues[i])
                        {
                            yExists = true;
                        }
                    }
                }

                if (!yExists)
                {
                    if (a.Brain.Step < a.Brain.Directions.Length)
                    {
                        yValues.Add(a.Brain.Directions[a.Brain.Step].Y);
                        uCountY++;
                    }
                }
            }
            Console.WriteLine("Unique X: " + uCountX);
            Console.WriteLine("Unique Y: " + uCountY);
        }
    }
}
