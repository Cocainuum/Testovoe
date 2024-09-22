using System.Windows;
using ClientApp.ViewModels;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = _viewModel = mainViewModel;
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            await _viewModel.OnLoad();
        }
    }
}