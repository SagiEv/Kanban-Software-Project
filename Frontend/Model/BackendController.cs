using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using IntroSE.Kanban.Backend.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.Backend.ServiceLayer;
using Kanban.Backend;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Windows.Markup;
using System.Windows.Controls;

namespace Frontend.Model
{
    public class BackendController
    {
        private SystemService Service { get; set; }
        public BackendController(SystemService service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new SystemService();
            Service.LoadData();
        }

        public UserModel Login(string username, string password)
        {
            string Json = Service.Login(username, password);
            var res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            //Response user = Service.Login(username, password);
            //if (user.ErrorOccured)

            return new UserModel(this, username.ToLower());
        }

        public UserModel Register(string username, string password)
        {
            string Json = Service.Register(username, password);
            var res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            //Response user = Service.Login(username, password);
            //if (user.ErrorOccured)

            return new UserModel(this, username.ToLower());
        }

        internal List<int> GetAllBoards(string email)
        {
            string Json = Service.GetUserBoards(email);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            //Array jsonList =(Array) res.ReturnValue;
            //List<int> boardsIds =new List<int>();
            JsonElement js = (JsonElement)res.ReturnValue;
            List<int> list = js.EnumerateArray().Select(e => e.GetInt32()).ToList();
            //IReadOnlyCollection<int> boards = boardsIds;
            return list;
            //return null;
        }

        internal (int Id, string boardName, string Owner) GetBoard(string email, int Id)
        {
            //string owner= Service.GetOwner(string boardName)
            string Json = Service.GetBoard(email, Id);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            //Message ans = Service.GetMessage(email, messageId).Value;
            //return (ans.Tid, ans.BoardName, ans.Owner);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            JsonElement js = (JsonElement)res.ReturnValue;
            BoardToSend b = JsonSerializer.Deserialize<BoardToSend>(js.GetRawText());
            return (b.id, b.name, b.owner);
        }



        internal void UpdateOwner(string emailOld, string emailNew, string boardName)
        {
            Service.TransferOwnership(emailOld, emailNew, boardName);
        }

        internal UserModel UserModel(string username, string password)
        {
            //Response res = Service.Register(username, password);
            //if (res.ErrorOccured)
            //{
            //    throw new Exception(res.ErrorMessage);
            //}
            string Json = Service.Register(username, password);
            var res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            return new UserModel(this, username.ToLower());
        }

        internal void DeleteBoard(string email, string boardName)
        {
            string Json = Service.DeleteBoard(email, boardName);
            var res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            //    Response res = Service.DeleteBoard(email, id);
            //    if (res.ErrorOccured)
            //    {
            //        throw new Exception(res.ErrorMessage);
            //    }
            MessageBox.Show($"Board {boardName} deleted successfully");
        }

        //internal IEnumerable<int> GetAllBoardsIds(string email)
        //{
        //    //IEnumerable<object> output= new List<object>();
        //    string json = Service.GetUserBoards(email);
        //    Response res = JsonSerializer.Deserialize<Response>(json);
        //    JsonElement val =(JsonElement) res.ReturnValue;
        //    List<int> list = val.EnumerateArray().Select(e => e.GetInt32()).ToList(); ;
        //    IEnumerable<int> output = list;
        //    return output;
        //}

        //internal IEnumerable<BoardToSend> GetAllBoardsIds(string email)
        //{
        //    //IEnumerable<object> output= new List<object>();
        //    string json = Service.GetBoardsWithDetailsOfUser(email);
        //    Response res = JsonSerializer.Deserialize<Response>(json);
        //    JsonElement val = (JsonElement)res.ReturnValue;
        //    List<int> list = val.EnumerateArray().Select(e => e.GetInt32()).ToList(); ;
        //    IEnumerable<int> output = list;
        //    return output;
        //}

