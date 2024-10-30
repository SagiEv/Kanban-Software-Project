using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<string> Members { get; set; }

        /*private string _emailToSend;
        public string EmailToSend
        {
            get => _emailToSend;
            set
            {
                _emailToSend = value;
                RaisePropertyChanged("EmailToSend");
            }
        }*/


        private int _id;
        public int Tid
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string _title;
        public string Tname
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
                //Controller.UpdateTaskTitle(user.Email,BoardName,Column,Tid,value);
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
                //Controller.UpdateDescription(user.Email,BoardName,Column,Tid,Description);
            }
        }
        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                this._creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
                //Controller.UpdateDueDate(user.Email, BoardName, Column, Tid, value);
            }
        }

        private int _bid;
        public int Bid
        {
            get => _bid;
            private set
            {
                this._bid = value;
                RaisePropertyChanged("BID");
            }
        }

        /////////////////////////
        /// <summary>
        /// 
        /// </summary>
        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            private set
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
        ////////////////////////


        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set
            {
                this._assignee = value;
                RaisePropertyChanged("Assignee");
                //Controller.SetAssignee(user.Email,BoardName,Column, this.Tid, value);
            }
        }

        public TaskModel(BackendController controller, UserModel user, ObservableCollection<string> members, int id, string title, string desc, DateTime creation, DateTime due, string assignee) : base(controller)
        {
            this.user = user;
            this.Members = members;
            this.Tid = id;
            this.Tname = title;
            this.Description = desc;
            this.CreationTime = creation;
            this.DueDate = due;
            this.Assignee = assignee;
        }

        public TaskModel(BackendController controller, UserModel user, string boardName, TaskToSend t,int col) : base(controller)
        {
            this.user = user;
            BoardName = boardName;
            (int,string,DateTime,DateTime,string,string) getTaskFromController = controller.GetTaskInColumn(user.Email,boardName, t.id, col);
            this.Tid = getTaskFromController.Item1;
            this.BoardName = boardName;
            this.CreationTime = getTaskFromController.Item3;
            this.DueDate = getTaskFromController.Item4;
            this.Description = getTaskFromController.Item5;
            string assign = getTaskFromController.Item6;
            this.Assignee = assign.Substring(0,assign.Length - 2);
            this.Tname = t.title;
        }

        //public TaskModel(BackendController controller, UserModel user,string bname,int bid, ObservableCollection<string> members, int id, string title, string desc, DateTime creation, DateTime due, string assignee) : base(controller)
        //{
        //    this.BoardName = bname;
        //    this.Bid = bid;
        //    this.user = user;
        //    this.Members = members;
        //    this.Tid = id;
        //    this.Tname = title;
        //    this.Description = desc;
        //    this.CreationTime = creation;
        //    this.DueDate = due;
        //    this.Assignee = assignee;
        //}


        //public TaskModel(BackendController controller, string bname, int bid, UserModel user, ObservableCollection<string> members, int id, string title, string desc, DateTime creation, DateTime due, string assignee) : base(controller)
        //{
        //    this.Bid = bid;
        //    this.BoardName = bname;
        //    this.user = user;
        //    this.Members = members;
        //    this.Tid = id;
        //    this.Tname = title;
        //    this.Description = desc;
        //    this.CreationTime = creation;
        //    this.DueDate = due;
        //    this.Assignee = assignee;
        //}

        //public TaskModel(BackendController controller, UserModel user, int id, string title, string desc, DateTime creation, DateTime due, string assignee) : base(controller)
        //{
        //    this.user = user;
        //    this.Tid = id;
        //    this.Tname = title;
        //    this.Description = desc;
        //    this.CreationTime = creation;
        //    this.DueDate = due;
        //    this.Assignee = assignee;
        //}

        //public TaskModel(BackendController controller, UserModel user, int bid, string bname, int id, string title, string desc, DateTime creation, DateTime due, string assignee) : base(controller)
        //{
        //    this.user = user;
        //    this.BoardName = bname;
        //    this.Bid = bid;
        //    this.Tid = id;
        //    this.Tname = title;
        //    this.Description = desc;
        //    this.CreationTime = creation;
        //    this.DueDate = due;
        //    this.Assignee = assignee;
        //    //this.column = ?
        //    List<string> membersToSend = user.Controller.GetAllMembersInBoards(user.Controller.GetBoard(user.Email, id).boardName);
        //    this.Members = new ObservableCollection<string>(membersToSend);

        //}

        //public TaskModel(BackendController controller, UserModel user, List<string> list, int tid, string title) : base(controller)
        //{
        //    //this.BoardName = Bid;
        //    this.user = user;
        //    this.Members = new ObservableCollection<string>(list);
        //    this.Tid = tid;
        //    this.Tname = title;
        //}

        //THIS CONSTRUCTOR ONLY USES THE INPROGRESS FEATURE SO IT DOESN'T CONTAIN ALL THE INFO OF TASK!!!
        //public TaskModel(BackendController controller, UserModel user,TaskToSend ts) : base(controller)
        //{
        //    this.user = user;
        //    this.Tid = ts.id;
        //    this.Tname = ts.title;

        //}

        //CONSTRUCTOR FOR IN PROGRESS
        //public TaskModel(BackendController controller, UserModel user, string bname, int bid, ObservableCollection<string> members, int id, string title) : base(controller)
        //{
        //    this.Bid = bid;
        //    this.BoardName = bname;
        //    this.user = user;
        //    this.Members = members;
        //    this.Tid = id;
        //    this.Tname = title;
        //}
    }
}
