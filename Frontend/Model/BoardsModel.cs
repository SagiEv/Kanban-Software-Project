using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardsModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> userBoards { get; set; }
        public ObservableCollection<BoardModel> otherBoards { get; set; }


        private int boardidcount = 2;
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
                RaisePropertyChanged("BoardName");
            }
        }
        private string _owner;
        public string Owner
        {
            get => _owner;
            set
            {
                this._owner = value;
                RaisePropertyChanged("Owner");
                //need to change
                Controller.UpdateOwner(UserEmail, value, BoardName);
            }
        }
        private string UserEmail; //storing this user here is an hack becuase static & singletone are not allowed.
        public BoardsModel(BackendController controller, int id, string boardName, string owner, string user_email) : base(controller)
        {
            Id = id;
            BoardName = boardName;
            Owner = owner;
            UserEmail = user_email;
        }

        public BoardsModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            UserEmail = user.Email;
            //userBoards = new ObservableCollection<BoardModel>(controller.GetAllBoards(user.Email).
            //    Select((c, i) => new BoardModel(controller, controller.GetBoard(user.Email, i), user)).ToList());
            List<BoardToSend> B2Suser = controller.GetAllBoardsOfUser(user.Email); // Your list of BoardToSend objects
            List<BoardModel> userboards = new List<BoardModel>();
            foreach (BoardToSend b in B2Suser)
            {
                int boardId = b.id;
                BoardModel boardModel = new BoardModel(controller, controller.GetBoard(user.Email, boardId), user);
                userboards.Add(boardModel);
            }
            userBoards = new ObservableCollection<BoardModel>(userboards);
            userBoards.CollectionChanged += HandleChange;
            List<BoardToSend> B2Sothers = controller.GetAllBoardsComplement(user.Email); // Your list of BoardToSend objects
            List<BoardModel> others = new List<BoardModel>();
            foreach (BoardToSend b in B2Sothers)
            {
                int boardId = b.id;
                BoardModel boardModel = new BoardModel(controller, controller.GetBoard(user.Email, boardId), user);
                others.Add(boardModel);
            }
            otherBoards = new ObservableCollection<BoardModel>(others);
            //otherBoards = new ObservableCollection<BoardModel>(controller.GetAllBoardsComplement(user.Email).
            //    Select((c, i) => new BoardModel(controller, controller.GetBoard(user.Email, i), user)).ToList());
            //otherBoards.CollectionChanged += HandleChange;
        }

        public BoardsModel(BackendController controller, (int Id, string BoardName, string Owner) board, UserModel user) : this(controller, board.Id, board.BoardName, board.Owner, user.Email)
        {
        }


        public void RemoveBoard(BoardModel t)
        {

            userBoards.Remove(t);

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (BoardModel y in e.OldItems)
                    {

                        Controller.DeleteBoard(user.Email, y.BoardName);
                    }
                }

            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.OldItems != null)
                {
                    foreach (BoardModel y in e.OldItems)
                    {

                        Controller.CreateBoard(user.Email, y.BoardName);
                    }
                }

            }
        }

        internal void CreateBoard(string newBoardName)
        {
            if (newBoardName != null)
            {
                //userBoards.Add(new BoardModel(Controller, user, 0,newBoardName, user.Email, user.Email));
                Controller.CreateBoard(user.Email, newBoardName);
                userBoards.Add(new BoardModel(Controller, user, boardidcount, newBoardName, user.Email, user.Email));
                boardidcount++;
                userBoards.CollectionChanged += HandleChange;
                //RaisePropertyChanged("Boards of user");
                UpdateUserBoards();
                //userBoards = Controller.GetAllBoardsOfUser(UserEmail);
            }
        }

        private void UpdateUserBoards()
        {
            List<BoardToSend> B2Suser = Controller.GetAllBoardsOfUser(user.Email); // Your list of BoardToSend objects
            List<BoardModel> userboards = new List<BoardModel>();
            foreach (BoardToSend b in B2Suser)
            {
                int boardId = b.id;
                BoardModel boardModel = new BoardModel(Controller, Controller.GetBoard(user.Email, boardId), user);
                userboards.Add(boardModel);
            }
            userBoards = new ObservableCollection<BoardModel>(userboards);
            userBoards.CollectionChanged += HandleChange;
        }

        private void UpdateOtherBoards()
        {
            List<BoardToSend> B2Sothers = Controller.GetAllBoardsComplement(user.Email); // Your list of BoardToSend objects
            List<BoardModel> others = new List<BoardModel>();
            foreach (BoardToSend b in B2Sothers)
            {
                int boardId = b.id;
                BoardModel boardModel = new BoardModel(Controller, Controller.GetBoard(user.Email, boardId), user);
                others.Add(boardModel);
            }
            otherBoards = new ObservableCollection<BoardModel>(others);
        }



        internal void CreateBoard(BoardModel selectedBoard)
        {
            if (selectedBoard != null && !otherBoards.Contains(selectedBoard))
            {
                userBoards.Add(selectedBoard);
                RaisePropertyChanged("Boards of user");
                //otherBoards.Remove(selectedBoard);
            }
        }

        internal void JoinBoard(BoardModel selectedBoard)
        {
            //Controller.JoinBoard(selectedBoard);
            //userBoards.Add(selectedBoard);
            //otherBoards.Remove(selectedBoard);
            if (selectedBoard != null && !userBoards.Contains(selectedBoard))
            {
                //Controller.JoinBoard(UserEmail,selectedBoard.Tid);
                userBoards.Add(selectedBoard);
                RaisePropertyChanged("Boards of user");
                otherBoards.Remove(selectedBoard);
                RaisePropertyChanged("Boards of others");
                UpdateUserBoards();
                UpdateOtherBoards();
                //List<BoardToSend> B2Sothers = Controller.GetAllBoardsComplement(user.Email); // Your list of BoardToSend objects
                //List<BoardModel> others = new List<BoardModel>();
                //foreach (BoardToSend b in B2Sothers)
                //{
                //    int boardId = b.id;
                //    BoardModel boardModel = new BoardModel(Controller, Controller.GetBoard(user.Email, boardId), user);
                //    others.Add(boardModel);
                //}
                //otherBoards = new List<BoardModel>(others);
            }
        }

        //internal void GetMembers(string email, string bname)
        //{
        //    //Controller.
        //}


    }
}
