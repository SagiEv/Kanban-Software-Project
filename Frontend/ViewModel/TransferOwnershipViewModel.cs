using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    internal class TransferOwnershipViewModel : NotifiableObject
    {
        private UserModel _user;
        public UserModel User{
            get => _user;
            set{ _user = value; }
            }
        private BoardModel _board;
        public BoardModel Board
        {
            get => _board;
            set
            {
                _board = value;
            }
        }


        public TransferOwnershipViewModel(UserModel user, BoardModel board) 
        {
            this.User = user;
            this.Board = board;
        }

        private string _selectedMember;
        public string SelectedMember
        {
            get => _selectedMember;
            set { 
                this.SelectedMember = value;
                RaisePropertyChanged("Selected Member");
            }

        }

        internal void TransferOwnership()
        {
            if (_selectedMember != null)
            {
                Board.Owner = SelectedMember;
            }
        }
    }
}
