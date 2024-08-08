using IntroSE.Kanban.Backend.Backend.Business_Layer;
using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Reflection;
using System.IO;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;

namespace Kanban.Backend.Business_Layer
{
    internal class UserFacade
    {
        //fields
        private Dictionary<string,User> users;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserController controller;

        //constructor
        internal UserFacade()
        {
            users = new Dictionary<string, User>();
            log.Info("UserFacade object was initiated");
            LoadData();
            controller = new UserController();
        }
        
        //methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        public User GetUser(string email)
        {
            log.Info("inside getUser");
            CheckEmail(email);
            email = email.ToLower();
            if (users.ContainsKey(email))
                return users[email];
            else
            {
                throw new KanException($"e-mail address: {email} was not found in the system");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        /// <exception cref="KanException"></exception>
        internal void ChangePassword(string email, string oldPass, string newPass)
        {
            log.Info("inside UserFacade changePassword");
            CheckEmail(email);
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} was not found in the system");
            }
            else if (!users[email].GetPassword().Equals(oldPass))
            {
                throw new KanException("incorrect old Password!");
            }       
            users[email].ChangePassword(newPass);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal void Register(string email, string password)
        {
            log.Info("registering a new user");
            CheckEmail(email);
            email = email.ToLower();
            if (users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} already exists in the system");
            }
            //CheckEmail(email);

            User user = new User(email, password);
            user.CheckPassword(password);
            UserDTO dto = new UserDTO(email, password, false);
            controller.Insert(dto);
            //if (controller.Insert(dto))
            //{
            users.Add(email, user);
            Login(email, password);
                //user.Login(password);
            //}
            //else
            //{
            //    throw new KanException($"registration of user: {email} has failed!");
            //}
            log.Info($"a new user with e-mail addres: {email} and password: {password} was registered successfully");
            //return user; // ?maybe need to change return type to void?
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="KanException"></exception>
        private void CheckEmail(string email)
        {
            log.Info("inside UserFacade CheckEmail");            
            if (email == null)
            {
                throw new KanException("Email can't be null!");
            }
            bool isEmail = Regex.IsMatch(email, @"\A(?:[A-Za-z][A-Za-z0-9]*(?:\.[A-Za-z][A-Za-z0-9]*)*@(?:[A-Za-z](?:[A-Za-z]*[A-Za-z])?\.)+[A-Za-z][A-Za-z]*[A-Za-z]*)\Z", RegexOptions.IgnoreCase);
            if (email.Length==0 || !isEmail || !email.Contains("@") || email.Contains(" "))
            {
                throw new KanException("Email is invalid!");
            }
            log.Info($"e-mail address: {email} checking is completed");
        }


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal string Login(string email, string password)
        {
            log.Info("inside UserFacade login");
            CheckEmail(email);
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} was not found in the system!");
            }
            try
            {
                users[email].Login(password);
            }
            catch (KanException ke)
            {
                throw ke;
            }
            //users[email].Login(password);

            log.Info($"user {email} has logged in successfully");
            return email;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal void Logout(string email) {
            log.Info("inside UserFacade logout");
            CheckEmail(email);
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} was not found in the system!");
            }
            else if (!users[email].IsLoggedIn()) {
                throw new KanException($"user {email} is already logged out"); 
            }
            users[email].Logout();
            log.Info($"user {email} has logged out successfully");
        }


        // probably we can delete this
/*        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <exception cref="KanException"></exception>
        internal void CreateUser(string email, string password)
        {
            log.Info("Creating User");
            CheckEmail(email); //if doesnt return an exception
            email = email.ToLower();
            if (users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} already exists in the system");
            }
            User user = new(email, password);// maybe add an empty boardFacade for the begin.
            users[email] = user;
            log.Info($"a new User with an e-mail address: {email} and a password: {password} was created successfully");
        }*/



        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <exception cref="KanException"></exception>
        internal void CheckLoggedIn(string email)
        {
            log.Info("inside UserFacade isLoggedIn");
            CheckEmail(email);
            email = email.ToLower();
            if (!users.ContainsKey(email))
            {
                throw new KanException($"e-mail address: {email} does not exist in the system");
            }
            if(!users[email].IsLoggedIn())
            {
                throw new KanException($"user with the email: {email} is not logged-in!");
            }
        }
        /// <summary>
        /// this method load the data from the database.
        /// </summary>
        internal void LoadData()
        {
            List<UserDTO> usersDB = new UserController().SelectAllUsers();
            foreach (UserDTO u in usersDB)
            {
                User user = new User(u);
                users.Add(u.Email, user);
            }
        }

        /// <summary>
        /// this method delete data of the database.
        /// </summary>
        internal void DeleteData()
        {
            controller.DeleteTableData();
            users = new Dictionary<string, User>();
        }
    }
}
