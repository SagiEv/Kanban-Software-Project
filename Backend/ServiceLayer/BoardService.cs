using Kanban.Backend.Business_Layer;
using Kanban.Backend.ServiceLayer;
using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using Kanban.Backend;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class BoardService
    {
        private UserFacade uf;
        private BoardFacade bf;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BoardService()
        {
            this.bf= new BoardFacade();
            this.uf= new UserFacade();
        }

        public BoardService(UserFacade uf, BoardFacade bf)
        {
            this.bf = bf;
            this.uf = uf;
        }


        /// <summary>
        /// This method creates a board for the given user.
        /// </summary>
        /// <param name="email">Email of the user, must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>An empty response, unless an error occurs<returns>
        public string CreateBoard(string email,string name)
        {       
            try {
                uf.CheckLoggedIn(email);
                bf.CreateBoard(email, name);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e) {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e) {
                return JsonSerializer.Serialize(new Response(e));
            }
            //BF.addBoard(email,name);
            //string res = JsonSerializer.Serialize(new Response("Not Implemented Yet"));
            //Response res =  new Response("Not Implemented Yet");
            //return res.ResToString();
            //throw new NotImplementedException();
        }

        /// <summary>
        /// This method deletes a board.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in and an owner of the board.</param>
        /// <param name="name">The name of the board</param>
        /// <returns>An empty response, unless an error occurs<returns>
        public string DeleteBoard(string email, string name) 
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.DeleteBoard(email, name);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            //string res = JsonSerializer.Serialize(new Response("Not Implemented Yet"));
            //Response res = new Response("Not Implemented Yet");
            //return res.ResToString();
            //throw new NotImplementedException();
        }


        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs</returns>
        public string ShowInProgress(string email) 
        {
            try
            {
                uf.CheckLoggedIn(email);
                //bf.showInProgress(email);
                return JsonSerializer.Serialize(new Response(bf.ShowInProgress(email)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's limit, unless an error occurs</returns>
        public string GetLimitOnColumn(string email,string boardName,int column) 
        {
            try
            {
                uf.CheckLoggedIn(email);
                return JsonSerializer.Serialize(new Response(bf.GetLimitOnColumn(email, boardName, column)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string SetLimitOnColumn(string email, string boardName,int column, int limit)  
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.SetLimitOnColumn(email, boardName,column,limit);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
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
            try
            {
                uf.CheckLoggedIn(email);
                bf.AdvanceTask(email, boardName, columnOrdinal, taskId);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }
        //this function is only implemented beacuse of GRADING SERVICE! this is NOT functional requirement

        /// <summary>
        /// This method gets the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response with the column's name, unless an error occurs</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //bf.getColumnName(email, boardName, columnOrdinal);
                return JsonSerializer.Serialize(new Response(bf.GetColumnName(email, boardName, columnOrdinal)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <returns></returns>
        internal string GetColumn(string email, string boardName, int columnOrdinal)
        {
            {
                try
                {
                    uf.CheckLoggedIn(email);
                    //bf.getColumn(email, boardName, columnOrdinal);
                    object col = bf.GetColumn(email, boardName, columnOrdinal);
                    return JsonSerializer.Serialize(new Response(col));
                }
                catch (KanException e)
                {
                    return JsonSerializer.Serialize(new Response(e));
                }
                catch (Exception e)
                {
                    return JsonSerializer.Serialize(new Response(e));
                }
            }
        }

        internal string GetUserBoards(string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                return JsonSerializer.Serialize(new Response(bf.GetUserBoards(email)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string JoinBoard(string email, int boardID)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.JoinBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string LeaveBoard(string email, int boardID)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.LeaveBoard(email, boardID);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetBoardName(int boardId)
        {
            try
            {
                //uf.CheckLoggedIn(email);
                return JsonSerializer.Serialize(new Response(bf.GetBoardName(boardId)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            try
            {
                uf.CheckLoggedIn(currentOwnerEmail);
                bf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string LoadData()
        {
            try
            {
                bf.LoadData();
            }
            catch 
            {
                //if we got an exception it means that there were no data to load from tables and this is perfectly FINE!
                log.Debug("no data in db file");
            }
            return JsonSerializer.Serialize(new Response());
           
        }
        internal string DeleteData()
        {
            try
            {
                bf.DeleteData();
            }
            catch
            {
                log.Debug("there is no available data in DB");
            }
            return JsonSerializer.Serialize(new Response());
        }

        internal string GetBoard(string email, string boardName)
        {
            try
            {
                uf.CheckLoggedIn(email);
                return JsonSerializer.Serialize(new Response(bf.GetBoard(email,boardName)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetBoard(string email, int boardId)
        {
            try
            {
                uf.CheckLoggedIn(email);
                Board board = bf.GetBoard(boardId);
                BoardToSend board2send = new BoardToSend(board);
                return JsonSerializer.Serialize(new Response(board2send));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetBoardsWithDetails(string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Board> boards = bf.GetBoardsDetails(email);
                List<BoardToSend> boardsToSend = new List<BoardToSend>();
                foreach(Board b in boards)
                {
                    boardsToSend.Add(new BoardToSend(b));
                }
                return JsonSerializer.Serialize(new Response(boardsToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetBoardsComplementDetails(string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Board> boards = bf.GetBoardsComplementDetails(email);
                List<BoardToSend> boardsToSend = new List<BoardToSend>();
                foreach (Board b in boards)
                {
                    boardsToSend.Add(new BoardToSend(b));
                }
                return JsonSerializer.Serialize(new Response(boardsToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetAllBoardsWithDetails(string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Board> boards = bf.GetAllBoardsWithDetails();
                List<BoardToSend> boardsToSend = new List<BoardToSend>();
                foreach (Board b in boards)
                {
                    boardsToSend.Add(new BoardToSend(b));
                }
                return JsonSerializer.Serialize(new Response(boardsToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetTasksInBacklog(string email, string boardName)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Task> tasks = bf.GetTasksInBacklog(email, boardName);
                List<TaskToSend> tasksToSend = new List<TaskToSend>();
                foreach (Task t in tasks)
                {
                    tasksToSend.Add(new TaskToSend(t));
                }
                return JsonSerializer.Serialize(new Response(tasksToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetTasksInProgress(string email, string boardName)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Task> tasks = bf.GetTasksInProgress(email, boardName);
                List<TaskToSend> tasksToSend = new List<TaskToSend>();
                foreach (Task t in tasks)
                {
                    tasksToSend.Add(new TaskToSend(t));
                }
                return JsonSerializer.Serialize(new Response(tasksToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string GetTasksInDone(string email, string boardName)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<Task> tasks = bf.GetTasksInDone(email, boardName);
                List<TaskToSend> tasksToSend = new List<TaskToSend>();
                foreach (Task t in tasks)
                {
                    tasksToSend.Add(new TaskToSend(t));
                }
                return JsonSerializer.Serialize(new Response(tasksToSend));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }
        internal string GetAllMembersInBoards(string boardName)
        {
            try
            {
                bf.DoesBoardExist(boardName);
                //Notice we do not getting the objects from the business out, only partial info important for the UI using BoardToSend
                List<string> members = bf.GetAllMembersInBoards(boardName);
                List<string> memberss = new List<string>();
                foreach (String s in members)
                {
                    memberss.Add(new string(s));
                }
                return JsonSerializer.Serialize(new Response(memberss));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        internal string ShowInProgressSender(string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                //bf.showInProgress(email);
                return JsonSerializer.Serialize(new Response(bf.ShowInProgressSender(email)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }

        public string GetTaskInColumn(string email, string boardName, int taskId, int columnOrdinal)
        {
            try
            {
                uf.CheckLoggedIn(email);
                string taskserialized = JsonSerializer.Serialize(bf.GetTaskInColumn(email, boardName, taskId, columnOrdinal));
                Response res = new Response(taskserialized);
                string json = JsonSerializer.Serialize(res);
                
                Response ret = JsonSerializer.Deserialize<Response>(json);
                object var = ret.ReturnValue;
                JsonElement val =(JsonElement)ret.ReturnValue;
                //string json = JsonSerializer.Serialize(res);
                //bf.showInProgress(email);
                return json;
                return JsonSerializer.Serialize(new Response(bf.GetTaskInColumn(email, boardName, taskId, columnOrdinal)));
            }
            catch (KanException e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                return JsonSerializer.Serialize(new Response(e));
            }
        }
    }


}
