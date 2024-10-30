using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kanban.Backend;
namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskToSend
    {
        public int id { get; set; }
        public string title { get; set; }
        public string assignee { get; set; }

        internal TaskToSend(Task t)
        {
            this.id = t.Id;
            this.title = t.Title;
            this.assignee = t.GetAssignee();
        }
        public TaskToSend()
        {
            // Parameterless constructor
        }
    }
}
