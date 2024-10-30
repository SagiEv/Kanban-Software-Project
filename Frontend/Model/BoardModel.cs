
using Frontend.Model;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Frontend
{
    public class BoardModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<TaskModel> TasksBacklog { get; set; }
        public ObservableCollection<TaskModel> TasksInProgress { get; set; }
        public ObservableCollection<TaskModel> TasksDone { get; set; }

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
        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            set
            {
                this._boardName = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _owner;
        public string Owner
        {
            get => _owner;
            set
            {
                if (value != null)
                {
                    this._owner = value;
                    RaisePropertyChanged("Owner");
                    Controller.UpdateOwner(UserEmail, value, BoardName);
                }
            }
        }

        private int _limbacklog;
        public int Lim_backlog
        {
            get => _limbacklog;
            set
            {
                if (value != null)
                {
                    this._limbacklog = value;
                    RaisePropertyChanged("limit");
                    Controller.setLimitBacklog(user.Email, BoardName, value);
                }
            }
        }

        private int _liminprog;
        public int Lim_inprog
        {
            get => _liminprog;
            set
            {
                if (value != null)
                {
                    this._liminprog = value;
                    RaisePropertyChanged("limit");
                    Controller.setLimitInProg(user.Email, BoardName, value);
                }
            }
        }

        private int _limdone;
        public int Lim_done
        {
            get => _limdone;
            set
            {
                if (value != null)
                {
                    this._limdone = value;
                    RaisePropertyChanged("limit");
                    Controller.setLimitDone(user.Email, BoardName, value);
                }
            }
        }

        private List<string> _members;
        public List<string> Members
        {
            get => _members;
            set  { _members = value; RaisePropertyChanged("members"); }
        }

        private string UserEmail; //storing this user here is an hack becuase static & singletone are not allowed.
        public BoardModel(BackendController controller,UserModel u, int id, string boardName, string owner, string user_email) : base(controller)
        {
            this.user = u;
            Id = id;
            BoardName = boardName;
            Owner = owner;
            Members = controller.GetAllMembersInBoards(boardName);
            List<TaskToSend> backlog = controller.GetTasksInBacklog(user_email, boardName);
            List<TaskModel> others = new List<TaskModel>();
            foreach(TaskToSend t in backlog)
            {
               others.Add(new TaskModel(controller, user, boardName, t,0));
            }
            TasksBacklog = new ObservableCollection<TaskModel>(others);
            TasksBacklog.CollectionChanged += HandleChange;

            List<TaskToSend> inprog = controller.GetTasksInProgress(user_email, boardName);
            List<TaskModel> others2 = new List<TaskModel>();
            foreach (TaskToSend t in inprog)
            {
                others2.Add(new TaskModel(controller, user, boardName, t, 1));
            }
            TasksInProgress = new ObservableCollection<TaskModel>(others2);
            TasksInProgress.CollectionChanged += HandleChange;

            List<TaskToSend> done = controller.GetTasksInDone(user_email, boardName);
            List<TaskModel> others3 = new List<TaskModel>();
            foreach (TaskToSend t in done)
            {
                others3.Add(new TaskModel(controller, user, boardName, t, 2));
            }
            TasksDone = new ObservableCollection<TaskModel>(others3);
            TasksDone.CollectionChanged += HandleChange;

            //Lim_backlog = Controller.GetLimitBacklog(user.Email, BoardName);
            //Lim_inprog = Controller.GetLimitInProg(user.Email, BoardName);
            //Lim_done = Controller.GetLimitDone(user.Email, BoardName);
            //UserEmail = user_email;
            //Tasks = tasks; 
            //Tasks.CollectionChanged += HandleChange;
        }

        public BoardModel(BackendController controller, (int Id, string BoardName, string Owner) board, UserModel user) : this(controller, user, board.Id, board.BoardName, board.Owner, user.Email) { }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BoardModel y in e.OldItems)
                {

                    //Controller.AdvanceTask(user.Email,BoardName,taskid,col);                    
                }

            }
        }

    }
}
