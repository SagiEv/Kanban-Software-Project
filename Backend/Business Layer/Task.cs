using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer;
using Microsoft.VisualBasic;
using static System.Data.Entity.Infrastructure.Design.Executor;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Kanban.Backend
{
    [Serializable]
    public class Task
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        private int boardId;
        private string assignee;

        private readonly int TITLE_LENGTH = 50;
        private readonly int DESCRIPTION_LENGTH = 300;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private TaskController taskController;
        private TaskDTO tDTO;
        private readonly int backlog = 0;

        public Task(string title, DateTime duedate, int Id, int boardId)
        {
            CheckTitle(title);
            CheckDueDate(duedate);
            //taskController = new TaskController();
            this.Title = title;
            this.DueDate = duedate;
            this.CreationTime = DateTime.Now;
            this.Description = null;
            this.Id = Id;
            assignee = null;
            tDTO = new TaskDTO(Id, boardId, CreationTime, Title, Description, DueDate, assignee, backlog);

            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="boardName"></param>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        /// <param name="desc"></param>
        public Task(string title, DateTime duedate, string desc,int Id,int boardId)
        {
            CheckForExecptions(title,duedate,desc);
            //taskController = new TaskController();
            this.assignee = null;
            this.Title = title;
            this.DueDate = duedate;
            this.CreationTime = DateTime.Now;
            this.Description = desc;
            this.Id = Id;
            assignee = null;
            this.boardId = boardId;
            tDTO = new TaskDTO(Id, boardId, CreationTime, Title, Description, DueDate, assignee, backlog);
        }

        internal Task(TaskDTO t)
        {
            CheckForExecptions(t.Title, t.DueDate, t.Description);
            this.tDTO = t;
            this.assignee = tDTO.Assignee;
            this.Title = tDTO.Title;
            this.DueDate = tDTO.DueDate;
            this.CreationTime = DateTime.Now;
            this.Description = tDTO.Description;
            this.Id = tDTO.ID;
            this.assignee = tDTO.Assignee;
            this.boardId = tDTO.BoardId;
        }

        //public void InsertTask(int boardID)
        //{
        //    if (!taskController.Insert(new(Id, boardID, CreationTime, Title, Description, DueDate, assignee, 0)))
        //        throw new Exception("Failed to insert task to DB");
        //    else
        //    {
        //        log.Info("Success in inserting task into DB");
        //    }
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        internal void SetDescription(string email ,string desc)
        {
            checkAssignee(email);
            CheckDescription(desc);
            tDTO.Description = desc;
            this.Description = desc;
            //if (taskController.Update(Id, "Description", desc))
            //{
            //    this.Description = desc;
            //    log.Info("Success to update description in DB");
            //}
            //else
            //{
            //    log.Error("Failure to edit description in DB");
            //    throw new Exception("Failure to edit description in DB");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="duedate"></param>
        internal void SetDueDate(string email, DateTime duedate)
        {
            checkAssignee(email);
            CheckDueDate(duedate);
            this.DueDate = duedate;
            tDTO.DueDate = duedate;
            //if (taskController.Update(Id, "DueDate", duedate))
            //{
            //    this.DueDate = duedate;
            //    log.Info("Success to update duedate in DB");
            //}
            //else
            //{
            //    log.Error("Failure to edit duedate in DB");
            //    throw new Exception("Failure to edit duedate in DB");
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        internal void SetTitle(string email, string title)
        {
            checkAssignee(email);
            CheckTitle(title);
            this.Title = title;
            tDTO.Title = title;
            //if (taskController.Update(Id, "Title", title))
            //{
            //    this.Title = title;
            //    log.Info("Success to update title in DB");
            //}
            //else
            //{
            //    log.Error("Failure to edit title in DB");
            //    throw new Exception("Failure to edit title in DB");
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <exception cref="KanException"></exception>
        internal void CheckTitle(string title)
        {
            if (title == null || title.Equals("") || title.Length > TITLE_LENGTH)
            {
                log.Error("invalid title");
                throw new KanException("Invalid new title");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duedate"></param>
        /// <exception cref="KanException"></exception>
        internal void CheckDueDate(DateTime duedate)
        {
            if (duedate==null ||duedate <= CreationTime)
            {
                log.Error("date time has already passed");
                throw new KanException("Invalid date time");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="desc"></param>
        /// <exception cref="KanException"></exception>
        internal void CheckDescription(string desc)
        {
            if (desc == null)
            {
                log.Error("Description cannot have a null value");
                throw new KanException("Description cannot have a null value");
            }
            else if (desc.Length > DESCRIPTION_LENGTH)
            {
                log.Error("Description over 300 characters");
                throw new KanException("Description over 300 characters");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="duedate"></param>
        /// <param name="desc"></param>
        internal void CheckForExecptions(string title, DateTime duedate, string desc)
        {
            CheckTitle(title);
            CheckDueDate(duedate);
            CheckDescription(desc);
        }

        /// <summary>
        /// this function is not part of our design its only for our own tests
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Task no.{Id}";
        }

        /// <summary>
        /// this method get the assignee
        /// </summary>
        /// <returns></returns>
        public string GetAssignee()
        {
            return assignee;
        }

        /// <summary>
        /// this method set the assignee
        /// </summary>
        /// <param name="email"></param>
        internal void SetAssignee(string email)
        {
            //need to add check for email assignee
            assignee = email;
            tDTO.Assignee = assignee; 
        }

        /// <summary>
        /// this method check if the email is assignee of the task. else, throw exception.
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="KanException"></exception>
        private void checkAssignee(string email)
        {
            if(!email.ToLower().Equals(assignee))
            {
                throw new KanException($"{email} is not the assignee! therefore can't change task!");
            }
        }

        /// <summary>
        /// this method set assignee.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailAssignee"></param>
        /// <exception cref="KanException"></exception>
        internal void Assign(string email,string emailAssignee)
        {
            if(assignee != null && !assignee.Equals(email))
            {
                throw new KanException($"this task is already assignned to {assignee}! he is the only one who can change assignment!");
            }
            assignee = emailAssignee.ToLower();
            tDTO.Assignee = assignee;
        }

        /// <summary>
        /// this method check if the email is assignee of the task
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal bool isAssignee(string email)
        {
            return email.ToLower().Equals(assignee);
        }
        /// <summary>
        /// this method get TaskDTO
        /// </summary>
        /// <returns></returns>
        internal TaskDTO GetDTO()
        {
            return tDTO;
        }

        public int GetBoardId()
        {
            return boardId;
        }

    }
}

