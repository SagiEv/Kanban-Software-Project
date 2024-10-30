using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Backend.ServiceLayer
{
    public class KanException: Exception
    {
        public KanException(string message) : base(message) { }
    }
}
