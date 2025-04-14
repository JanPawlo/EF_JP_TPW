using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class LogicImplementation : LogicAbstractAPI
    {
        private readonly Data.DataAbstractAPI LayerBelow = null;
        private Timer timer;
        // pamietac o spojnosci z MainWindow.xaml
        private readonly int width = 300, height = 400;
        private readonly object locker = new();

        public LogicImplementation()
        {
            LayerBelow = Data.DataAbstractAPI.Create();
        }

        public override void start(int numOfBalls)
        {
            //Not yet implemented
            Console.WriteLine($"Starting with {numOfBalls} balls.");
            timer = new Timer(Update, null, 0, 16);
        }

        // Do poprawienia (losowanie, ograniczenie wzgledem planszy)
        private void Update(object state)
        {
            lock (locker)
            {
                Random rand = new Random();
                foreach (var ball in LayerBelow.GetBalls())
                {
                    ball.x = rand.NextDouble() * width;
                    ball.y = rand.NextDouble() * height;
                }
            }
        }

        public override IReadOnlyList<Data.Ball> GetBalls()
        {
            lock (locker)
            {
                return LayerBelow.GetBalls();
            }
        }

    }
}
