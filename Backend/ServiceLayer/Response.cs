using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanban.Backend.ServiceLayer;

namespace Kanban.Backend.ServiceLayer
{
    [Serializable]
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }
        public Response() { }

        public Response(Exception e)
        {
            if(e is KanException)
            {
                ErrorMessage = e.Message;
            }
            else
            {
                ErrorMessage = "Unexpected (Untracked) exception... The error is: " + e.Message;   
            }
        }

        public Response(object returnValue) 
        { 
            ReturnValue = returnValue;
        }

        public string ResToString() 
        {
            if (ErrorMessage != null)
            {
                return ErrorMessage;
            }
            return "";
        }

        public bool isErrorOccured() 
        {
            return ErrorMessage != null;
        }
    }
}
