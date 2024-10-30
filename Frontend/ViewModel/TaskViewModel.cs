using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.Model;

namespace Frontend.ViewModel
{
     class TaskViewModel: NotifiableObject
    {
        private int _id;
        public int Id 
        { 
            get => _id; 
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        public TaskModel Task { get; set; }
        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            set
            {
                this._boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        private int _column;
        public int Column
        {
            get => this._column;
            set
            {
                this._column = value;
                RaisePropertyChanged("Column");
            }
        }
        public BackendController Controller { get; private set; }

        private bool isReadOnly = true;

        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                if (isReadOnly == false || userCookie.Email == _assignee) { 
                    isReadOnly = value;
                    RaisePropertyChanged("IsReadOnly");
            }
                //if (userCookie.Email == _assignee)
                //{
                //    if (isReadOnly != value)
                //    {
                //        isReadOnly = value;
                //        RaisePropertyChanged("IsReadOnly");

                //    }
                //}
            }
        }

        public UserModel userCookie;
        public BoardModel boardOfCurrTask;
        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                _assignee = value;
                RaisePropertyChanged("Assignee");
            }
        }

        public TaskViewModel(UserModel u, BoardModel b)
        {
            userCookie = u;
            Controller = u.Controller;
            boardOfCurrTask = b;
            //isReadOnly = true;
            //TaskModel
            //this.BoardName = selectedTask.BoardName;
            //this.Assignee = selectedTask.Assignee;
            //this.Id = selectedTask.Tid;
            //this.Column = selectedTask.Column;
        }

        public TaskViewModel(UserModel u, BoardModel b,TaskModel selectedtask)
        {
            userCookie = u;
            Controller = u.Controller;
            boardOfCurrTask = b;
            Task = selectedtask;
            //isReadOnly = true;
            //TaskModel
            this.BoardName = selectedtask.BoardName;
            this.Assignee = selectedtask.Assignee;
            this.Id = selectedtask.Tid;
            isReadOnly = !(u.Email == _assignee);
            //this.Column = selectedtask.Column;
        }


    }

}
