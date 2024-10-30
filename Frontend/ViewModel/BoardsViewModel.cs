using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    internal class BoardsViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public UserModel userCookie { get; }

        private UserModel user;
        public SolidColorBrush BackgroundColor {
            get
            {
                //if in my boards and user.Email==b.bid in boards then green else yellow
                return new SolidColorBrush(user.Email.Contains("none") ? Colors.Blue : Colors.Red);                
            }
        }
        public string Email { get; private set; }

        public BoardsModel Boards { get; private set; }
        public string Title { get; private set; }
        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        private string _newBoard;

        public string NewBoard
        {
            get { return _newBoard; }
            set
            {
                _newBoard = value;
                RaisePropertyChanged(nameof(NewBoard)); // Notify property changed
            }
        }

        internal void Logout()
        {
            this.controller.Logout(user.Email);
        }

        public BoardsViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            this.userCookie = user;
            Title = $"Welcome {user.Email}\n   Boards Management";
            Boards = user.GetBoards();
            Email = user.Email;
        }

        public void RemoveBoard()
        {

            try
            {
                Boards.RemoveBoard(SelectedBoard);
            }
            catch (Exception e )
            {
                MessageBox.Show("Cannot remove board. " + e.Message);
            }
            
        }

        internal void CreateBoard()
        {
            try
            {
                Boards.CreateBoard(NewBoard);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot create board. " + e.Message);
            }
        }

        internal void JoinBoard()
        {
            try
            {
                Boards.JoinBoard(SelectedBoard);
                //controller.JoinBoard(user.Email,SelectedBoard.Tid);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot join this board. " + e.Message);
            }
        }
    }
}