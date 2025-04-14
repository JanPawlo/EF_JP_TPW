using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        // Fabryka
        public static DataAbstractAPI Create()
        {
            return new DataImplementation();
        }
        public abstract void Start(int numOfBalls);
        public abstract List<Ball> GetBalls();


    }
}
