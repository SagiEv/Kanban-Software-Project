using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer
{
    internal class User2BoardsController
    {
        private const string BoardsTableName = "Users2Boards";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public User2BoardsController() 
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;"; 
            this._tableName = BoardsTableName;
        }


        /// <summary>
        /// This method select all User2Board table.
        /// </summary>
        /// <returns></returns>
        public List<User2BoardDTO> Select()
        {
            List<User2BoardDTO> results = new List<User2BoardDTO>();
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
                        results.Add(ConvertReaderToBoardDTO(dataReader));
                        log.Info("Successful Select");
                    }
                }
                catch
                {
                    log.Error("Failed Select");
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
        /// This method select all the boards.
        /// </summary>
        /// <returns></returns>
        public List<User2BoardDTO> SelectAllBoards()
        {
            List<User2BoardDTO> result = Select().Cast<User2BoardDTO>().ToList();

            return result;
        }


        /// <summary>
        /// This method adds User2BoardDTO.
        /// </summary>
        /// <param name="board"></param>
        public void Insert(User2BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardsTableName} ({User2BoardDTO.Email}, {User2BoardDTO.BoardID} ,{User2BoardDTO.BoardName}) " +
                        $"VALUES (@emailVal,@idVal,@nameVal);";


                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.BoardId);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", board.boardName);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", board.email);


                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Prepare();
                    command.ExecuteNonQuery();
                    log.Info("Successful Insert");
                }
                catch
                {
                    log.Error($"Failed insert in {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This method deletes User2BoardDTO.
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <exception cref="KanException"></exception>
        public void Delete(User2BoardDTO DTOObj)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where {User2BoardDTO.BoardID}={DTOObj.BoardId} AND {User2BoardDTO.Email}='{DTOObj.email}'"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={DTOObj.BoardId}");
                    throw new KanException($"Cannot delete from {_tableName} where Id = {DTOObj.BoardId}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where {User2BoardDTO.BoardID}={id}"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={id}");
                    throw new KanException($"Cannot delete from {_tableName} where Id = {id}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        //public bool Update(int id, string attributeName, string attributeValue)
        //{
        //    int res = -1;
        //    using (var connection = new SQLiteConnection(_connectionString))
        //    {
        //        SQLiteCommand command = new SQLiteCommand
        //        {
        //            Connection = connection,

        //            CommandText = $"update {_tableName} set [{attributeName}]=@attributeValue  {User2BoardDTO.BoardID}={DTOObj.boardId} AND {User2BoardDTO.Email}={DTOObj.email}"
        //        };
        //        try
        //        {

        //            command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
        //            connection.Open();
        //            res = command.ExecuteNonQuery();
        //        }
        //        catch
        //        {
        //            //log
        //        }
        //        finally
        //        {
        //            command.Dispose();
        //            connection.Close();
        //        }

        //    }
        //    return res > 0;
        //}

        //public bool Update(int id, string attributeName, int attributeValue)
        //{
        //    int res = -1;
        //    using (var connection = new SQLiteConnection(_connectionString))
        //    {
        //        SQLiteCommand command = new SQLiteCommand
        //        {
        //            Connection = connection,
        //            CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
        //        };
        //        try
        //        {
        //            command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
        //            connection.Open();
        //            command.ExecuteNonQuery();
        //        }
        //        finally
        //        {
        //            command.Dispose();
        //            connection.Close();

        //        }

        //    }
        //    return res > 0;
        //}

        //public bool Update(string name, string attributeName, string attributeValue)
        //{
        //    int res = -1;
        //    using (var connection = new SQLiteConnection(_connectionString))
        //    {
        //        SQLiteCommand command = new SQLiteCommand
        //        {
        //            Connection = connection,

        //            CommandText = $"update {_tableName} set [{attributeName}]=@attributeValue where name={name}"
        //        };
        //        try
        //        {

        //            command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
        //            connection.Open();
        //            res = command.ExecuteNonQuery();
        //        }
        //        catch
        //        {
        //            //log
        //        }
        //        finally
        //        {
        //            command.Dispose();
        //            connection.Close();
        //        }

        //    }
        //    return res > 0;
        //}

        /// <summary>
        /// This method reads a row in the User2Board table in the database.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public User2BoardDTO ConvertReaderToBoardDTO(SQLiteDataReader reader)
        {
            return new User2BoardDTO(reader.GetString(0), (int)reader.GetInt32(1), reader.GetString(2));
        }

        /// <summary>
        /// this method find all members of specific board id.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public List<string> GetMembers(int boardId)
        {
            List<string> memberEmails = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = $"SELECT {User2BoardDTO.Email} FROM {_tableName} WHERE {User2BoardDTO.BoardID} = @BoardId;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BoardId", boardId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string email = reader["email"].ToString();
                    if (!string.IsNullOrEmpty(email))
                    {
                        memberEmails.Add(email);
                    }
                }
                reader.Close();
                connection.Close();
            }
            return memberEmails;
        }

        /// <summary>
        /// this method select all User2Board and return a list.
        /// </summary>
        /// <returns></returns>
        internal List<User2BoardDTO> SelectAllUsers2Boards()
        {
            List<User2BoardDTO> result = Select().Cast<User2BoardDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method delete all the data in User2Boards table.
        /// </summary>
        internal void DeleteTableData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("DELETE FROM Users2Boards", connection);

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
