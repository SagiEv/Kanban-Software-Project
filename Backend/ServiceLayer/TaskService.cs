using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Kanban.Backend.Business_Layer;
using Microsoft.VisualBasic;
using System.IO;
using log4net.Config;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class TaskService
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BoardFacade bf;
        private UserFacade uf;

        public TaskService() {
            //this.board = board;
            this.bf=new BoardFacade();

        }

        public TaskService(UserFacade uf, BoardFacade bf)
        {
            //this.board = board;
            this.bf = bf;
            this.uf = uf;

        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.AddTask(email, boardName, title,dueDate, description);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }

        }


        //@override
        /// <summary>
        /// This method adds a new task - WITHOUT DESCRIPTION.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AddTask(string email, string boardName, string title, DateTime dueDate)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.AddTask(email, boardName, title, dueDate);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }
        //@override
        /// <summary>
        /// This method adds a new task - WITHOUT DESCRIPTION.
        /// </summary>
        /// <param name="email">Email of the user. The user must be logged in.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="column">The number of the column the task is in</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string AdvanceTask(string email, string boardName, int taskId, int column)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.AdvanceTask(email, boardName, taskId, column);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }

        }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs <returns
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }

        /// <summary>
        /// This method updates the Description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New Description for the task</param>
        /// <returns>An empty response, unless an error occurs <returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId,string description)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }

        /// <summary>
        /// This method updates task Title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New Title for the task</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }

        internal string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            try
            {
                uf.CheckLoggedIn(email);
                bf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                return JsonSerializer.Serialize(new Response());
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }

/*        internal string GetTask(int id, string email)
        {
            try
            {
                uf.CheckLoggedIn(email);
                Task t = bf.GetTask(id, email);
                //bf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                return JsonSerializer.Serialize(new Response(TaskToSend(t));
            }
            catch (KanException e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return JsonSerializer.Serialize(new Response(e)); ;
            }
        }*/

        //internal void LoadData()
       // {
        //    throw new NotImplementedException();
       // }
    }
}
