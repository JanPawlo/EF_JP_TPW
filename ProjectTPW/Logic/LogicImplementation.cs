using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class LogicImplementation : LogicAbstractAPI
    {
        private readonly List<Ball> balls = new();
        private Timer timer;
        // pamietac o spojnosci z MainWindow.xaml
        private readonly int width = 300, height = 400;
        private readonly object locker = new();


        public override void start(int numOfBalls)
        {
            //Not yet implemented
            Console.WriteLine($"Starting with {numOfBalls} balls.");
            timer = new Timer(Update, null, 0, 16);
        }

        public override void createBalls(int numOfBalls)
        {
            var rand = new Random();
            for (int i = 0; i < numOfBalls; i++)
            {
                balls.Add(new Ball
                {
                    X = rand.NextDouble() * width,
                    Y = rand.NextDouble() * height,
                    VelocityX = rand.NextDouble() * 4 - 2,
                    VelocityY   = rand.NextDouble() * 4 - 2
                });
            }
        }

        public override List<Ball> GetBalls() => balls;


        private void Update(object state)
        {
            lock (locker)
            {
                foreach (var ball in balls)
                {
                    ball.X += ball.VelocityX;
                    ball.Y += ball.VelocityY;

                    if (ball.X <= 0 || ball.X >= width) ball.VelocityX *= -1;
                    if (ball.Y <= 0 || ball.Y >= height) ball.VelocityY *= -1;
                }
            }
        }

    }
}
