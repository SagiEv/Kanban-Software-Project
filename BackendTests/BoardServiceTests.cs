using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Kanban.Backend.Business_Layer;
using IntroSE.Kanban.Backend.Backend.ServiceLayer;


namespace IntroSE.Kanban.Backend.ServiceLayer
{
    //NOTE: the BackendTests check our OWN classes - NOT the GradingClass - as was written in the forum!
    internal class BoardServiceTests
    {
        //fields
        private readonly SystemService ss = new SystemService();

        //constructor
        /// <summary>
        /// This constructor calls all the tests in the correct order
        /// </summary>
        public BoardServiceTests()
        {
            //ss.DeleteData();
            Console.WriteLine("Board Service Tests:");
            Console.WriteLine("-------------------");
            initTest();
            checkCreation();
            checkDelete();
            checkInProgress();
            testColumnOperations();
            checkJoinBoard();
            checkLeaveBoard();
            checkTransferOwnership();
            checkGetBoardName();
            checkGetUserBoards();
            Console.WriteLine("------------------");
            //ss.DeleteData();
        }

        /// <summary>
        /// This function creates users in order to check BoardService class (we can't test it without users)
        /// </summary>
        private void initTest()
        {
            Console.WriteLine("Initiation of Board Service Tests");
            Console.WriteLine("REGISTER & LOGIN - sagi@gmail.com");
            ss.Register("sagi@gmail.com", "abcdefAghi6");
            Console.WriteLine("REGISTER & LOGIN - orel@gmail.com");
            ss.Register("orel@gmail.com", "121212a121A");
            Console.WriteLine("REGISTER & LOGIN - ofri@gmail.com");
            ss.Register("ofri@gmail.com", "Z12345c678");
            Console.WriteLine("LOGOUT - ofri@gmail.com");
            ss.Logout("ofri@gmail.com");
            Console.WriteLine("REGISTER & LOGIN - meni@gmail.com");
            ss.Register("meni@gmail.com", "ghi6AAA");

            Console.WriteLine();
        }

        /// <summary>
        /// This function tests requirement 6,9
        /// </summary>
        private void checkCreation()

        {
            Console.WriteLine("BOARD CREATION CHECK:");

            //Summary of users boards
            //sagi: board2(1)
            //orel: b2(3), board1(2)
            //ofri:

            //requirement 6 tests:
            string JSONoutput = ss.CreateBoard("sagi@gmail.com", "board1");//this is acceptable email and name
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = null;
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            //CreateBoard function is void, the test should pass with no exceptions, therefore should be return empty string
            string expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            JSONoutput = ss.CreateBoard("sagi@gmail.com", "board1");//board already exist!
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Board name already exists!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            //requirement 9 tests:
            JSONoutput = ss.CreateBoard("", "board1");//this is not acceptable email
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard(null, "board1");//this is not acceptable email
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Email can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("sagi@gmail.com", "");//this is not acceptable board name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Board name can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("sagi1@gmail.com", "board1");//this user doesn't exist
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: sagi1@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("sagi@gmail.com", "board2");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("orel@gmail.com", "board1");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("orel@gmail.com", "b2");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.CreateBoard("ofri@gmail.com", "boardyboard");//ofri is not logged in!
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "user with the email: ofri@gmail.com is not logged-in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();
        }


