using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using Logic;

// Dziala miedzy UI a logiką, posiada listenery(?)
//idk czym sie roznia presentationModel i presentationViewModel

namespace PresentationViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private PresentationModel.ModelAbstractAPI modelLayer;

        private readonly LogicAbstractAPI logic;

        public ObservableCollection<BallViewModel> Balls { get; } = new();

        //konstruktor
        public void MainViewModel()
        {
            logic = LogicAbstractAPI.Create();
            logic.CreateBalls(10);
            logic.Start();

            Task.Run(UpdateLoop);
        }

        private async Task UpdateLoop()
        {
            while (true)
            {
                var logicBalls = logic.GetBalls();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Balls.Count != logicBalls.Count)
                    {
                        Balls.Clear();
                        foreach (var b in logicBalls)
                            Balls.Add(new BallViewModel());
                    }

                    for (int i = 0; i < logicBalls.Count; i++)
                    {
                        Balls[i].X = logicBalls[i].X;
                        Balls[i].Y = logicBalls[i].Y;
                    }
                });

                await Task.Delay(16);
            }
        }

        /*
        public void start(int numOfBalls)
            {
                modelLayer.start(numOfBalls);
            }
        */



        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class BallViewModel : INotifyPropertyChanged
    {
        private double _x, _y;

        public double X { get => _x; set { _x = value; OnPropertyChanged(); } }
        public double Y { get => _y; set { _y = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
} 