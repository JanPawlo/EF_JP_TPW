using System.Windows;
using System;

namespace PresentationView
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }


        // Reakcja na wcisniecie przycisku
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            // Odwolanie do obiektow z xaml'a za pomoca atrybutu "Name"
            testHello.Text = "New Hello!";
        }

    }
}