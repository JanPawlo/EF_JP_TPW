namespace PresentationViewModel
{
    public class MainWindowViewModel
    {
        private PresentationModel.ModelAbstractAPI modelLayer;

        public void start(int numOfBalls)
        {
            modelLayer.start(numOfBalls);
        }


    }
}
