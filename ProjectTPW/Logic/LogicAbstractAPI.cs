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


        // Operacje reaktywne
        // to jest chyba reaktywne?
        public abstract void createBalls(int numOfBalls);


    }
}
