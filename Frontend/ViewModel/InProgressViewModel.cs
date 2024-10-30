using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    internal class InProgressViewModel : NotifiableObject
    {
        private UserModel _user;
        public UserModel User
        {
            get => _user; set => _user = value;
        }

        public InProgressViewModel(UserModel user)
        {
            this.User = user;
        }

        private string _selectedTask;
        public string SelectedTask
        {
            get => _selectedTask;
            set { 
                SelectedTask = value;
                RaisePropertyChanged("Selected Task");
            }

        }


        public InProgressModel _inProgressModel { get; private set; }
        
    }
}
