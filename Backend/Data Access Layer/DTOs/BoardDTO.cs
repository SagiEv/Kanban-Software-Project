using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs
{
    internal class BoardDTO
    {
        //those are the titles for the columns of boards table
        public const string BoardID = "ID";
        public const string BoardName = "Name";
        public const string BacklogLim = "backlog_lim";
        public const string InProgressLim = "in_progress_lim";
        public const string DoneLim = "done_lim";
        public const string Owner = "owner";
        private BoardController _controller;

        //fields
        /// <summary>
        /// Get for id of BoardDTO.
        /// </summary>
        private int _boardId;
        public int BoardId 
        {
            get => _boardId;
            //set { _boardId = value; _controller.Update(_boardId, BoardID, _boardId); }
        }

        /// <summary>
        /// Get for name of BoardDTO.
        /// </summary>
        private string _boardName;
        public string boardName 
        {
            get => _boardName;
            //set { _boardName = value; _controller.Update(_boardId, BoardName, _boardName); }
        }

        /// <summary>
        /// Get and Set for owner of BoardDTO.
        /// </summary>
        private string _owner;
        public string owner {
            get => _owner;
            set { _owner = value; _controller.Update(_boardId, Owner, _owner); }
         }

        /// <summary>
        /// Get and Set for limit backlog column of BoardDTO.
        /// </summary>
        private int _backlog;
        public int limit_backlog
        {
            get => _backlog;
            set { _backlog = value; _controller.Update(_boardId, BacklogLim, _backlog); }
        }

        /// <summary>
        /// Get and Set for limit inprogress column of BoardDTO.
        /// </summary>
        private int _inprog;
        public int limit_inprogress
        {
            get => _inprog;
            set { _inprog = value; _controller.Update(_boardId, InProgressLim, _inprog); }
        }

        /// <summary>
        /// Get and Set for limit done column of BoardDTO.
        /// </summary>
        private int _done;
        public int limit_done
        {
            get => _done;
            set { _done = value; _controller.Update(_boardId, DoneLim, _done); }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="bname"></param>
        /// <param name="owner"></param>
        /// <param name="lim_backlog"></param>
        /// <param name="lim_inprog"></param>
        /// <param name="lim_done"></param>
        public BoardDTO(int ID, string bname, string owner,int lim_backlog,int lim_inprog, int lim_done)
        {
            _controller = new BoardController();
            _boardId = ID;
            _boardName = bname;
            _owner = owner;
            _backlog = lim_backlog;
            _inprog = lim_inprog;
            _done = lim_done; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>this function return the name of the column 
        public string colName(int col)
        {
            if (col == 0) return BacklogLim;
            else if (col == 1) return InProgressLim;
            else return DoneLim;
        }
    }
}
