using IntroSE.Kanban.Backend.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kanban.BackendTests
{
    internal class UserServiceTests
    {
        //fields
        private readonly SystemService ss = new SystemService();
        //private readonly BoardService boardService = new BoardService();

        /// <summary>
        /// This constructor calls all the tests in the correct order
        /// </summary>
        public UserServiceTests()
        {
            //ss.DeleteData();
            Console.WriteLine("User Service Tests:");
            Console.WriteLine("------------------");
            RegistrationTests();
            LoginTests();
            LogoutTests();
            Console.WriteLine("------------------");
            //ss.DeleteData();
        }


        /// <summary>
        /// This function tests Register requirements: 2,3
        /// </summary>
        private void RegistrationTests()
        {
            Console.WriteLine("Registration Tests:");
            //requirement 2 tests:
            string output = ss.Register("orelof@gmail.com", null); //illegal password (null password)
            Response res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            string expected = "Password is null";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "o123M"); //illegal password (less than 6 characters)
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "Password length is not valid";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "o123M123123123123123123123"); //illegal password (more than 20 characters)
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "orel123"); //illegal password (without uppercase character) 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "a valid password must contain at least 1 of the following: uppercase letter, lowercase letter, digit";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "OREL123"); //illegal password (without lowercase character)
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "ORELorel"); //illegal password (without number)
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmail.com", "OrelO123"); //legal password
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{false} output:{res.isErrorOccured()}");
            Response expect = new Response(); // in a case of legal password, an empty response is created
            Console.WriteLine($"Expected: {expect.ResToString()}");
            Console.WriteLine();
            
            output = ss.Register("sagi@gmail.com", "SagilO123"); //legal password
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{false} output:{res.isErrorOccured()}");
            Console.WriteLine($"Expected: {expect.ResToString()}");
            Console.WriteLine();


            //currently only 2 users supposed to be registered & logged-in
            Console.WriteLine("Logout - sagi@gmail.com");
            output =ss.Logout("sagi@gmail.com");
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"Expected: {expect.ResToString()}");
            Console.WriteLine();
            //currently only 2 users supposed to be registered & only 1 is logged-in

            //rquirement 3 tests:
            output = ss.Register("orelof@gmail.com", "123Orel"); //email already exist
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "e-mail address: orelof@gmail.com already exists in the system";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            //Console.WriteLine(res.isErrorOccured());

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            output = ss.Register("orelof", "123Orel"); //illegal email
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@.com", "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof.com", "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("orelof@gmailcom", "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("@gmailcom", "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register(null, "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "Email is null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Register("", "123Orel"); //illegal email 
            res = JsonSerializer.Deserialize<Response>(output);
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine(res.isErrorOccured());
            Console.WriteLine();
        }

        /// <summary>
        /// This function tests Register requirement 7, Login requirements: 1,8
        /// </summary>
        private void LoginTests()
        {
            Console.WriteLine("Login Tests:");
            //Register requirement 7 - trying to login after registeration
            string output = ss.Login("orelof@gmail.com", "OrelO123"); //LoginTests() is procedured after RegisterTests() so this user already logged-in
            Response res = JsonSerializer.Deserialize<Response>(output);
            string expected = "this user is already logged in!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            //need to logout
            ss.Logout("orelof@gmail.com");
            //requirement 1 tests:
            output = ss.Login("orelof@gmail.com", null); //login with null password
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "password is null";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Login(null, "jfk3iO"); //login with null email
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "Email can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Login("orelof@gmail.com", "jfk3iO"); //login with wrong password
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "incorrect password, please try again!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Login("orelnew@gmail.com", "jfk3iO"); //login with unregistered user
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "e-mail address: orelnew@gmail.com was not found in the system!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Login("orelof@.com", "jfk3iO"); //login with invalid email
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            //requirement 8 tests:
            output = ss.Login("sagi@gmail.com", "SagilO123"); //login successful
            res = JsonSerializer.Deserialize<Response>(output);
            Response expect = new Response();
            Console.WriteLine($"isErrorOccured? expected:{true} output:{res.isErrorOccured()}");
            Console.WriteLine($"Expected: {expect.ResToString()}");
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();
        }

        /// <summary>
        /// This function tests Logout requirement: 8
        /// </summary>
        private void LogoutTests()
        {
            Console.WriteLine("Logout Tests:");

            string output = ss.Logout("orelof@gmail.com"); //logout successful
            Response res = JsonSerializer.Deserialize<Response>(output);
            Response expect = new Response();
            Console.WriteLine("Expected: " + expect.ResToString());
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Logout("orelof@gmail.com"); //logout before login
            res = JsonSerializer.Deserialize<Response>(output);
            string expected = "user orelof@gmail.com is already logged out";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Logout(null); //logout with null username
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "Email can't be null!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Logout("orelnew@.com"); //logout with invalid username
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "Email is invalid!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();

            output = ss.Logout("orelii@gmail.com"); //logout with unregistered user
            res = JsonSerializer.Deserialize<Response>(output);
            expected = "e-mail address: orelii@gmail.com was not found in the system!";
            Console.WriteLine("Expected: " + expected);
            Console.WriteLine("Output: " + res.ResToString());
            Console.WriteLine();
        }
    }
}
