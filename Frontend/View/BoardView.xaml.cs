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
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        private BoardViewModel viewModel;
        

        public BoardView(UserModel u, BoardModel b)
        {
            InitializeComponent();
            //this.u = u; 
            this.viewModel = new BoardViewModel(u,b);
            this.DataContext = viewModel;
            //this.boardModel = b;
        }
        private void AddTask_Button(object sender, RoutedEventArgs e)
        {
            viewModel.AddTask();
        }
        private void Button_Click(object sender, RoutedEventArgs e, UserModel u)
        {
            viewModel.Logout();
            BoardsView boardsView = new BoardsView(u);
            this.Close();
            boardsView.Show();
            
        }

        //private void AddTask_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Returned_Click(object sender, RoutedEventArgs e) //Return to the previous window
        {

        }

        private void Task_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.SelectedTask != null)
            {
                //viewModel.Boards (add to TaskView)
                //viewModel.Board;
                /*viewModel.SelectedTask*/;
                TaskView taskView = new TaskView(viewModel.user, viewModel.Board, viewModel.SelectedTask);
                taskView.Show();
                this.Close();
            }
        }


        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            // Change the mouse cursor to a specific style (e.g., Hand)
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            // Restore the default mouse cursor
            Mouse.OverrideCursor = null;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.user.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BoardsView boards = new BoardsView(viewModel.user);
            boards.Show();
            this.Close();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
                //viewModel.Boards (add to TaskView)
                //viewModel.Board;
                TaskView taskView = new TaskView(viewModel.user, viewModel.Board,true);
                taskView.Show();
                this.Close();
           
        }

        private void AssigneeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Show the options popup
            AssigneeOptionsPopup.IsOpen = true;
        }

        private void AssigneeButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // Check if the mouse is still within the button or the popup
            if (!IsMouseWithinButtonOrPopup())
            {
                // Hide the options popup
                AssigneeOptionsPopup.IsOpen = false;
            }
        }

        private bool IsMouseWithinButtonOrPopup()
        {
            Point mousePosition = Mouse.GetPosition(this);
            bool isMouseWithinButton = AssigneeButton.IsMouseOver;
            bool isMouseWithinPopup = AssigneeOptionsPopup.IsMouseOver;

            return isMouseWithinButton || isMouseWithinPopup;
        }


    }
}
