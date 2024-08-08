using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Collections;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Data.Common;

namespace Kanban.Backend.Business_Layer
{
    [Serializable]
    internal class Board
    {
        //fields
        private int boardId;
        private string boardName;
        private string owner;
        private int[] limitCol;
        private User2BoardsController u2bController;
        private BoardController boardController;
        private TaskController taskController;
        private const int columnsNum = 3; //this number represents the number of columns in the board:backlog,inProgress,done as appeared in the requirements.
        private Dictionary<int, Task>[] columns;//this array contains columns, each of the columns represented by dictionary with taskId as key.
        private List<string> members;
        private BoardDTO bDTO;


        //this fields are used only to avoid magic numbers
        private const int NoLimit = -1;
        private const int backlog = 0;
        private const int inProgress = 1;
        private const int done = 2;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        internal Board(string name, int boardId, string email)
        {
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            log.Debug("Initiating board");

            this.boardController = new BoardController();
            this.taskController = new TaskController();
            this.u2bController = new User2BoardsController();

            this.limitCol = new int[columnsNum];
            this.columns = new Dictionary<int, Task>[columnsNum];
            for (int i = 0; i < columnsNum; i++)
            {
                limitCol[i] = NoLimit; //this value represents that there is no limitation
                columns[i] = new Dictionary<int, Task>();
            }

            this.boardName = name;
            this.boardId = boardId;
            this.owner = email.ToLower();
            this.members = new List<string>();
            members.Add(owner);
            this.bDTO = new BoardDTO(boardId, boardName, owner, limitCol[backlog], limitCol[inProgress], limitCol[done]);
            
            LoadData();
        }

