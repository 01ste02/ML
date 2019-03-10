using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace ML_1._1
{
    public partial class Form1 : Form
    {
        public Target target;
        private Population population;

        private Form1 form1;

        private System.Timers.Timer timer;

        public Form1 ()
        {
            InitializeComponent();
            
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            BackColor = Color.White;

            //Draw the target dot (blue)
            target.Draw(e.Graphics);

            //If all dots are dead, then do some calculations into who was best, make clones and evolve them. Otherwise they should be updated in position and be drawn
            if (population.allDotsDead())
            {
                population.calculateFitness();
                population.naturalSelection();
                population.mutateBabies();
            }
            else
            {
                population.update();
                population.draw(e.Graphics);
            }
        }

        //When the desired delay (for 100fps) has passed, run the OnPaint function
        private void OnTimedEvent (Object source, System.Timers.ElapsedEventArgs e)
        {
            Invalidate();
        }

        //Start the timer, initialize the target and init the population of 1000 dots with some nessecary data
        private void Form1_Shown (object sender, EventArgs e)
        {
            target = new Target(20, ClientSize.Width / 2, 100);
            population = new Population(1000, this, ClientSize.Width / 3, ClientSize.Height - 200, 5, 200, ClientSize.Width, ClientSize.Height);

            form1 = this;

            //My dots were flickering, so I had to turn this on
            DoubleBuffered = true;


            //Handle the timer
            timer = new System.Timers.Timer();
            timer.Interval = 10;

            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
    }
}
