using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs
{
    internal class UserDTO
    {
        public const string UserEmailColumnName = "email";
        public const string UserPasswordColumnName = "password";
        public const string UserLoggedInColumnName = "isLoggedIn";
        private UserController _controller;

        private readonly string _email;
        /// <summary>
        /// Get for email of UserDTO.
        /// </summary>
        public string Email
        {
            get => _email;
        }

        private string _password;
        /// <summary>
        /// Get and set for password of UserDTO.
        /// </summary>
        public string Password
        {
            get => _password; set { _password = value; _controller.Update(_email, UserPasswordColumnName, _password); }
        }

        private bool _isLoggedIn;
        /// <summary>
        /// Get and set for isLoggedIn of UserDTO.
        /// </summary>
        public bool LoggedIn
        {
            get => _isLoggedIn; 
            set 
            {
                _isLoggedIn = value;
                _controller.Update(_email, UserLoggedInColumnName, _isLoggedIn); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        public UserDTO(string Email, string Password)
        {
            _email = Email;
            _password = Password;
            _isLoggedIn = false;
            _controller = new UserController();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <param name="IsLoggedIn"></param>
        public UserDTO(string Email, string Password, bool IsLoggedIn)
        {
            _email = Email;
            _password = Password;
            _isLoggedIn = IsLoggedIn;
            _controller = new UserController();
        }

    }
}