        internal List<BoardToSend> GetAllBoardsOfUser(string email)
        {
            string json = Service.GetBoardsWithDetailsOfUser(email);
            Response res = JsonSerializer.Deserialize<Response>(json);
            JsonElement val = (JsonElement)res.ReturnValue;
            List<BoardToSend> boards = new List<BoardToSend>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                boards = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<BoardToSend>(element.GetRawText()))
                    .ToList();
            }
            return boards;
        }

        internal List<BoardToSend> GetAllBoardsComplement(string email)
        {
            string json = Service.GetBoardsWithDetailsComplement(email);
            Response res = JsonSerializer.Deserialize<Response>(json);
            JsonElement val = (JsonElement)res.ReturnValue;
            List<BoardToSend> boards = new List<BoardToSend>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                boards = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<BoardToSend>(element.GetRawText()))
                    .ToList();
            }
            return boards;
        }

        internal void Logout(string email)
        {
            Service.Logout(email);
        }


        internal List<TaskToSend> GetTasksInBacklog(string email, string boardName)
        {
            string json = Service.GetTasksInBacklog(email, boardName);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                return new List<TaskToSend>();
            }
            JsonElement val = (JsonElement)res.ReturnValue;
            List<TaskToSend> tasks = new List<TaskToSend>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                tasks = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<TaskToSend>(element.GetRawText()))
                    .ToList();
            }
            return tasks;
        }

        internal List<TaskToSend> GetTasksInProgress(string email, string boardName)
        {
            string json = Service.GetTasksInProgress(email, boardName);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                return new List<TaskToSend>();
            }
            JsonElement val = (JsonElement)res.ReturnValue;
            List<TaskToSend> tasks = new List<TaskToSend>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                tasks = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<TaskToSend>(element.GetRawText()))
                    .ToList();
            }
            return tasks;
        }

        internal List<TaskToSend> GetTasksInDone(string email, string boardName)
        {
            string json = Service.GetTasksInDone(email, boardName);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                return new List<TaskToSend>();
            }
            JsonElement val = (JsonElement)res.ReturnValue;
            List<TaskToSend> tasks = new List<TaskToSend>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                tasks = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<TaskToSend>(element.GetRawText()))
                    .ToList();
            }
            return tasks;
        }

        internal List<string> GetAllMembersInBoards(string boardName)
        {
            string json = Service.GetAllMembersInBoards(boardName);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                return new List<string>();
            }
            JsonElement val = (JsonElement)res.ReturnValue;
            List<string> members = new List<string>();
            if (val.ValueKind == JsonValueKind.Array)
            {
                members = val.EnumerateArray()
                    .Select(element => JsonSerializer.Deserialize<string>(element.GetRawText()))
                    .ToList();
            }
            return members;
        }

        internal void CreateBoard(string email, string newBoardName)
        {
            string Json = Service.CreateBoard(email, newBoardName);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            //Message ans = Service.GetMessage(email, messageId).Value;
            //return (ans.Tid, ans.BoardName, ans.Owner);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            MessageBox.Show($"Board {newBoardName} created successfully");
        }

        internal void JoinBoard(string email, int id)
        {
            string Json = Service.JoinBoard(email, id);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            MessageBox.Show($"Joined to Board {id} successfully");
        }

        internal void SetAssignee(string email, string boardName, int columnOrdinal, int taskId, string newAssignee)
        {
            string Json = Service.AssignTask(email, boardName, columnOrdinal, taskId, newAssignee);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            MessageBox.Show($"Task {taskId} is now assigned to {newAssignee}");
        }

        internal void UpdateDueDate(string email, string boardName, int column, int id, DateTime newDate)
        {
            string Json = Service.UpdateTaskDueDate(email, boardName, column, id, newDate);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal void UpdateDescription(string email, string boardName, int column, int id, string description)
        {
            string Json = Service.UpdateTaskDescription(email, boardName, column, id, description);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal void UpdateTaskTitle(string email, string boardName, int column, int taskId, string newTitle)
        {
            string Json = Service.UpdateTaskTitle(email, boardName, column, taskId, newTitle);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal List<Task> ShowInProgressSender(string email)
        {
            string Json = Service.InProgressTasks(email);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            List<Task> tasks = new List<Task>();
            //JsonElement val = (JsonElement)res.ReturnValue;
            //if (val.ValueKind == JsonValueKind.Array)
            //{
            //    tasks = val.EnumerateArray()
            //        .Select(element => JsonSerializer.Deserialize<Task>(element.GetRawText()))
            //        .ToList();
            //}
            return tasks;
        }

        internal (int, string, DateTime, DateTime, string, string) GetTaskInColumn(string email, string boardName, int id, int col)
        {
            string json = Service.GetTaskInColumn(email, boardName, id, col);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }

            JsonElement js = (JsonElement) res.ReturnValue;
            string ar = js.GetString();
            string[] values = ar.Split(',');

            int intValue = int.Parse(values[0].Trim('[', ']'));
            string stringValue1 = values[1].Trim('"');
            DateTime dateTimeValue1 = DateTime.Parse(values[2].Trim('"'));
            DateTime dateTimeValue2 = DateTime.Parse(values[3].Trim('"'));
            string stringValue2 = values[4].Trim('"');
            string stringValue3 = values[5].Trim('"');


            var info = (intValue, stringValue1, dateTimeValue1, dateTimeValue2, stringValue2, stringValue3);
            return info;
        }

        internal int GetLimitBacklog(string email,string bname)
        {
            string Json = Service.GetColumnLimit(email, bname, 0);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            return int.Parse(res.ReturnValue.ToString());
        }

        internal int GetLimitInProg(string email, string bname)
        {
            string Json = Service.GetColumnLimit(email, bname, 1);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            return int.Parse(res.ReturnValue.ToString());
        }

        internal int GetLimitDone(string email, string bname)
        {
            string Json = Service.GetColumnLimit(email, bname, 1);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
            return int.Parse(res.ReturnValue.ToString());
        }

        internal void setLimitBacklog(string email, string bname, int lim)
        {
            string Json = Service.LimitColumn(email, bname, 0,lim);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal void setLimitInProg(string email, string bname, int lim)
        {
            string Json = Service.LimitColumn(email, bname, 1,lim);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal void setLimitDone(string email, string bname, int lim)
        {
            string Json = Service.LimitColumn(email, bname, 2, lim);
            Response res = JsonSerializer.Deserialize<Response>(Json);
            if (res.isErrorOccured())
            {
                throw new KanException(res.ErrorMessage);
            }
        }

        internal List<string> GetMembers(string email, string boardName)
        {
            string json = Service.GetAllMembersInBoards(boardName);
            Response res = JsonSerializer.Deserialize<Response>(json);
            if (res.isErrorOccured())
            {
                return new List<string>();
            }

            JsonElement js = (JsonElement)res.ReturnValue;
            return JsonSerializer.Deserialize<List<string>>(js);
        }
    }
}
