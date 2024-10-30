using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using System.Reflection;
using log4net;
using Kanban.Backend.ServiceLayer;
using System.Text.Json;
using IntroSE.Kanban.Backend.Backend.Business_Layer;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer
{
    internal class UserController
    {
        private const string UserTableName = "Users";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UserController()
        {
            string pathUnsliced = Directory.GetCurrentDirectory();
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = UserTableName;
        }

        /// <summary>
        /// This method select all User table.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        public List<UserDTO> Select()
        {
            List<UserDTO> results = new List<UserDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                        log.Info("Successful Select");
                    }
                }
                catch
                {
                    log.Error("Failed Select");
                    throw new KanException("Cannot perform Select on DB table");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        /// <summary>
        /// This method select all the users.
        /// </summary>
        /// <returns></returns>
        public List<UserDTO> SelectAllUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();

            return result;
        }


        /// <summary>
        /// This method adds UserDTO.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="KanException"></exception>
        public void Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.UserEmailColumnName} ,{UserDTO.UserPasswordColumnName}, {UserDTO.UserLoggedInColumnName}) " +
                        $"VALUES (@EmailVal,@PasswordVal,@LoggedInVal);";
                    SQLiteParameter EmailVal = new SQLiteParameter(@"EmailVal", user.Email);
                    SQLiteParameter PasswordVal = new SQLiteParameter(@"PasswordVal", user.Password);
                    SQLiteParameter LoggedInVal = new SQLiteParameter(@"LoggedInVal", user.LoggedIn);

                    command.Parameters.Add(EmailVal);
                    command.Parameters.Add(PasswordVal);
                    command.Parameters.Add(LoggedInVal);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    log.Info("Successful Insert");
                }
                catch
                {
                    log.Error($"Failed insert in {_tableName} table");
                    throw new KanException($"Cannot insert in {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This method deletes UserDTOs from the database.
        /// </summary>
        /// <param name="user"></param>
        /// <exception cref="KanException"></exception>
        public void Delete(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where email={user.Email}"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Email={UserDTO.UserEmailColumnName}");
                    throw new KanException($"Cannot delete from {_tableName} where Email = {UserDTO.UserEmailColumnName}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// This method updates a value in User table.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(string email, string attributeName, string attributeValue)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@attributeValue where email={email}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed update in {_tableName} table");
                    throw new KanException($"Cannot update in {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// This method updates a value in User table.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(string email, string attributeName, bool attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where email='{email}'"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Successful update in DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed update in {_tableName} table");
                    throw new KanException($"Cannot update in {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
        }
        /// <summary>
        /// this method parse string to bool
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool ParseStringToBool(string input)
        {
            return input.Equals("true");
        }

        /// <summary>
        /// This method reads a row in the User table in the database.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            string stringLoggedin = reader.GetString(2);
            bool booleanLoggedin = (stringLoggedin == "1");
            return new UserDTO(reader.GetString(0), reader.GetString(1), booleanLoggedin);
        }

        /// <summary>
        /// This method delete all the data in User table.
        /// </summary>
        internal void DeleteTableData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("DELETE FROM Users", connection);

                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }
    }
}
