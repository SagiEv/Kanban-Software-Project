using BackendTests;
using IntroSE.Kanban.Backend.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BackendTests;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.Backend.ServiceLayer;
using Kanban.BackendTests;
using log4net;
using log4net.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


//[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace IntroSE.Kanban.BackendTests
{
    //THIS CLASS IS FOR OUR OWN TESTS
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
        
    //string currentPath = Directory.GetCurrentDirectory();
    //string parentPath = Directory.GetParent(currentPath)?.Parent?.Parent?.Parent?.FullName;
    //string configPath = Path.Combine(parentPath + "\\Backend", "log4net.config");
    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            /////////////////////this section was used to create the database\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            //string databaseFilePath = "C:\\Users\\karin\\Desktop\\Kanban project\\Backend\\database.db";
            //string connectionString = $"Data Source={databaseFilePath};Version=3;";

            //using (var connection = new SQLiteConnection(connectionString))
            //{
            //    connection.Open();

            //    // Database connection is now open

            //    connection.Close();
            //}
            //using (var connection = new SQLiteConnection(connectionString))
            //{
            //    connection.Open();

            //    // Execute SQL commands to create tables or perform other operations
            //    string createBoards = "CREATE TABLE \"Boards\" (\r\n\t\"ID\"\tINTEGER,\r\n\t\"Name\"\tTEXT,\r\n\t\"Owner\"\tTEXT,\r\n\t\"backlog_lim\"\tINTEGER,\r\n\t\"in_progress_lim\"\tINTEGER,\r\n\t\"done_lim\"\tINTEGER,\r\n\tPRIMARY KEY(\"ID\")\r\n);";
            //    string createTables = "CREATE TABLE \"Tasks\" (\r\n\t\"ID\"\tINTEGER,\r\n\t\"BoardID\"\tINTEGER,\r\n\t\"CreationTime\"\tTEXT,\r\n\t\"Title\"\tTEXT,\r\n\t\"Description\"\tTEXT,\r\n\t\"DueDate\"\tTEXT,\r\n\t\"Assignee\"\tTEXT,\r\n\t\"Column\"\tINTEGER,\r\n\tPRIMARY KEY(\"ID\")\r\n);";
            //    string createUsers = "CREATE TABLE \"Users\" (\r\n\t\"email\"\tTEXT,\r\n\t\"password\"\tTEXT,\r\n\t\"isLoggedIn\"\tTEXT,\r\n\tPRIMARY KEY(\"email\")\r\n);";
            //    string createUsers2Boards = "CREATE TABLE \"Users2Boards\" (\r\n\t\"BoardID\"\tINTEGER,\r\n\t\"Email\"\tINTEGER,\r\n\tPRIMARY KEY(\"BoardID\",\"Email\")\r\n);";
            //    using (var command = new SQLiteCommand(createUsers, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    using (var command = new SQLiteCommand(createBoards, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    using (var command = new SQLiteCommand(createTables, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }
            //    using (var command = new SQLiteCommand(createUsers2Boards, connection))
            //    {
            //        command.ExecuteNonQuery();
            //    }

            //    connection.Close();
            //}
            Console.WriteLine("this is our main to check the DAL layer:");
            DataAccessLayerTests dal = new DataAccessLayerTests();

            Console.WriteLine("this is our main to check the service layer:");
            BoardServiceTests b = new BoardServiceTests();
            UserServiceTests u = new UserServiceTests();
            TaskServiceTests t = new TaskServiceTests();

            SystemService ss = new SystemService();
            ss.DeleteData();
            ss.Register("mail@mail.com", "Password1");
            ss.CreateBoard("mail@mail.com", "board1");
            DateTime d = new DateTime(2025, 1, 1);
            DateTime d1 = new DateTime(2030, 1, 1);
            ss.AddTask("mail@mail.com", "board1", "UI DESIGN", "fix some features", d);
            ss.AssignTask("mail@mail.com", "board1", 0, 0, "mail@mail.com");
            ss.AddTask("mail@mail.com", "board1", "UI DESIGN", "add some features", d1);
            ss.AssignTask("mail@mail.com", "board1", 0, 1, "mail@mail.com");
            ss.AdvanceTask("mail@mail.com", "board1", 0, 1);
            ss.AddTask("mail@mail.com", "board1", "Hopefully", "Getting 100 in this mileston 3", d1);
            ss.AssignTask("mail@mail.com", "board1", 0, 2, "mail@mail.com");
            ss.AdvanceTask("mail@mail.com", "board1", 0, 2);
            ss.AdvanceTask("mail@mail.com", "board1", 1, 2);
            ss.CreateBoard("mail@mail.com", "board2");
            ss.Logout("mail@mail.com");


        }
    }
}
