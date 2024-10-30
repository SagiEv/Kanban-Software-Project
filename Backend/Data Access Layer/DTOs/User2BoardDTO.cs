using Kanban.Backend.Business_Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs
{
    internal class User2BoardDTO
    {
        public const string BoardID = "BoardID";
        public const string BoardName = "BoardName";
        public const string Email = "Email";
        private BoardController _controller;

        //fields
        /// <summary>
        /// Get for id of User2BoardsDTO.
        /// </summary>
        private int _boardId;
        public int BoardId
        {
            get => _boardId;
            //set { _boardId = value; } // _controller.Update(_email,_boardId, BoardID, _boardId); }
        }

        /// <summary>
        /// Get for board name of User2BoardsDTO.
        /// </summary>
        private string _boardName;
        public string boardName
        {
            get => _boardName;
            //set { _boardName = value; }
            //_controller.Update(_email, _boardId, BoardName, _boardName); }
        }

        /// <summary>
        /// Get for email of User2BoardsDTO.
        /// </summary>
        private string _email;
        public string email
        {
            get => _email;
            //set { _email = value; }//_controller.Update(_email, _boardId, Email, _email); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ID"></param>
        /// <param name="bname"></param>
        public User2BoardDTO(string e, int ID, string bname)
        {
            _email = e;
            _boardId = ID;
            _boardName = bname;
            _controller = new BoardController();
        }
    }
}
