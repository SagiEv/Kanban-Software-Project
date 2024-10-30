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
    /// Interaction logic for TransferOwnershipView.xaml
    /// </summary>
    public partial class TransferOwnershipView : Window
    {
        private TransferOwnershipViewModel viewModel;
        public TransferOwnershipView(UserModel user, BoardModel board)
        {
            this.viewModel = new TransferOwnershipViewModel(user, board);
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BoardView boardViewWindow = new BoardView(viewModel.User, viewModel.Board);
            boardViewWindow.Show();
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.TransferOwnership();
            
            BoardView boardViewWindow = new BoardView(viewModel.User, viewModel.Board);
            boardViewWindow.Show();
            Close();
        }
    }
}
