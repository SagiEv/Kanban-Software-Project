using Frontend.Model;
using Frontend.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardsView : Window
    {
        private BoardsViewModel viewModel;
        
        public BoardsView(UserModel u)
        {
            InitializeComponent();
            this.viewModel = new BoardsViewModel(u);
            this.DataContext = viewModel;
        }

        private void Create_Button(object sender, RoutedEventArgs e)
        {
            viewModel.CreateBoard();
        }

        private void Remove_Button(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveBoard();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void InProgress_Click(object sender, RoutedEventArgs e)
        {
            InProgressView InProgTab = new InProgressView(viewModel.userCookie);
            InProgTab.Show();
            this.Close();
        }

        private void Transfer_Button(object sender, RoutedEventArgs e)
        {

        }

        private void JoinBoard_Click(object sender, RoutedEventArgs e)
        {
            viewModel.JoinBoard();
        }

        private void OtherBoardsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void MyBoardsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }


        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //viewModel.Logout();
            if (viewModel.SelectedBoard != null)
            {
                BoardView boardTab = new BoardView(viewModel.userCookie, viewModel.SelectedBoard);
                boardTab.Show();
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

        //private void TransferOwnership_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    // Show the options popup
        //    TransferOptionsPopup.IsOpen = true;
        //}

        //private void TransferOwnership_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    // Hide the options popup
        //    TransferOptionsPopup.IsOpen = false;
        //}

        private void TransferOwnership_MouseEnter(object sender, MouseEventArgs e)
        {
            // Show the options popup
            TransferOptionsPopup.IsOpen = true;
        }

        private void TransferOwnership_MouseLeave(object sender, MouseEventArgs e)
        {
            // Check if the mouse is still within the button or the popup
            if (!IsMouseWithinButtonOrPopup())
            {
                // Hide the options popup
                TransferOptionsPopup.IsOpen = false;
            }
        }

        private bool IsMouseWithinButtonOrPopup()
        {
            // Check if the mouse is within the bounds of the button or the popup
            Point mousePosition = Mouse.GetPosition(this);
            bool isMouseWithinButton = TransferOwnership.IsMouseOver;
            bool isMouseWithinPopup = TransferOptionsPopup.IsMouseOver;

            return isMouseWithinButton || isMouseWithinPopup;
        }

        private void boardNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            CreateBoard.IsEnabled = !string.IsNullOrEmpty(textBox.Text);
        }
    }
}
