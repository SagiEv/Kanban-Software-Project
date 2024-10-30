using Frontend.Model;
using Frontend.View;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            this.viewModel = (MainViewModel)DataContext;

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Login();
            if (u != null)
            {
                BoardsView boardsView = new BoardsView(u);
                boardsView.Show();
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            UserModel u = viewModel.Register();
            if (u != null)
            {
                BoardsView boardsView = new BoardsView(u);
                boardsView.Show();
                this.Close();
            }

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextChangedLog(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            Login.IsEnabled = !string.IsNullOrEmpty(textBox.Text);
        }

    }
}
