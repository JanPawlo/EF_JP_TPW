//__________________________________________________________________________________________
//
//  Copyright 2024 Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and to get started
//  comment using the discussion panel at
//  https://github.com/mpostol/TP/discussions/182
//__________________________________________________________________________________________

using System;
using System.Windows;
using TP.ConcurrentProgramming.Presentation.ViewModel;

namespace TP.ConcurrentProgramming.PresentationView
{
  /// <summary>
  /// View implementation
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      Random random = new Random();
      InitializeComponent();
      MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
      double screenWidth = SystemParameters.PrimaryScreenWidth;
      double screenHeight = SystemParameters.PrimaryScreenHeight;
    }

    private bool _startClicked = false;
    private bool _restartClicked = true;
  public void StartAction(object sender, RoutedEventArgs e)
  {
            if (_startClicked)
                return;
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;

            int numberOfBalls = (int?)(upDownSelectBallNr.Value) ?? (int)(upDownSelectBallNr.DefaultValue);


            viewModel.Start(numberOfBalls);
            _startClicked = true;
            _restartClicked = false;
    
  }

        public void RestartAction(object sender, RoutedEventArgs e)
        {
            if (_restartClicked)
                return;
            MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            viewModel.Stop();
            // Reset the start clicked flag
            _startClicked = false;
        }





        /// <summary>
        /// Raises the <seealso cref="System.Windows.Window.Closed"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnClosed(EventArgs e)
    {
      if (DataContext is MainWindowViewModel viewModel)
        viewModel.Dispose();
      base.OnClosed(e);
    }
  }
}