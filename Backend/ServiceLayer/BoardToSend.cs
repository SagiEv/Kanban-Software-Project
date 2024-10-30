using Kanban.Backend.Business_Layer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardToSend
    {

        public int id { get; set; }
        public string name { get; set; }
        public string owner { get; set; }

        internal BoardToSend(Board b)
        {
            id = b.GetId();
            name = b.GetName();
            owner = b.GetOwner();
        }

        public BoardToSend()
        {
            // Parameterless constructor
        }
    }
}
