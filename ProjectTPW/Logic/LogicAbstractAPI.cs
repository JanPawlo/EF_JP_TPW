namespace Logic
{
    public abstract class LogicAbstractAPI
    {
        public static LogicAbstractAPI Create()
        {
            return new LogicImplementation();
        }

        // Operacje interkatywne
        public abstract void start(int numOfBalls);
        
        // Operacje reaktywne
        public abstract IReadOnlyList<Data.Ball> GetBalls(); // Mozliwe ze bedzie trzeba stworzyc LogicBalls, aby wyzsze wartswy nie miały dostępu do danych



    }
}
