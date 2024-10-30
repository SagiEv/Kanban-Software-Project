using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    internal class TransferOwnershipModel
    {
        private readonly UserModel user;
        private readonly BoardModel board;
        public ObservableCollection<string> BoardMembers { get; set; }

        public TransferOwnershipModel(ObservableCollection<string> BoardMembers)
        {
            this.BoardMembers = BoardMembers;
        }
    }
}