        public Board(BoardDTO bDTO)
        {
            this.boardController = new BoardController();
            this.taskController = new TaskController();
            this.u2bController = new User2BoardsController();
            this.boardName = bDTO.boardName;
            this.bDTO = bDTO;
            this.boardId = bDTO.BoardId;
            this.owner = bDTO.owner;
            this.members = new List<string>();
            this.columns = new Dictionary<int, Task>[columnsNum];
            for (int i = 0; i < columnsNum; i++)
            {
                columns[i] = new Dictionary<int, Task>();
            }
            this.limitCol = new int[columnsNum];
            this.limitCol[backlog] = bDTO.limit_backlog;
            this.limitCol[inProgress] = bDTO.limit_inprogress;
            this.limitCol[done] = bDTO.limit_done;
            //query in db in for all members of boardId
            LoadData();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <@pre: 0<=column<=2
        /// <returns></returns>
        private bool CanAddTo(int column)
        {
            if (limitCol[column] == NoLimit) return true;
            return columns[column].Count < limitCol[column];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        /// <param name="description"></param>
        /// <param name="taskId"></param>
        /// <exception cref="Exception"></exception>
        internal void AddTask(string email, string boardName, string title, DateTime duedate, string description, int taskId)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new KanException("Description can't be empty!");

            //check if owner or member
            CheckMember(email);
            if (!CanAddTo(backlog)) throw new KanException("Over the limit!");
            Task add = new Task(title, duedate, description, taskId,boardId);
            columns[backlog].Add(taskId, add);
            taskController.Insert(add.GetDTO());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        /// <param name="taskId"></param>
        /// <exception cref="Exception"></exception>
        //without Description
        internal void AddTask(string email, string title, DateTime duedate, int taskId)
        {
            CheckMember(email);
            CheckTaskNotExist(taskId);
            if (!CanAddTo(backlog)) throw new KanException("Over the limit!");
            Task add = new Task(title, duedate, taskId, boardId);
            columns[backlog].Add(taskId, add);
            taskController.Insert(add.GetDTO());
        }

        private void CheckTaskNotExist(int taskId)
        {
            if (columns[backlog].ContainsKey(taskId) || columns[inProgress].ContainsKey(taskId) || columns[done].ContainsKey(taskId))
                throw new KanException($"Task no. {taskId} already exist in this board!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="column"></param>
        /// <exception cref="KanException"></exception>
        internal void AdvanceTask(string email, int taskId, int column)
        {
            CheckValidColumn(column);
            CheckIdExist(taskId, column);
            CheckAssignee(email, taskId, column);

            if (column == done)
            {
                throw new KanException($"You are trying to move a task that is already done! Task id is {taskId}");
            }

            else if (!CanAddTo(column + 1))
            {
                throw new KanException($"Column no. {column + 1} is full!");
            }

            Task move = columns[column][taskId];
            columns[column].Remove(taskId);
            columns[column + 1][taskId] = move;
            move.GetDTO().Column = column + 1;
            log.Debug($"Moved taskId: {taskId} from: {column} to {column + 1} successfully!");

            //TaskDTO.set
            //else if (taskController.Update(taskId, TaskDTO.NumberOfCloumnColumnName, column)) //everything is ok and we can advance task safely
            //{
            //    Task move = columns[column][taskId];
            //    columns[column].Remove(taskId);
            //    columns[column + 1][taskId] = move;
            //    log.Debug($"Moved taskId: {taskId} from: {column} to {column + 1} successfully!");
            //}
            //else
            //{
            //    throw new KanException("Failed to advance task in DB");
            //}
        }

        private void CheckAssignee(string email, int taskID, int col)
        {
            if (!email.ToLower().Equals(columns[col][taskID].GetAssignee()))
                throw new KanException($"{email} is not the assignee of this task!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="columnId"></param>
        /// <exception cref="KanException"></exception>
        private void CheckIdExist(int taskId, int columnId)
        {
            log.Info($"checks if taskId:{taskId} exists in columnId:{columnId}");
            if (!(columns[columnId].ContainsKey(taskId)))
            {
                throw new KanException("the task does not exist");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="columnId"></param>
        /// <param name="desc"></param>
        /// <exception cref="KanException"></exception>
        internal void SetDescription(string email, int taskId, int columnId, string desc)
        {
            CheckValidColumn(columnId);
            if (columnId == done)
            {
                throw new KanException("A task that is done cannot be changed!");
            }
            CheckIdExist(taskId, columnId);
            log.Info("calls setDescription function inside Task class");
            columns[columnId][taskId].SetDescription(email, desc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="columnId"></param>
        /// <param name="title"></param>
        /// <exception cref="KanException"></exception>
        internal void SetTitle(string email, int taskId, int columnId, string title)
        {
            CheckValidColumn(columnId);

            if (columnId == done)
            {
                throw new KanException("A task that is done cannot be changed!");
            }

            CheckIdExist(taskId, columnId);

            columns[columnId][taskId].SetTitle(email, title);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="columnId"></param>
        /// <param name="duedate"></param>
        /// <exception cref="KanException"></exception>
        internal void SetDueDate(string email, int taskId, int columnId, DateTime duedate)
        {
            CheckValidColumn(columnId);
            CheckIdExist(taskId, columnId);
            if (columnId == done)
            {
                throw new KanException("A task that is done cannot be changed");
            }

            columns[columnId][taskId].SetDueDate(email, duedate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnId"></param>
        /// <param name="limit"></param>
        /// <exception cref="KanException"></exception>
        internal void SetLimitOnColumn(int columnId, int limit)
        {
            CheckValidColumn(columnId);
            if (limit > 0 && limit >= columns[columnId].Count)
            {
                //if (boardController.Update(columnId, bDTO.colName(columnId) ,limit))

                limitCol[columnId] = limit;
                if (columnId == backlog)
                {
                    bDTO.limit_backlog = limit;
                }
                if (columnId == inProgress)
                {
                    bDTO.limit_inprogress = limit;
                }
                if (columnId == done)
                {
                    bDTO.limit_done = limit;
                }
            }
            else
                throw new KanException("invalid limit!");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        internal int GetLimitOnColumn(int columnId)
        {
            CheckValidColumn(columnId);
            return limitCol[columnId];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal ArrayList ShowInProgress(string email)
        {
            ArrayList list = new ArrayList();
            foreach (KeyValuePair<int, Task> task in columns[inProgress])
            {
                if (task.Value.isAssignee(email))
                {
                    list.Add(task.Value);
                }
            }
            return list;
        }

        internal List<Task> ShowInProgressSender(string email)
        {
            List<Task> list = new List<Task>();
            foreach (KeyValuePair<int, Task> task in columns[inProgress])
            {
                if (task.Value.isAssignee(email))
                {
                    list.Add(task.Value);
                }
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnId"></param>
        /// <exception cref="KanException"></exception>
        private void CheckValidColumn(int columnId)
        {
            if (!(backlog <= columnId && columnId <= done)) throw new KanException($"invalid column! {columnId} is not in range!");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal string GetColumnName(int column)
        {
            CheckValidColumn(column);
            switch (column)
            {
                case backlog: return nameof(backlog);

                case inProgress: return "in progress";

                case done: return nameof(done);

                default: throw new KanException($"illegal column! can't get name of column no.{column}");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        internal List<Task> GetColumn(int column)
        {
            CheckValidColumn(column);
            return (List<Task>)columns[column].Values.ToList();
        }


        private void CheckOwner(string email)
        {
            if (!(owner.Equals(email.ToLower())))
                throw new KanException($"{email} is not the owner of this board!");
        }

        private void CheckMember(string email)
        {
            if (!members.Contains(email.ToLower()))
                throw new KanException($"{email} is not a member in this board!");
        }

        /// <summary>
        /// this function only get called after a user leaves the board
        /// </summary>
        /// <param name="email"></param>
        private void UnassignTasks(string email)
        {
            UnassignTasksInCol(email, backlog);
            UnassignTasksInCol(email, inProgress);
            UnassignTasksInCol(email, done);
        }

        /// <summary>
        /// this function gets user and col num and deletes his tasks
        /// </summary>
        /// <param name="email"></param>
        /// <param name="col"></param>
        private void UnassignTasksInCol(string email, int col)
        {
            foreach (KeyValuePair<int, Task> task in columns[col])
            {
                if (task.Value.GetAssignee() == email.ToLower())
                {
                    //if (taskController.ChangeAssignee(email, null, TaskDTO.AssigneeColumnName, task.Value.Id, col))
                    task.Value.SetAssignee(null);
                }
            }

        }
        /// <summary>
        /// this method add a member
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="KanException"></exception>
        internal void AddMember(string email)
        {
            if (members.Contains(email.ToLower()))
            {
                throw new KanException($"{email} already a member!");
            }
            members.Add(email.ToLower());
            }

        /// <summary>
        /// this method get the board name
        /// </summary>
        /// <returns></returns>
        internal string GetName()
        {
            return boardName;
        }

        /// <summary>
        /// this method get the board id
        /// </summary>
        /// <returns></returns>
        internal int GetId()
        {
            return boardId;
        }

        /// <summary>
        /// this method remove a member
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="KanException"></exception>
        internal void RemoveMember(string email)
        {
            if (email.ToLower().Equals(owner))
            {
                throw new KanException("Owner cannot leave his board!");
            }
            CheckMember(email);
            UnassignTasks(email);
            members.Remove(email.ToLower());
            }

        /// <summary>
        /// this method get the board's owner
        /// </summary>
        /// <returns></returns>
        internal string GetOwner()
        {
            return owner;
        }

        /// <summary>
        /// this method change ownership to this board.
        /// </summary>
        /// <param name="newOwnerEmail"></param>
        internal void TransferOwnership(string newOwnerEmail)
        {
            CheckMember(newOwnerEmail);
            //if (boardController.Update(boardId, BoardDTO.Owner, newOwnerEmail))
            owner = newOwnerEmail.ToLower();
            bDTO.owner = owner;
        }
        /// <summary>
        /// this method assign task
        /// </summary>
        /// <param name="email"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskID"></param>
        /// <param name="emailAssignee"></param>
        internal void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            CheckMember(email);
            CheckMember(emailAssignee);
            CheckIdExist(taskID, columnOrdinal);
            columns[columnOrdinal][taskID].Assign(email, emailAssignee);
        }

        /// <summary>
        /// this method get BoardDTO
        /// </summary>
        /// <returns></returns>
        internal BoardDTO GetDTO()
        {
            return bDTO;
        }

        /// <summary>
        /// this method load the data from the database.
        /// </summary>
        internal void LoadData()
        {
            List<TaskDTO> tasks = taskController.GetAllTasksInBoard(boardId);
            foreach (TaskDTO t in tasks)
            {
                Task newTask = new Task(t);
                columns[t.Column].Add(t.ID, newTask);
            }

            try { members = u2bController.GetMembers(boardId); }
            //members = u2bController.GetMembers(boardId);
            catch
            {
                log.Debug("no new data from user2boards table");
            }
            try
            {
                int limit_backlog = boardController.SelectColLimit("backlog_lim", boardId);
                int limit_inprogress = boardController.SelectColLimit("in_progress_lim", boardId);
                int limit_done = boardController.SelectColLimit("done_lim", boardId);
                this.SetLimitOnColumn(backlog, limit_backlog);
                this.SetLimitOnColumn(inProgress, limit_inprogress);
                this.SetLimitOnColumn(done, limit_done);
            }
            catch
            {
                log.Debug("no new data from boards table");
            }
        }
        internal List<Task> GetTasksInColumn(string email,int column)
        {
            CheckValidColumn(column);
            List<Task> res = new List<Task>();
            res= columns[column].Values.ToList();
            return res;
        }
        internal List<Task> GetTasksInBacklog(string email, string boardName)
        {
            return GetTasksInColumn(email, backlog);
        }
        internal List<Task> GetTasksInProgress(string email, string boardName)
        {
            return GetTasksInColumn(email, inProgress);
        }

        internal List<Task> GetTasksInDone(string email, string boardName)
        {
            return GetTasksInColumn(email, done);
        }
        
        internal List<string> GetAllMembersInBoard()
        {
            return this.members;
        }

        internal Task GetTaskInColumn(int taskId, int columnOrdinal)
        {
            CheckValidColumn(columnOrdinal);
            if (columns[columnOrdinal].ContainsKey(taskId))
            {
                return columns[columnOrdinal][taskId];
            }
            throw new KanException("No Such Task!");
        }
    }
}