        /// <summary>
        /// This function tests requirement 9
        /// </summary>
        private void checkDelete()
        {
            ss.AddTask("sagi@gmail.com", "board1", "taskToDelete", DateTime.Now);
            //ONLY ASSIGNEE CAN ADVANCE TASKS
            ss.AssignTask("sagi@gmail.com", "board1", 0, 0, "sagi@gmail.com");
            ss.AdvanceTask("sagi@gmail.com", "board1", 0, 0);
            Console.WriteLine("BOARD DELETE CHECK:");
            string output = null;
            string JSONoutput = ss.DeleteBoard("", "board1");//this is not acceptable email
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard(null, "board1");//this is not acceptable email
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Email can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard("sagi@gmail.com", "");//this is not acceptable board name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            expected = "board '' does not exist in the user sagi@gmail.com!";
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard("sagi1@gmail.com", "board1");//this user doesn't exist
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: sagi1@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            /////
            ss.AssignTask("sagi.gmail.com", "board1", 0, 0, "sagi.gmail.com");
            ////

            JSONoutput = ss.InProgressTasks("sagi@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ReturnValue.ToString();
            //"1 in progress in board1---taskToDelete,Task 0";
            expected = "/[{\"Id\":0,\"CreationTime\":\"2023-06-09T12:55:18.0227349+03:00\",\"Title\":\"taskToDelete\",\"Description\":null,\"DueDate\":\"2023-06-09T12:55:18.0227118+03:00\"}]";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard("sagi@gmail.com", "board1");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            //check if the task has been delete

            JSONoutput = ss.InProgressTasks("sagi@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else { output = RESPONSEoutput.ReturnValue.ToString(); }
            //"0 in progress in board2";
            expected = "[]";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            ////////////////////////////////////////////////////
            JSONoutput = ss.DeleteBoard("sagi@gmail.com", "board1");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'board1' does not exist in the user sagi@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard("sagi@gmail.com", "b2");//this is acceptable email and name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'b2' does not exist in the user sagi@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.DeleteBoard("ofri@gmail.com", "boardyboard");//ofri is not logged in!
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "user with the email: ofri@gmail.com is not logged-in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();
        }

        /// <summary>
        /// This function tests requirement 22
        /// </summary>
        private void checkInProgress()
        {
            Console.WriteLine("IN PROGRESS CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.InProgressTasks("yaki@gmail.com");//this is not acceptable email
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "e-mail address: yaki@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.InProgressTasks(null);//this is not acceptable email
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Email can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.InProgressTasks("sagi@gmail.com");//this acceptable board name
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ReturnValue.ToString();
            expected = "[]";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            //if(expected.Equals(output))
            //{
            //    Console.WriteLine("Expected List = { }");
            //    Console.Write("Output List = ");
            //    List<Task> showinp = (List<Task>)RESPONSEoutput.ReturnValue;
            //    printList(showinp);
            //}


            JSONoutput = ss.InProgressTasks("orel@gmail.com");//this acceptable email
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ReturnValue.ToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            JSONoutput = ss.InProgressTasks("ofri@gmail.com");//ofri is not logged in!
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "user with the email: ofri@gmail.com is not logged-in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            Response y = JsonSerializer.Deserialize<Response>(ss.Register("AAa135AA@a.x", "AAa135AA"));
            //Console.WriteLine(y.ResToString());
            //ss.Logout("aaa135aa@a.x");
            ss.CreateBoard("AAa135AA@a.x", "kaki");
            Console.WriteLine("Created board kaki");
            DateTime dt = DateTime.Now;
            ss.AddTask("AAa135AA@a.x", "kaki", "eat", "sleep", dt);
            Console.WriteLine("Added TaskId0 to board kaki");
            Response ip = JsonSerializer.Deserialize<Response>(ss.InProgressTasks("AAa135AA@a.x"));
            //Console.WriteLine(ip.ReturnValue);
            Console.WriteLine("InProgress EXPECTED: ");
            Console.WriteLine("InProgress OUTPUT: " + ip.ReturnValue.ToString());
            ss.AssignTask("AAa135AA@a.x", "kaki", 0, 1, "AAa135AA@a.x");
            ss.AdvanceTask("AAa135AA@a.x", "kaki", 0, 1);
            Console.WriteLine("Advanced TaskId1 to from 0 to 1");
            ip = JsonSerializer.Deserialize<Response>(ss.InProgressTasks("AAa135AA@a.x"));
            //Console.WriteLine(ip.ResToString());
            Console.WriteLine($"InProgress EXPECTED: [{{\"Id\":0,\"CreationTime\":\"{DateTime.Now}\",\"Title\":\"eat\",\"Description\":\"sleep\",\"DueDate\":\"2023-05-11T21:54:21.626248+03:00\"}}]");
            Console.WriteLine("InProgress OUTPUT: " + ip.ReturnValue);
            //ss.AssignTask("AAa135AA@a.x", "kaki", 1, 1, "AAa135AA@a.x");
            ss.AdvanceTask("AAa135AA@a.x", "kaki", 1, 1);
            Console.WriteLine("Advanced TaskId1 to from 1 to 2");
            ip = JsonSerializer.Deserialize<Response>(ss.InProgressTasks("AAa135AA@a.x"));
            //Console.WriteLine(ip.ResToString());
            Console.WriteLine("InProgress EXPECTED: []");
            Console.WriteLine("InProgress OUTPUT: " + ip.ReturnValue);
            Console.WriteLine();
        }

        //private void printList(List<Task> inProgress)
        //{
        //    Console.Write("{");
        //    foreach (var task in inProgress)
        //    {
        //        Console.Write(" "+task.ToString());
        //    }
        //    Console.WriteLine("}");
        //}

        /// <summary>
        /// This function tests requirement 16,17
        /// </summary>
        private void testColumnOperations()
        {
            Console.WriteLine("COLUMNS CHECK:");
            Console.WriteLine();
            //string output = null;
            string JSONoutput = ss.GetColumnLimit("sagi@gmail.com", "board2", 0);//this supposed to return -1
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            string output = RESPONSEoutput.ResToString();
            string expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.LimitColumn("sagi@gmail.com", "board2", 0, 10);//this supposed to return -1
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);

            JSONoutput = ss.GetColumnLimit("sagi@gmail.com", "board2", 0);//this supposed to return -1
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            object lim = RESPONSEoutput.ReturnValue;
            expected = "10";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + lim);

            //sagi has board2
            DateTime date = new DateTime(2024, 4, 17, 5, 5, 5);
            JSONoutput = ss.AddTask("sagi@gmail.com", "board2", "Practice", "Run 5 km + 50 pushups", date);
            ss.AssignTask("sagi@gmail.com", "board2", 0, 1, "sagi@gmail.com");
            ss.AdvanceTask("sagi@gmail.com", "board2", 0, 1);
            JSONoutput = ss.GetColumn("sagi@gmail.com", "board2", 1);//this supposed to return {task 1}
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            JSONoutput = RESPONSEoutput.ResToString();
            object outputObj = RESPONSEoutput.ReturnValue;
            string expectedObj1 = $"[{{\"Id\":0,\"CreationTime\":\"{DateTime.Now}\",\"Title\":\"eat\",\"Description\":\"sleep\",\"DueDate\":\"2023-05-11T21:54:21.626248+03:00\"}}]";
            Console.WriteLine("Expected: " + expectedObj1);
            Console.WriteLine("Output: " + outputObj);
            Console.WriteLine();

            //orel has board1
        }

        /// <summary>
        /// This function tests requirement 12
        /// </summary>
        private void checkJoinBoard()
        {
            //Summary of users boards
            //sagi: board2(1)
            //orel: b2(3), board1(2)
            //ofri:

            Console.WriteLine("JOIN BOARD CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.JoinBoard("meni@gmail.com", 14); //The board does not exist to join
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "board id is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.JoinBoard("meniiii@gmail.com", 14); //user does not exist
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: meniiii@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();


            JSONoutput = ss.JoinBoard("meni@gmail.com", 1); //good
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();
            //meni: board2(1)

            JSONoutput = ss.JoinBoard("meni@gmail.com", 1); //The user has already joined this board
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "meni@gmail.com already a member!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

        }

        /// <summary>
        /// This function tests requirement 12,14,15
        /// </summary>
        private void checkLeaveBoard()
        {
            //Summary of users boards
            //sagi: board2(1)
            //orel: b2(3), board1(2)
            //ofri:
            //meni: board2(1)join

            Console.WriteLine("LEAVE BOARD CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.LeaveBoard("sagiiii@gmail.com", 14); //user does not exist
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = " e-mail address: sagiiii@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.LeaveBoard("orel@gmail.com", 1); //user has not joined this board
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "orel@gmail.com is not a member in this board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.LeaveBoard("sagi@gmail.com", 1); //the owner cant leave the board he created
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Owner cannot leave his board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.LeaveBoard("meni@gmail.com", 1); //good
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.LeaveBoard("meni@gmail.com", 1); //user has not joined this board
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "meni@gmail.com is not a member in this board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            ss.JoinBoard("meni@gmail.com", 1);
            ss.TransferOwnership("sagi@gmail.com", "meni@gmail.com", "board2");

            JSONoutput = ss.LeaveBoard("meni@gmail.com", 1);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Owner cannot leave his board!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.LeaveBoard("sagi@gmail.com", 1); //good
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            ss.JoinBoard("sagi@gmail.com", 1);
            ss.TransferOwnership("meni@gmail.com", "sagi@gmail.com", "board2");

        }
        /// <summary>
        /// This function tests requirement 13
        /// </summary>
        private void checkTransferOwnership()
        {

            ss.JoinBoard("meni@gmail.com", 1);
            //meni: board2(1)

            //Summary of users boards
            //sagi: board2(1)
            //orel: b2(3), board1(2)
            //ofri:
            //meni: board2(1)join
            Console.WriteLine("TRANSFER OWNERSHIP CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.TransferOwnership("sagi@gmail.com", "meniii@gmail.com", "board2");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "the new owner user does not exist";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.TransferOwnership("sagi@gmail.com", "orel@gmail.com", "board2");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board 'board2' does not exist in the user orel@gmail.com!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.TransferOwnership("sagi@gmail.com", "meni@gmail.com", "board2");//good
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            
            //Summary of users boards
            //orel: b2(3), board1(2)
            //ofri:
            //meni: board2(1)
        }
        /// <summary>
        /// This method tests BoardService's GetBoardName method 
        /// </summary>
        private void checkGetBoardName()
        {

            Console.WriteLine("GET BOARD NAME CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.GetBoardName(18);
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = " board id is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.GetBoardName(1);
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "board2";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.GetBoardName(0);//this board was deleted
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "Invalid board id";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();
        }

        /// <summary>
        /// This method tests BoardService's GetUserBoards method 
        /// </summary>
        private void checkGetUserBoards()
        {

            Console.WriteLine("GET USER BOARDS CHECK:");
            Console.WriteLine();
            string output = null;
            string JSONoutput = ss.GetUserBoards("orel@gmail.com");
            Response RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            string expected = "board1, b2";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.GetUserBoards("meniii@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "e-mail address: meniii@gmail.com does not exist in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            JSONoutput = ss.GetUserBoards("ofri@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "user with the email: ofri@gmail.com is not logged-in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();

            ss.Login("ofri@gmail.com", "Z12345c678");

            JSONoutput = ss.GetUserBoards("ofri@gmail.com");
            RESPONSEoutput = JsonSerializer.Deserialize<Response>(JSONoutput);
            if (RESPONSEoutput.isErrorOccured()) { output = RESPONSEoutput.ErrorMessage; }
            else output = RESPONSEoutput.ResToString();
            expected = "";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + output);
            Console.WriteLine();
        }
    }
        }
