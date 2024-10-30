using IntroSE.Kanban.Backend.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BackendTests
{
    internal class DataAccessLayerTests
    {
        private readonly SystemService ss = new SystemService();
        private readonly SystemService ss1 = new SystemService();

        public DataAccessLayerTests()
        {
            initiateSomeData();
            tryLoadData();
        }

        public void initiateSomeData()
        {
            ss.Register("sagi@gmail.com", "abcdefAghi6");
            ss.Register("ofri@gmail.com", "abcdefAghi6");
            ss.CreateBoard("sagi@gmail.com", "Data Layer Test");
            ss.AddTask("sagi@gmail.com", "Data Layer Test", "task1", DateTime.Now);
            ss.AddTask("sagi@gmail.com", "Data Layer Test", "task2", DateTime.Now);
            ss.JoinBoard("ofri@gmail.com",0);
            ss.AssignTask("sagi@gmail.com", "Data Layer Test", 0, 0, "ofri@gmail.com");
            ss.Logout("sagi@gmail.com");
            ss.Logout("ofri@gmail.com");
        }

        public void tryLoadData()
        {
            ss1.LoadData();
            ss1.Login("sagi@gmail.com", "abcdefAghi6");
            ss1.Login("ofri@gmail.com", "abcdefAghi6");
            ss1.CreateBoard("sagi@gmail.com", "Data Layer Test");
            ss1.AddTask("sagi@gmail.com", "Data Layer Test", "task1", DateTime.Now);

        }
    }
}
