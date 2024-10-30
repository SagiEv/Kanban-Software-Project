using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.Backend.Business_Layer;
using log4net;
using System;
using System.IO;
using log4net.Config;
using System.Data.SQLite;
using Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.Backend.ServiceLayer
{
    public class SystemService
    {
        private UserFacade uf;
        private BoardFacade bf;
        private UserService userService;
        private BoardService boardService;
        private TaskService taskService;
        //private static readonly ILog log = LogHelper.GetLogger(typeof(SystemService));



        public SystemService()
        {
            uf = new UserFacade();
            bf = new BoardFacade();
            userService = new UserService(uf);
            boardService = new BoardService(uf, bf);
            taskService = new TaskService(uf, bf);
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            return userService.Register(email, password);
        }


        /// <summary>
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            return userService.Login(email, password);
        }


        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            return userService.Logout(email);
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            //throw new NotImplementedException();
            return boardService.SetLimitOnColumn(email, boardName, columnOrdinal, limit);
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            return boardService.GetLimitOnColumn(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            return boardService.GetColumnName(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            return taskService.AddTask(email, boardName, title, description, dueDate);
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AddTask(string email, string boardName, string title, DateTime dueDate)
        {
            return taskService.AddTask(email, boardName, title, dueDate);
        }


        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            return taskService.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
        }


        /// <summary>
        /// This method updates task Title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New Title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            return taskService.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
        }


        /// <summary>
        /// This method updates the Description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New Description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            return taskService.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
        }


        /// <summary>
        /// This method advances a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            return boardService.AdvanceTask(email, boardName, columnOrdinal, taskId);
        }


        /// <summary>
        /// This method returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            return boardService.GetColumn(email, boardName, columnOrdinal);
        }


        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string CreateBoard(string email, string name)
        {
            return boardService.CreateBoard(email, name);
        }


        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string DeleteBoard(string email, string name)
        {
            return boardService.DeleteBoard(email, name);
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            return boardService.ShowInProgress(email);
        }

        public string InProgressTasksSender(string email)
        {
            return boardService.ShowInProgressSender(email);
        }

        public string GetUserBoards(string email)
        {
            return boardService.GetUserBoards(email);
        }

        public string GetBoard(string email, int boardId)
        {
            return boardService.GetBoard(email, boardId);
        }

        public string JoinBoard(string email, int boardID)
        {
            return boardService.JoinBoard(email, boardID);
        }

        public string LeaveBoard(string email, int boardID)
        {
            return boardService.LeaveBoard(email, boardID);
        }

        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            return taskService.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
        }

        public string GetBoardName(int boardId)
        {
            return boardService.GetBoardName(boardId);
        }

        public string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            return boardService.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
        }

        public string DeleteData()
        {
            boardService.DeleteData();
            userService.DeleteData();


            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //this._connectionString = $"Data Source={"C:\\Users\\karin\\Desktop\\Kanban project\\Backend\\kanban.db"}; Version=3;";
            string connectionString = $"Data Source={path}; Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Users", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Boards", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Tasks", connection))
                {
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM Users2Boards", connection))
                {
                    command.ExecuteNonQuery();
                }

                return JsonSerializer.Serialize(new Response());
            }
        }

        public string LoadData()
        {
            userService.LoadData();
            boardService.LoadData();
            //taskService.LoadData();
            return JsonSerializer.Serialize(new Response());
        }

        public string GetBoard(string email, string boardName)
        {
            return boardService.GetBoard(email, boardName);
        }

        public string GetAllBoardsWithDetails(string email)
        {
            return boardService.GetAllBoardsWithDetails(email);
        }

        public string GetBoardsWithDetailsOfUser(string email)
        {
            return boardService.GetBoardsWithDetails(email);
        }

        public string GetBoardsWithDetailsComplement(string email)
        {
            return boardService.GetBoardsComplementDetails(email);
        }

        public string GetTasksInBacklog(string email, string boardName)
        {
            return boardService.GetTasksInBacklog(email, boardName);
        }

        public string GetTasksInProgress(string email, string boardName)
        {
            return boardService.GetTasksInProgress(email, boardName);
        }

        public string GetTasksInDone(string email, string boardName)
        {
            return boardService.GetTasksInDone(email, boardName);
        }

        public string GetAllMembersInBoards(string boardName)
        {
            return boardService.GetAllMembersInBoards(boardName);
        }

        public string GetTaskInColumn(string email, string boardName, int taskId, int columnOrdinal) 
        {
            return boardService.GetTaskInColumn(email, boardName, taskId, columnOrdinal);
        }
    }
    }
