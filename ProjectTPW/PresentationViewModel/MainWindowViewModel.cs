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