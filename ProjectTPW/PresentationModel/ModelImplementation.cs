using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Logic;
using static System.Net.Mime.MediaTypeNames;
using Data;


namespace PresentationModel
{
    public class ModelImplementation : ModelAbstractAPI
    {
        
        private readonly LogicAbstractAPI LogicLayer = null;


        public ObservableCollection<PresentationViewModel.BallViewModel> Balls { get; } = new();

        public override void start(int numOfBalls)
        {
            LogicLayer.start(numOfBalls);
            Task.Run(UpdateLoop);
        }

        //konstruktor
        public ModelImplementation()
        {
            LogicLayer = LogicAbstractAPI.Create();
            // LogicLayer.Start();

            // Task.Run(UpdateLoop);
        }


        private async Task UpdateLoop()
        {
            while (true)
            {
                var logicBalls = LogicLayer.GetBalls();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Balls.Count != logicBalls.Count)
                    {
                        Balls.Clear();
                        foreach (var b in logicBalls)
                            Balls.Add(new PresentationViewModel.BallViewModel());
                    }

                    for (int i = 0; i < logicBalls.Count; i++)
                    {
                        Balls[i].x = logicBalls[i].X;
                        Balls[i].y = logicBalls[i].Y;
                    }
                });

                await Task.Delay(16);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
