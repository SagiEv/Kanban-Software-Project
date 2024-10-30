using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs
{
    internal class TaskDTO
    {
        public const string IDColumnName = "ID";
        public const string BoardIdColumnName = "BoardID";
        public const string CreationTimeColumnName = "CreationTime";
        public const string TitleColumnName = "Title";
        public const string DescriptionColumnName = "Description";
        public const string DueDateColumnName = "DueDate";
        public const string AssigneeColumnName = "Assignee";
        public const string NumberOfCloumnColumnName = "Column";
        private TaskController _controller;

        /// <summary>
        /// Get for id of taskDTO.
        /// </summary>
        private int _id;
        public int ID { get => _id; }

        /// <summary>
        /// Get for boardId of taskDTO.
        /// </summary>
        private int _boardId;
        public int BoardId
        {
            get => _boardId;
        }

        /// <summary>
        /// Get and set for title of taskDTO.
        /// </summary>
        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; _controller.Update(ID, TitleColumnName, value); }
        }

        /// <summary>
        /// Get for creation time of taskDTO.
        /// </summary>
        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; }

        /// <summary>
        /// Get and set for description of taskDTO.
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; _controller.Update(ID, DescriptionColumnName, value); }
        }

        /// <summary>
        /// Get and set for duedate of taskDTO.
        /// </summary>
        private DateTime _duedate;
        public DateTime DueDate
        {
            get => _duedate;
            set { _duedate = value; _controller.Update(ID, DueDateColumnName, value); }
        }

        /// <summary>
        /// Get and set for assignee of taskDTO.
        /// </summary>
        private string _assignee;
        public string Assignee
        {
            get => _assignee;
            set { _assignee = value; _controller.Update(ID, AssigneeColumnName, value); }
        }

        /// <summary>
        /// Get and set for column of taskDTO.
        /// </summary>
        private int _column;
        public int Column
        {
            get => _column;
            set { _column = value; _controller.Update(ID, NumberOfCloumnColumnName, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="boardid"></param>
        /// <param name="creationtime"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="assignee"></param>
        /// <param name="column"></param>
        public TaskDTO(int id, int boardid, DateTime creationtime, string title, string description, DateTime dueDate, string assignee, int column)
        {
            _id = id;
            _boardId = boardid;
            _creationTime = creationtime;
            _title = title;
            _description = description;
            _duedate = dueDate;
            _assignee = assignee;
            _column = column;
            _controller = new TaskController();
        }
    }
}
