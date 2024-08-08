using Kanban.Backend.Business_Layer;
using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using System.Configuration;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;

namespace IntroSE.Kanban.Backend.Backend.Business_Layer
{
    internal class User
    {
        //fields
        private string email;
        private string password;
        private bool loggedIn;
        private UserDTO userDTO;
        private UserController controller;
        private readonly int MIN_PASSWORD_LENGTH = 6;
        private readonly int MAX_PASSWORD_LENGTH = 20;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        //constructor
        internal User(string email, string password)
        {
            CheckPassword(password);
            this.email = email;
            this.password = password;
            // needs to decide if loggedin should be initiated with true or false value
            this.loggedIn = false;
            this.userDTO = new UserDTO(email,password);
            this.controller = new UserController();
            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        internal User(UserDTO userDTO)
        {
            this.email = userDTO.Email;
            this.password = userDTO.Password;
            this.loggedIn = userDTO.LoggedIn;
            this.userDTO = userDTO;
            this.controller = new UserController();
        }

        //methods

        //maybe we need a method that return all the boards the user has.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal void Login(string password)
        {
            log.Info("inside User login");
            if (this.IsLoggedIn())
            {
                throw new KanException("this user is already logged in!");
            }
            if (password == null)
            {
                throw new KanException("password is null");
            }
            if (password.Equals(this.password))
            {               
                userDTO.LoggedIn = true;
                this.loggedIn = true;

            }
            else
            {
                throw new KanException("incorrect password, please try again!");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="KanException"></exception>
        internal void Logout()
        {
            log.Info("inside User logout");
            if (!this.IsLoggedIn())
            {
                throw new KanException("this user is already logged out!");
            }
            userDTO.LoggedIn= false;
            this.loggedIn = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal bool IsLoggedIn()
        {
            return this.loggedIn;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal string GetPassword() // maybe should change to private?
        {
            return this.password;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPassword"></param>
        internal void ChangePassword(string newPassword)
        {
            CheckPassword(newPassword);
            //if (controller.Update(email, "password", newPassword))
            userDTO.Password = newPassword;
            this.password = newPassword;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <exception cref="KanException"></exception>
        internal void CheckPassword(string password) //check if the password is valid
        {
            if (password == null)
                throw new KanException("Password is null");
            if (password.Length < MIN_PASSWORD_LENGTH || password.Length > MAX_PASSWORD_LENGTH)
            {
                throw new KanException("Password length is not valid");
            }
            bool upperCase = false;
            bool lowerCase = false;
            bool digit = false;
            for (int i = 0; i < password.Length; i++)
            {
                char c = password[i];
                if (char.IsUpper(c))
                    upperCase = true;
                if (char.IsLower(c))
                    lowerCase = true;
                if (char.IsNumber(c))
                    digit = true;
            }
            if (!digit || !upperCase || !lowerCase)
            {
                throw new KanException("a valid password must contain at least 1 of the following: uppercase letter, lowercase letter, digit");
            }
        }
    }
}
