using Frontend.ViewModel;
using IntroSE.Kanban.Backend;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.Backend;
using Kanban.Backend;
using System.Collections.Specialized;

namespace Frontend.Model
{
    internal class InProgressModel : NotifiableModelObject
    {
        private readonly UserModel user;
        //private readonly BoardsModel boards;
        public ObservableCollection<TaskModel> InProgressTasks { get; set; }

        public string BoardName { get; set; }

        public int BID { get; set; }

        private BackendController controller;

        public InProgressModel(BackendController controller,UserModel user,string bname,int bid): base(controller)
        {

            this.user = user;
            controller.ShowInProgressSender(user.Email);
            BoardName = bname;
            BID = bid;
            List<Task> tasks = controller.ShowInProgressSender(user.Email);
            ObservableCollection<TaskModel> InProgressTasks = new ObservableCollection<TaskModel>();
            foreach (Task t in tasks)
            {
                List<string> members = user.Controller.GetAllMembersInBoards(user.Controller.GetBoard(user.Email, t.GetBoardId()).boardName);
                TaskModel taskModel = new TaskModel(controller,user,new ObservableCollection<string>(members),t.Id,t.Title,t.Description,t.CreationTime,t.DueDate,t.GetAssignee());
                InProgressTasks.Add(taskModel);
            }
            InProgressTasks = new ObservableCollection<TaskModel>(InProgressTasks);
            InProgressTasks.CollectionChanged += HandleChange;
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


    }
}
