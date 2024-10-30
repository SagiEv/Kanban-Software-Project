using IntroSE.Kanban.Backend.Backend.Data_Access_Layer.DTOs;
using Kanban.Backend.ServiceLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.Backend.Data_Access_Layer
{
    internal class BoardController
    {
        private const string BoardsTableName = "Boards";
        private readonly string _connectionString;
        private readonly string _tableName;
        private TaskController _taskController;
        private User2BoardsController _u2bController;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = BoardsTableName;
            this._taskController = new TaskController();
            this._u2bController = new User2BoardsController();
        }

        /// <summary>
        /// This method select all Board table.
        /// </summary>
        /// <returns></returns>
        public List<BoardDTO> Select()
        {
            List<BoardDTO> results = new List<BoardDTO>();
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
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();

            return result;
        }


        /// <summary>
        /// This method adds BoardDTO.
        /// </summary>
        /// <param name="board"></param>
        /// <exception cref="KanException"></exception>
        public void Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardsTableName} ({BoardDTO.BoardID} ,{BoardDTO.BoardName},{BoardDTO.Owner} ,{BoardDTO.BacklogLim},{BoardDTO.InProgressLim},{BoardDTO.DoneLim}) " +
                        $"VALUES (@idVal,@nameVal,@ownerVal,@backlogVal,@inprogressVal,@doneVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.BoardId);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", board.boardName);
                    SQLiteParameter ownerParam = new SQLiteParameter(@"ownerVal", board.owner);
                    SQLiteParameter backlogLimParam = new SQLiteParameter(@"backlogVal", board.limit_backlog);
                    SQLiteParameter inprogressLimParam = new SQLiteParameter(@"inprogressVal", board.limit_inprogress);
                    SQLiteParameter doneLimParam = new SQLiteParameter(@"doneVal", board.limit_done);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(ownerParam);
                    command.Parameters.Add(backlogLimParam);
                    command.Parameters.Add(inprogressLimParam);
                    command.Parameters.Add(doneLimParam);
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
        /// This method deletes BoardDTOs from the database.
        /// </summary>
        /// <param name="DTOObj"></param>
        /// <exception cref="KanException"></exception>
        public void Delete(BoardDTO DTOObj)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where id={DTOObj.BoardId}"
                };
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    DeleteTasksOfBoard(DTOObj.BoardId);
                    DeleteFromUser2Boards(DTOObj.BoardId);
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

        /// <summary>
        /// This method updates a value in Board table.
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
        /// This method updates a value in Board table.
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
        /// This method updates a value in Board table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        /// <exception cref="KanException"></exception>
        public void Update(string name, string attributeName, string attributeValue)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,

                    CommandText = $"update {_tableName} set [{attributeName}]=@attributeValue where name={name}"
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
        /// This method reads a row in the Board table in the database.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public BoardDTO ConvertReaderToBoardDTO(SQLiteDataReader reader)
        {
            string a = reader.GetString(1);
            string b = reader.GetString(2);
            int c = reader.GetInt32(0);

            int e = reader.GetInt32(3);
            int f = reader.GetInt32(4);
            int g = reader.GetInt32(5);
            //string x = ("PRINTING!!!!!! THIS" + reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetInt32(3) + " " + reader.GetInt32(4) + " " + reader.GetInt32(5) + " ");
            //string y =("PRINTING!!!!!! THIS" + reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetInt32(2) + " " +reader.GetInt32(3) + " " + reader.GetInt32(4) + " " + reader.GetInt32(5) + " ");
            return new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
        }

        /// <summary>
        /// This method return the max board's id in the database.
        /// </summary>
        /// <returns></returns>
        public int GetMaxBoardId()
        {
            int maxBoardId = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT MAX({BoardDTO.BoardID}) FROM {_tableName};";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    if (!reader.IsDBNull(0))
                    {
                        maxBoardId = reader.GetInt32(0);
                    }
                }
                reader.Close();
                connection.Close();
            }
            return maxBoardId;
        }

        /// <summary>
        /// This method return column's limit according to board id and the name of the column in the database.
        /// </summary>
        /// <param name="col"></param>
        /// <param name="boardId"></param>
        /// <returns></returns>
        /// <exception cref="KanException"></exception>
        internal int SelectColLimit(string col,int boardId)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select {col} from {_tableName} where ID={boardId} ;";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    res = dataReader.GetInt32(0);
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
            return res;
        }

        /// <summary>
        /// This method delete all the data in Board table.
        /// </summary>
        internal void DeleteTableData()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand("DELETE FROM boards", connection);

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
        /// <param name="bid"></param>
        internal void DeleteTasksOfBoard(int bid)
        {
            _taskController.DeleteTasksOfBoard(bid);
        }

        internal void DeleteFromUser2Boards(int id)
        {
            _u2bController.Delete(id);
        }
    }
}
