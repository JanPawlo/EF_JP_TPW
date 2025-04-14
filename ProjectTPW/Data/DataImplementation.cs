using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        public override void Start(int numOfBalls)
        {
            Random rand = new Random();
            for (int i = 0; i < numOfBalls; i++)
            {
                balls.Add(new Ball
                {
                    x = rand.NextDouble() * 300,
                    y = rand.NextDouble() * 400,
                    Radius = rand.NextDouble() * 20 + 5
                });
            }
        }

        private List<Ball> balls = [];
        public override List<Ball> GetBalls()
        {
            return balls;
        }
    }
}
