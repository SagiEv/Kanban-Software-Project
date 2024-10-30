using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for InProgressView.xaml
    /// </summary>
    public partial class InProgressView : Window
    {
        private InProgressViewModel viewModel;
        private UserModel userCookie;

        public InProgressView(UserModel user)
        {
            this.userCookie = user;
            this.viewModel = new InProgressViewModel(user);
            InitializeComponent();
            this.DataContext = viewModel;
        }


        // not neccessery right now 
       /* public void ClickOnTask(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem is TaskModel selectedTask)
            {

                TaskView task = new TaskView(viewModel.User);
                task.Show();
                this.Close();
            }
        }*/

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
               BoardsView boardsViewWindow = new BoardsView(viewModel.User);
               boardsViewWindow.Show();
               Close();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            userCookie.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
