using System.Windows;
using System;
using PresentationModel;

//kontrola UI zaprojektowanego w MainWindow.xaml

namespace PresentationView
{


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ModelImplementation();
        }






        // Reakcja na wcisniecie przycisku
        private void btn0_Click(object sender, RoutedEventArgs e)
        {
            // Odwolanie do obiektow z xaml'a za pomoca atrybutu "Name"
            testHello.Text = "New Hello!";
        }

    }
}