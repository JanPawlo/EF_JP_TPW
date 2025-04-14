using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class LogicImplementation : LogicAbstractAPI
    {
        public override void start(int numOfBalls)
        {
            //Not yet implemented
            Console.WriteLine($"Starting with {numOfBalls} balls.");
            //Presumably call createBalls() here

        }


        public override void createBalls(int numOfBalls)
        {
            // Implementacja metody createBalls
            // TBD
            Console.WriteLine($"Creating {numOfBalls} balls.");
        }


    }
}
