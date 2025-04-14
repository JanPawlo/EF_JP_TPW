using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PresentationModel
{
    internal class ModelImplementation : ModelAbstractAPI
    {
        private readonly Logic.LogicAbstractAPI layerBellow = null;

        public override void start(int numOfBalls)
        {
            layerBellow.start(numOfBalls);
        }
    }
    
    }
}
