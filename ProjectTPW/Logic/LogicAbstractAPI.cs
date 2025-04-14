namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI GetLogicLayer()
        {
            return new LogicImplementation();
        }

        // Operacje interkatywne
        public abstract void start(int numOfBalls);
        //to jest chyba interkatywne?
        public abstract List<Ball> GetBalls();
        
        // Operacje reaktywne
        
        // to jest chyba reaktywne?
        public abstract void createBalls(int numOfBalls);


    }
}
