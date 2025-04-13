namespace Logic
{
    internal abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI GetLogicLayer()
        {
            return new LogicImplementation();
        }

        // Operacje interkatywne
        public abstract void createBalls(int numOfBalls);

        // Operacje reaktywne

        
           
    }
}
