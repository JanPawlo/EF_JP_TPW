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
            // Implementacja metody Start
            // TBD
            Console.WriteLine($"Starting with {numOfBalls} balls.");
        }
    }
}
