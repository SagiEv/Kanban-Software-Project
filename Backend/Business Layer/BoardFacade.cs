using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using log4net;
using System.Collections;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using System.Linq;
using System.Xml.Linq;
using IntroSE.Kanban.Backend.Backend.Business_Layer;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Text.RegularExpressions;

namespace Kanban.Backend.Business_Layer
{
    [Serializable]
    internal class BoardFacade
    {
        private Dictionary<string, Dictionary<string, Board>> usersBoards;
        //private Dictionary<int, List<string>> members;
        private Dictionary<int, Board> boards;

        private int boardIdCounter;
        private int taskIdCounter;

        private User2BoardsController user2BoardsController;
        private BoardController boardController;
        private TaskController taskController;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardFacade()
        {
            log.Debug("initiating board facade");
            boardController = new BoardController();
            taskController = new TaskController();
            user2BoardsController = new User2BoardsController();

            usersBoards = new Dictionary<string, Dictionary<string, Board>>();//<email, <nameOfBoard, Board>>????
            boards = new Dictionary<int, Board>();
            //members = new Dictionary<int, List<string>>();


            LoadData();

            log.Info($"initiated boardFacade, currently {taskIdCounter} tasks on board.");
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        internal void LoadData()
        {

            //Load all boards from database:
            List<BoardDTO> boardsDB = boardController.SelectAllBoards();
            foreach (BoardDTO b in boardsDB)
            {
                Board board = new Board(b);
                boards.Add(b.BoardId, board);
            }

            taskIdCounter = taskController.GetMaxTaskId() + 1;
            boardIdCounter = boardController.GetMaxBoardId() + 1;

            //Load usersBoards from database:
            List<User2BoardDTO> users2boardsDB = user2BoardsController.SelectAllUsers2Boards();
            foreach (User2BoardDTO u2b in users2boardsDB)
            {
                Board b = new Board(u2b.boardName, u2b.BoardId, u2b.email);
                if (!usersBoards.ContainsKey(u2b.email))
                {
                    usersBoards.Add(u2b.email, new Dictionary<string, Board>());
                }
                //this will restore all the tasks from the table using the bid to track the tasks
                //b.LoadData();
                usersBoards[u2b.email].Add(u2b.boardName, b);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <exception cref="KanException"></exception>
        /// <exception cref="Exception"></exception>
        internal void CreateBoard(string email, string name)
        {
            //checkUserExists(email);
            //checkNotLoggedIn(loggedIn);
            if (name == null)
            {
                log.Error("board name can't be null!");
                throw new KanException("Board name can't be null!");
            }
            if (name.Length == 0)
            {
                log.Error("board name can't be empty!");
                throw new KanException("Board name can't be null!");
            }
            string nameWithoutSpaces = name.Trim();// remove spaces---- name:"  " is invalid
            if (nameWithoutSpaces.Length == 0)
            {
                log.Error("Illegal board name!");
                throw new KanException("Board name cannot be empty or contains just spaces");
            }

            //check if name exists in usersBoards of user:
            email = email.ToLower();
            if (!usersBoards.ContainsKey(email))
            {
                usersBoards.Add(email, new Dictionary<string, Board>());
            }

            //check if board already exist
            if (usersBoards[email].ContainsKey(name))
            {
                log.Error("Board name already exists");
                throw new KanException("Board name already exists!");
            }

            //if everything is ok
            Board newBoard = new Board(name, boardIdCounter, email);
            try
            {
                usersBoards[email].Add(name, newBoard);
                boards.Add(boardIdCounter, newBoard);
                boardController.Insert(newBoard.GetDTO());
                User2BoardDTO u2b = new User2BoardDTO(email, boardIdCounter, name);
                user2BoardsController.Insert(u2b);
                boardIdCounter++;
                log.Info($"Added board: {name} to the user: {email} successfully!");
            }
            catch (KanException ke) { throw ke; }
            catch (Exception e) { throw e; }
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        internal void DeleteBoard(string email, string name)
        {
            email = email.ToLower();
            CheckForExceptions(email, name);
            if (email.ToLower().Equals(usersBoards[email][name].GetOwner()))
            {
                try
                {
                    //need to remove all the tasks in this board.....................
                    Board toDelete = usersBoards[email][name];
                    boards.Remove(usersBoards[email][name].GetId());
                    usersBoards[email].Remove(name);
                    boardController.Delete(toDelete.GetDTO());
                    log.Info($"Removed board: {name} of user: {email} successfully!");
                }
                catch (KanException ke) { throw ke; }
                catch (Exception e) { throw e; }
            }
            else
            {
                throw new KanException("Only board owner can delete this board!");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal ArrayList ShowInProgress(string email)
        {
            //checkNotLoggedIn(loggedIn);
            email = email.ToLower();
            CheckUserHasABoard(email);
            ArrayList tasks = new ArrayList();
            //for(int i = 0; i < usersBoards[email].Count; i++)
            //{

            //    List<Task> currBoardShowInProg = b.Value.showInProgress();
            //    tasks.Concat(currBoardShowInProg);
            //}
            foreach (KeyValuePair<string, Board> b in usersBoards[email])
            {
                ArrayList currBoardShowInProg = b.Value.ShowInProgress(email);
                tasks.AddRange(currBoardShowInProg);
                //tasks.AddRange(currBoardShowInProg);
            }
            return tasks;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <returns></returns>
        internal Board GetBoard(string email, string boardName)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            log.Info("got board successfully!");
            return usersBoards[email][boardName];
        }

        internal Board GetBoard(int boardId)
        {
            if (!boards.ContainsKey(boardId))
            {
                throw new KanException("Board id is not exist!");
            }
            log.Info("got board successfully!");
            return boards[boardId];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        /// <param name="description"></param>
        //with Description
        internal void AddTask(string email, string boardName, string title, DateTime duedate, string description)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].AddTask(email, boardName, title, duedate, description, taskIdCounter);
            log.Info($"Added task {taskIdCounter} to user{email} in board {boardName} successfully!");
            taskIdCounter++;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        //without Description
        internal void AddTask(string email, string boardName, string title, DateTime duedate)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].AddTask(email, title, duedate, taskIdCounter);
            log.Info($"Added task {taskIdCounter} to user {email} in board {boardName} successfully!");
            taskIdCounter++;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="taskId"></param>
        /// <param name="column"></param>
        internal void AdvanceTask(string email, string boardName, int column, int taskId)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].AdvanceTask(email, taskId, column);
            log.Info($"Moved task {taskId} of user {email} in board {boardName} successfully!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnId"></param>
        /// <param name="limit"></param>
        internal void SetLimitOnColumn(string email, string boardName, int columnId, int limit)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].SetLimitOnColumn(columnId, limit);
            log.Info($"user {email} setted a limit in board {boardName} on column {columnId} successfully!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnId"></param>
        /// <returns></returns>
        internal int GetLimitOnColumn(string email, string boardName, int columnId)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            log.Info($"user {email} got a limit in board {boardName} on column {columnId} successfully!");
            return usersBoards[email][boardName].GetLimitOnColumn(columnId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="dueDate"></param>
        internal void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].SetDueDate(email, taskId, columnOrdinal, dueDate);
            log.Info($"user {email} updated due date in board {boardName} for task {taskId} successfully!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="description"></param>
        internal void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].SetDescription(email, taskId, columnOrdinal, description);
            log.Info($"user {email} updated description in board {boardName} for task {taskId} successfully!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        internal void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            usersBoards[email][boardName].SetTitle(email, taskId, columnOrdinal, title);
            log.Info($"user {email} updated tasktitle in board {boardName} for task {taskId} successfully!");
        }

        //internal void checkNotLoggedIn(bool loggedIn)
        //{
        //    if (!loggedIn) throw new KanException("User is not logged in!");
        //}

        private void CheckUserHasABoard(string email)
        {
            email = email.ToLower();
            if (!usersBoards.ContainsKey(email))
            {
                log.Error("user hasn't created board yet!");
                throw new KanException($"user with email:{email} hasn't created board yet!");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <exception cref="KanException"></exception>
        private void CheckBoardNameExists(string email, string boardName)
        {
            email = email.ToLower();
            if (!usersBoards[email].ContainsKey(boardName))
            {
                log.Error($"board {boardName} does not exist in the user {email}!");
                throw new KanException($"board '{boardName}' does not exist in the user {email}!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loggedin"></param>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        private void CheckForExceptions(string email, string boardName)
        {
            log.Debug("<checks for exceptions>");
            //checkNotLoggedIn(loggedin);
            email = email.ToLower();
            //if logged in than
            CheckUserHasABoard(email);
            //if user exists than
            CheckBoardNameExists(email, boardName);

        }

        /// <summary>
        ///  this function is only for grading service, it is not included in our design nor in the requirements of the assignment!
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        internal string GetColumnName(string email, string boardName, int column)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            log.Info("got columnName successfully");
            return usersBoards[email][boardName].GetColumnName(column);
        }

        /// <summary>
        /// this function is only for grading service, it is not included in our design nor in the requirements of the assignment!
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        internal List<Task> GetColumn(string email, string boardName, int columnOrdinal)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            log.Info("got column tasks list successfully");
            return usersBoards[email][boardName].GetColumn(columnOrdinal);
        }

        /// <summary>
        /// this method gets all the board that the user has.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal List<int> GetUserBoards(string email)
        {
            List<int> res = new List<int>();
            if (!usersBoards.ContainsKey(email.ToLower()))
            {
                //throw new KanException($"user '{email}' does not have boards yet!");
                return res;
            }
            foreach (KeyValuePair<string, Board> b in usersBoards[email.ToLower()])
            {
                res.Add(b.Value.GetId());
            }
            return res;
        }

        /// <summary>
        /// this method join user to a specific board.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <exception cref="KanException"></exception>
        internal void JoinBoard(string email, int boardId)
        {
            //check valid email
            //check boardId valid
            if (!boards.ContainsKey(boardId))
            {
                throw new KanException("board id is invalid!");
            }
            try
            {
                boards[boardId].AddMember(email.ToLower());
                if (!usersBoards.ContainsKey(email.ToLower()))
                {
                    usersBoards.Add(email.ToLower(), new Dictionary<string, Board>());
                }
                string boardName = boards[boardId].GetName();
                usersBoards[email.ToLower()].Add(boardName, boards[boardId]);
                //NEED TO ADD TO DB IN TABLE USERS2BOARDS THE EMAIL AND THE BOARD ID
                User2BoardDTO u2b = new User2BoardDTO(email, boardId, boardName);
                user2BoardsController.Insert(u2b);
            }
            catch (KanException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        /// <summary>
        /// this method leave board in user's boards.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardId"></param>
        /// <exception cref="KanException"></exception>
        internal void LeaveBoard(string email, int boardId)
        {
            //check valid email
            //check boardId valid
            //check notOwner
            if (!boards.ContainsKey(boardId))
            {
                throw new KanException("board id is invalid!");
            }
            try
            {
                string boardName = boards[boardId].GetName();
                boards[boardId].RemoveMember(email);
                usersBoards[email].Remove(boardName);
                //DELETE MEMBER EMAIL & BOARD ID FROM TABLE USERS2BOARDS
                User2BoardDTO u2b = new User2BoardDTO(email, boardId, boardName);
                user2BoardsController.Delete(u2b);
            }
            catch (KanException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// this method get the name of board.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal string GetBoardName(int boardId)
        {
            if (!boards.ContainsKey(boardId))
            {
                throw new KanException("board id is invalid!");
            }
            return boards[boardId].GetName();
        }

        /// <summary>
        /// this method change ownership in specific board.
        /// </summary>
        /// <param name="currentOwnerEmail"></param>
        /// <param name="newOwnerEmail"></param>
        /// <param name="boardName"></param>
        internal void TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            CheckForExceptions(currentOwnerEmail, boardName);
            CheckForExceptions(newOwnerEmail, boardName);
            usersBoards[currentOwnerEmail][boardName].TransferOwnership(newOwnerEmail);
        }

        /// <summary>
        /// this method assign task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskID"></param>
        /// <param name="emailAssignee"></param>
        internal void AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            CheckForExceptions(email, boardName);
            usersBoards[email.ToLower()][boardName].AssignTask(email, columnOrdinal, taskID, emailAssignee);
        }

        /// <summary>
        /// this method delete the data.
        /// </summary>
        internal void DeleteData()
        {
            boardController.DeleteTableData();
            taskController.DeleteTableData();
            user2BoardsController.DeleteTableData();
            usersBoards = new Dictionary<string, Dictionary<string, Board>>();
            boards = new Dictionary<int, Board>();
            boardIdCounter = 0;
            taskIdCounter = 0;
        }

        internal List<Board> GetBoardsDetails(string email)
        {
            List<Board> res = new List<Board>();
            if (!usersBoards.ContainsKey(email.ToLower()))
            {
                return res;
            }
            res = usersBoards[email.ToLower()].Values.ToList();
            return res;
        }

        internal List<Board> GetBoardsComplementDetails(string email)
        {
            List<Board> res = new List<Board>();
            if (!usersBoards.ContainsKey(email.ToLower()))
            {
                return boards.Values.ToList();
            }
            res = boards.Values.ToList();
            List<Board> remove = usersBoards[email.ToLower()].Values.ToList();
            res.RemoveAll(board1 => remove.Any(board2 => board1.GetId() == board2.GetId()));
            return res;
        }

        internal List<Board> GetAllBoardsWithDetails()
        {
            return boards.Values.ToList();
        }


        internal List<Task> GetTasksInColumn(string email, string boardName, int columnOrdinal)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);

            return usersBoards[email][boardName].GetTasksInColumn(email, columnOrdinal);
        }

        internal List<Task> GetTasksInBacklog(string email, string boardName)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            return usersBoards[email][boardName].GetTasksInBacklog(email, boardName);
        }

        internal List<Task> GetTasksInProgress(string email, string boardName)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            return usersBoards[email][boardName].GetTasksInProgress(email, boardName);
        }

        internal List<Task> GetTasksInDone(string email, string boardName)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            return usersBoards[email][boardName].GetTasksInDone(email, boardName);
        }

        internal List<Task> GetAllTasksInProgress(string email)
        {
            email = email.ToLower();
            CheckUserHasABoard(email);
            List<Task> tasks = new List<Task>();
            foreach (KeyValuePair<string, Board> b in usersBoards[email])
            {
                List<Task> currBoardShowInProg = b.Value.GetTasksInProgress(email, b.Value.GetOwner());
                tasks.AddRange(currBoardShowInProg);
            }
            return tasks;
        }

        internal List<string> GetAllMembersInBoards(string boardName)
        {
            List<string> allMembers = new List<string>();
            foreach (var userBoard in usersBoards.Values)
            {
                if (userBoard.ContainsKey(boardName))
                {
                    Board board = userBoard[boardName];
                    allMembers.AddRange(board.GetAllMembersInBoard());
                    // No need to continue iterating once the specific board is found
                    break;
                }
            }
            return allMembers.ToList();
        }

        internal void DoesBoardExist(string boardName)
        {
            bool exist = false;
            foreach (var userBoard in usersBoards.Values)
            {
                if (userBoard.ContainsKey(boardName))
                {
                    // Board with the given name exists
                    exist= true;
                }
            }
            if (exist != true) // Board with the given name doesn't exist
            {
                throw new KanException("board name is doesnt exist");
            }
            
        }

        internal List<Task> ShowInProgressSender(string email)
        {
            email = email.ToLower();
            CheckUserHasABoard(email);
            List<Task> tasks = new List<Task>();
            foreach (KeyValuePair<string, Board> b in usersBoards[email])
            {
                List<Task> currBoardShowInProg = b.Value.ShowInProgressSender(email);
                tasks.AddRange(currBoardShowInProg);
                //tasks.AddRange(currBoardShowInProg);
            }
            return tasks;
        }

        //(int, string, DateTime, DateTime, string, string)
        internal ArrayList GetTaskInColumn(string email, string boardName, int taskId,int columnOrdinal)
        {
            email = email.ToLower();
            CheckForExceptions(email, boardName);
            Task t = usersBoards[email][boardName].GetTaskInColumn(taskId,columnOrdinal);
            ArrayList list=new ArrayList();
            list.Add(t.Id);
            list.Add(t.Title);
            list.Add(t.CreationTime);
            list.Add(t.DueDate);
            list.Add(t.Description);
            list.Add(t.GetAssignee());
            return list;
            //return (t.Id,t.Title, t.CreationTime, t.DueDate, t.Description, t.GetAssignee());
        }
    }

}
