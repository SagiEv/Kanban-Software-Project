using System;
using System.Text.Json;
using Kanban.Backend.ServiceLayer;
using Kanban.Backend.Business_Layer;
using log4net;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class UserService
    {
        private UserFacade uf;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserService()
        {
            this.uf = new UserFacade();
        }

        public UserService(UserFacade uf)
        {
            this.uf = uf;
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs </returns>
        public string Register(string email, string password)
        {
            log.Info("inside UserService Register");
            try
            {
                uf.Register(email,password);
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
        ///  This method logs in an existing user.
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response with the user's email, unless an error occurs <returns>
        public string Login(string email, string password)
        {
            log.Info("inside UserService Login");
            try
            {
                return JsonSerializer.Serialize(new Response(uf.Login(email, password)));
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
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs  <returns>
        public string Logout(string email)
        {
            log.Info("inside UserService Logout");
            try
            {
                uf.Logout(email);
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
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal string CheckLoggedIn(string email)
        {
            log.Info("inside UserService IsLoggedIn");
            try
            {
                uf.CheckLoggedIn(email);
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
                return JsonSerializer.Serialize(new Response(e)); 
            }
        }

        internal string LoadData()
        {
            try
            {
                uf.LoadData();
            }
            catch 
            {
                log.Debug("there is no available data in DB");
            }
            return JsonSerializer.Serialize(new Response());
        }

        internal string DeleteData()
        {
            try
            {
                uf.DeleteData();
            }
            catch
            {
                log.Debug("there is no available data in DB");
            }
            return JsonSerializer.Serialize(new Response());
        }
    }
}
