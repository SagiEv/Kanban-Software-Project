using Frontend.ViewModel;
using Frontend.Model;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for TaskView.xaml
    /// </summary>
    public partial class TaskView : Window
    {
        private TaskViewModel viewModel;
        //private UserModel userCookie;
        //private BoardModel boardModel;
        public TaskView(UserModel user, BoardModel board, bool newTask)
        {
            //this.boardModel = board;
            //this.userCookie = user;
            InitializeComponent();
            this.viewModel = new TaskViewModel(user,board);
            this.DataContext = viewModel;
            viewModel.IsReadOnly = false;
        }

        public TaskView(UserModel user, BoardModel board,TaskModel t)
        {
            //this.boardModel = board;
            //this.userCookie = user;
            InitializeComponent();
            this.viewModel = new TaskViewModel(user, board,t);
            this.DataContext = viewModel;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //viewModel.IsReadOnly = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BoardView boardViewWindow = new BoardView(viewModel.userCookie, viewModel.boardOfCurrTask);
            boardViewWindow.Show();
            Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.userCookie.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
