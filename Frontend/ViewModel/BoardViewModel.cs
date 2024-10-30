using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        public UserModel user { get; }

        public BoardsModel Boards { get; private set; }
        public string Title { get; private set; }
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
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

        private BoardModel _board;
        public BoardModel Board { get; set; }

        internal void Logout()
        {

        }

        public BoardViewModel(UserModel user,BoardModel b)
        {
            this.controller = user.Controller;
            this.user = user;
            Board = b;
            Title = $"ID:{Board.Id} | NAME: {Board.BoardName} Board | Owner: {Board.Owner}";
            Boards = user.GetBoards();
        }

        public void AddTask()
        {

            //try
            //{
             //   Boards.RemoveBoard(SelectedTask);
            //}
           // catch (Exception e)
           // {
           //     MessageBox.Show("Cannot create message. " + e.Message);
           // }

        }

        internal void AdvanveTask()
        {
            //try
            //{
            //    Boards.RemoveBoard(SelectedTask);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("Cannot create message. " + e.Message);
           // }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                //if in my boards and user.Email==b.bid in boards then green else yellow
                return new SolidColorBrush(user.Email.ToLower().Equals(SelectedTask.Assignee) ? Colors.Blue : Colors.Pink);
            }
        }

        public string ZeroToInfinityConverter(object value)
        {
            if (value is int intValue && intValue == 0)
            {
                return "Infinity";
            }
            else
            {
                return value.ToString();
            }
        }
    }

}

