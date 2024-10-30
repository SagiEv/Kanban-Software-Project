using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using log4net;
using System.Reflection;
using System.Data.SqlClient;
using Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer
{
    internal class TaskController
    {
        private const string TaskTableName = "Tasks";
        private readonly string _connectionString;
        private readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public TaskController()
        {

            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = TaskTableName;

        }
        /// <summary>
        /// This method gets a list of all tasks in the database.
        /// </summary>
        /// <returns></returns>
        public List<TaskDTO> GetAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method gets a list of all tasks in specific board in the database.
        /// </summary>
        /// <returns></returns>
        public List<TaskDTO> GetAllTasksInBoard(int boardId)
        {
            List<TaskDTO> result = SelectTasks(boardId).Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// This method adds TaskDTO.
        /// </summary>
        /// <returns></returns>
        public void Insert(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.IDColumnName} ,{TaskDTO.BoardIdColumnName},{TaskDTO.CreationTimeColumnName},{TaskDTO.TitleColumnName},{TaskDTO.DescriptionColumnName},{TaskDTO.DueDateColumnName},{TaskDTO.AssigneeColumnName}, {TaskDTO.NumberOfCloumnColumnName}) " +
                        $"VALUES (@idVal,@boardIdVal,@creationTimeVal,@titleVal,@descriptionVal,@dueDateVal,@assigneeVal,@numberOfCloumnVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.ID);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", task.BoardId);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", task.CreationTime);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.Assignee);
                    SQLiteParameter numberOfCloumnParam = new SQLiteParameter(@"numberOfCloumnVal", task.Column);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(numberOfCloumnParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Failed insertion on {_tableName} table");
                    throw new KanException($"Cannot perform insert on {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
            }
        }
        /// <summary>
        ///  This method updates a value in Task table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(int id, string attributeName, string attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
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
        ///  This method updates a value in Task table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(int id, string attributeName, int attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
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
        ///  This method updates a value in Task table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(int id, string attributeName, DateTime attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
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
        /// This method select all Task table.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        public List<TaskDTO> Select()
        {
            List<TaskDTO> results = new List<TaskDTO>();
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
        /// This method gets a list of all tasks in specific board in the database.
        /// </summary>
        /// <returns></returns>
        protected List<TaskDTO> SelectTasks(int boardId)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new(null, connection) { CommandText = $"select * from {TaskTableName} where BoardID={boardId};" };
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.Add(ConvertReaderToObject(dataReader));
                    log.Info($"Successful select in {TaskTableName} table");
                }
                catch
                {
                    log.Error($"Failed select on {TaskTableName} table");
                    throw new KanException($"Cannot perform select on {TaskTableName} table");
                }
                finally
                {
                    if (dataReader != null) dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        /// <summary>
        /// This method deletes TaskDTOs from the database.
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <exception cref="KanException"></exception>
        public void Delete(TaskDTO DTOObj)
        {
   
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where id={DTOObj.ID}"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Error($"Failed deletion from {_tableName} where Id={DTOObj.ID}");
                    throw new KanException($"Cannot delete from {_tableName} where Id = {DTOObj.ID}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// This method reads a row in the Task table in the database.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {

            int id = reader.GetInt32(0);
            int bid = reader.GetInt32(1);
            DateTime cTime = reader.GetDateTime(2);
            string title = reader.GetString(3);
            string desc = "";
            if (!reader.IsDBNull(4))
            {
                desc = reader.GetString(4);
            }
            DateTime dueDate = reader.GetDateTime(5);
            string assignee = "";
            if (!reader.IsDBNull(6))
            {
                assignee = reader.GetString(6);
            }
            int col = reader.GetInt32(7);
            return new TaskDTO(id, bid, cTime, title, desc, dueDate, assignee, col);
        }
        /// <summary>
        /// This method return the max task's id in the database.
        /// </summary>
        /// <returns></returns>
        public int GetMaxTaskId()
        {
            int maxTaskId = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                string query = $"SELECT MAX({TaskDTO.IDColumnName}) FROM {_tableName};";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    if (!reader.IsDBNull(0))
                    {
                        maxTaskId = reader.GetInt32(0);
                    }
                }
                reader.Close();
                connection.Close();
            }
            return maxTaskId;
        }

        /// <summary>
        /// This method change the assignee of specific task in Task table.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="value"></param>
        /// <param name="assigneeColumnName"></param>
        /// <param name="taskId"></param>
        /// <param name="col"></param>
        /// <exception cref="KanException"></exception>
        internal void ChangeAssignee(string email, string value, string assigneeColumnName, int taskId, int col)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{assigneeColumnName}]=@{assigneeColumnName} where email={email} and ID = {taskId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(assigneeColumnName, value));
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Successful select in {TaskTableName} table");
                }
                catch
                {
                    log.Error($"Failed change assignee in {_tableName} table");
                    throw new KanException($"Cannot change assignee in {_tableName} table");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
        }

        /// <summary>
        /// This method delete all the data in Task table.
        /// </summary>
        internal void DeleteTableData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("DELETE FROM Tasks", connection);

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
        /// <summary>
        /// This method delete all the task in a specific board in Task table.
        /// </summary>
        internal void DeleteTasksOfBoard(int bid)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where BoardID={bid}"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Info($"Deletion successful from DB {_tableName} table");
                }
                catch
                {
                    log.Debug($"Didn't find any tasks where BoardID={bid} in {_tableName}");
                    throw new KanException($"Didn't find any tasks where BoardID={bid} in {_tableName}");
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

