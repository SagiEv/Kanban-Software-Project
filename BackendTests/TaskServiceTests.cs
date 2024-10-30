using IntroSE.Kanban.Backend.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.Backend.ServiceLayer;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{
    internal class TaskServiceTests
    {
        // fields
        private SystemService ss = new SystemService();

        // constructor
        /// <summary>
        /// This constructor calls all the tests in the correct order
        /// </summary>
        public TaskServiceTests()
        {
            //ss.DeleteData();
            Console.WriteLine("Task Service Tests:");
            Console.WriteLine("-------------------");
            initTestObjects();
            AddTaskTest();
            UpdateTaskDueDate();
            UpdateTaskDescription();
            UpdateTaskTitle();
            testChangesTaskDone();
            AssignTaskTest();
            Console.WriteLine("------------------");
            //ss.DeleteData();
        }

        /// <summary>
        /// This function creates users and boards in order to check TaskService class (we can't test it without users and boards)
        /// </summary>
        private void initTestObjects()
        {
            Console.WriteLine("Initiation of Task Service Test");
            Console.WriteLine("REGISTER & LOGIN - A@gmail.com");
            ss.Register("A@gmail.com", "Aaa123");
            Console.WriteLine("REGISTER & LOGIN - C@gmail.com");
            ss.Register("C@gmail.com", "Ccc123");
            Console.WriteLine("REGISTER & LOGIN - D@gmail.com");
            ss.Register("D@gmail.com", "dd123D");
            Console.WriteLine("REGISTER & LOGIN - H@gmail.com");
            ss.Register("H@gmail.com", "hh123H");
     
            Console.WriteLine("Creating a Board for each user");
            ss.CreateBoard("A@gmail.com", "A_Board");
            ss.CreateBoard("C@gmail.com", "C_Board");
            ss.CreateBoard("D@gmail.com", "D_Board");
            ss.CreateBoard("H@gmail.com", "H_Board");
            Console.WriteLine("User D@gmail.com logged out");
            ss.Logout("D@gmail.com");
            Console.WriteLine();

        }

        /// <summary>
        /// This function tests requirement 4
        /// </summary>
        private void AddTaskTest()
        {
            Console.WriteLine("AddTask Tests:");
            DateTime date = new DateTime(2024, 4, 17,5,5,5);
            string JSONoutput = ss.AddTask("A@gmail.com", "A_Board", "Practice", "Run 5 km + 50 pushups", date);
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "";
            //string expected = "Task is successfully added! task no. #uniqueId";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            JSONoutput = ss.AddTask("B@gmail.com", "A_Board", "Practice", "Run 5 km + 50 pushups", date); // no such user exists
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: B@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
          
            JSONoutput = ss.AddTask("A@gmail.com", "C_Board", "Practice", "Run 5 km + 50 pushups", date); // this board doesnt belong to the user
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'C_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.AddTask("A@gmail.com", "B_Board", "Practice", "Run 5 km + 50 pushups", date); // no such board exists

            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'B_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
        
            JSONoutput = ss.AddTask("A@gmail.com", "A_Board", null, "Run 5 km + 50 pushups", date); // null value as a Title
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid new title";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.AddTask("A@gmail.com", "A_Board", "", "Run 5 km + 50 pushups", date); // empty Title is illegal
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid new title";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.AddTask("A@gmail.com", "A_Board", "111111111111111111111111111111111111111111111111111", "Run 5 km + 50 pushups", date); // Title is longer than 50 characters
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid new title";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.AddTask("A@gmail.com", "A_Board", "Practice", null, date); // null value as a Description
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Description cannot have a null value";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.AddTask("D@gmail.com", "D_Board", "Practice", "Run 5 km + 50 pushups", date); // user is logged out
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "user with the email: D@gmail.com is not logged-in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            Console.WriteLine();
        }

        /// <summary>
        /// This function tests requirement 15
        /// </summary>
        private void testChangesTaskDone()
        {
            string output = null;
            string email= "ofri@walla.com";
            string pass= "123AAAb";
            ss.Register(email, pass);
            ss.CreateBoard(email, "bb");
            DateTime date = new DateTime(2024, 4, 17, 5, 5, 5);
            ss.AddTask("ofri@walla.com", "bb", "title", date);
            //shifting to InProgress Column
            ss.AdvanceTask(email,"bb",0,0);
            //shifting to Done column
            ss.AdvanceTask(email, "bb", 1, 0);
            Console.WriteLine("UpdateTaskDescription Tests:");

            string JSONoutput = ss.UpdateTaskDescription(email, "bb", 2, 0, "new_Description");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "A task that is done cannot be changed!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

        }

        /// <summary>
        /// This function tests requirement 16
        /// </summary>
        private void UpdateTaskDueDate()
        {
            Console.WriteLine("UpdateTaskDueDate Tests:");
            DateTime date = new DateTime(2025, 4, 17, 5, 5, 5);
            string JSONoutput = ss.UpdateTaskDueDate("A@gmail.com", "A_Board", 0, 0, date);
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            DateTime passedDate = new DateTime(2000, 4, 17, 5, 5, 5);
            JSONoutput = ss.UpdateTaskDueDate("A@gmail.com", "A_Board", 0, 0, passedDate);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid date time";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDueDate("A@gmail.com", "A_Board", 3, 0, date);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "invalid column! 3 is not in range!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDueDate("A@gmail.com", "A_Board", 0, 2, date);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "the task does not exist";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDueDate("A@gmail.com", "B_Board", 0, 0, date);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'B_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDueDate("B@gmail.com", "A_Board", 0, 0, date);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: B@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            Console.WriteLine();
        }
            /// <summary>
            /// This method tests TaskService's UpdateTaskDescription method 
            /// </summary>
            private void UpdateTaskDescription()
        {
            Console.WriteLine("UpdateTaskDescription Tests:");
            string JSONoutput = ss.UpdateTaskDescription("A@gmail.com", "A_Board", 0, 0, "new_Description");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDescription("A@gmail.com", "A_Board", 0, 0, "11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Description over 300 characters";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDescription("A@gmail.com", "B_Board", 0, 0, "new_Description");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'B_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDescription("B@gmail.com", "A_Board", 0, 0, "new_Description");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: B@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDescription("A@gmail.com", "A_Board", 3, 0, "new_Description");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "invalid column! 3 is not in range!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskDescription("A@gmail.com", "A_Board", 0, 3, "new_Description");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "the task does not exist";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            Console.WriteLine();
        }

        /// <summary>
        /// This function tests requirement 16
        /// </summary>
        private void UpdateTaskTitle()
        {
            Console.WriteLine("UpdateTaskTitle Tests:");
            string JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "A_Board", 0, 0, "new_Title");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected= "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "A_Board", 0, 0, "");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid new title";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "A_Board", 0, 0, "111111111111111111111111111111111111111111111111111");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid new title";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "B_Board", 0, 0, "new_Title");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'B_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("B@gmail.com", "A_Board", 0, 0, "new_Title");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: B@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "B_Board", 0, 0, "new_Title");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'B_Board' does not exist in the user A@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "A_Board", 3, 0, "new_Title");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "invalid column! 3 is not in range!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.UpdateTaskTitle("A@gmail.com", "A_Board", 0, 3, "new_Title");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "the task does not exist";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            Console.WriteLine();
        }
        /// <summary>
        /// This function tests requirement 23
        /// </summary>
        private void AssignTaskTest()
        {
            //AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
            Console.WriteLine("AssignTaskTest Tests:");
            Console.WriteLine("-- Creating new Users and Boards with default data to avoid mistakes --");
            ss.Register("X@gmail.com", "Xxx123");
            ss.Register("Y@gmail.com", "Yyy123");
            ss.Register("Z@gmail.com", "Zzz123");

            ss.CreateBoard("X@gmail.com", "X_Board");
            Console.WriteLine("*********************************************************");
            //Console.WriteLine(ss.getBoardIdCounter());
            Console.WriteLine("*********************************************************");

            ss.JoinBoard("Y@gmail.com", 5);

            Console.WriteLine("-- Adding a new task to X_Board --");
            DateTime date = new DateTime(2024, 4, 17, 5, 5, 5);
            ss.AddTask("X@gmail.com", "X_Board", "shoping", "milk+eggs", date);
            
            Console.WriteLine("this test supposed to work:");
            string JSONoutput = ss.AssignTask("X@gmail.com", "X_Board", 0, 2, "Y@gmail.com");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            //email doesnt exists
            JSONoutput = ss.AssignTask("QQQ@gmail.com", "X_Board", 0, 2, "Y@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: QQQ@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            //board doesnt belong to the user
            JSONoutput = ss.AssignTask("X@gmail.com", "A_Board", 0, 0, "Y@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board A_Board does not exist in the user X@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            //email assignee doesnt exists
            JSONoutput = ss.AssignTask("X@gmail.com", "X_Board", 0, 2, "YYY@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "YYY@gmail.com is not a member in this board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            //task doesnt exists
            JSONoutput = ss.AssignTask("X@gmail.com", "X_Board", 0, 17, "Y@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "the task does not exist";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            //assignee is not registered to the board
            JSONoutput = ss.AssignTask("X@gmail.com", "X_Board", 0, 2, "Z@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Z@gmail.com is not a member in this board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            ss.LeaveBoard("Y@gmail.com",5);
            ss.JoinBoard("Z@gmail.com", 5);
            JSONoutput = ss.AssignTask("Z@gmail.com", "X_Board", 0, 2, "X@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();



        }
    }
}

