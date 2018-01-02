using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace badmintoncourtmanager.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class UserListPageModel : FreshMvvm.FreshBasePageModel
    {
		//Using iOC to inject.
        //https://gist.github.com/anonymous/a3e5b8130a0eacbc795394502b78a865
        IUserManager _userManager;
		public UserListPageModel(IUserManager userManager)
		{
		   _userManager = userManager;
		}

        //public ObservableCollection<User> UserList { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<User> UserList { get; set; }

		public override async void Init(object initData)
        {
            base.Init(initData);

            UserList = new ObservableCollection<User>();

            var result = await _userManager.GetUserList();

            if (result != null)
            {
                foreach (var item in result.OrderBy(x=>x.Name))
                {
                    UserList.Add(item);
                }
            }

		}


		//This is called when a pushed Page returns to this Page
		public override async void ReverseInit(object returnedData)
		{
            if (returnedData != null)
            {
                var newUserRecord = returnedData as User;

				var result = await _userManager.GetUserList();

				if (result != null)
				{
                    UserList.Clear();

					foreach (var item in result.OrderBy(x => x.Name))
					{
						UserList.Add(item);
					}
				}
            }
		}


        public Command CreateNewUserCommand{
            get {
                return new Command<User>(async(arg) =>
                {
                    await CoreMethods.PushPageModel<UserPageModel>();
                });
            }
        }


        User _selectedUser;
        public User SelectedItem{
            get{
                return _selectedUser;
            }
            set{
                _selectedUser = value;
                if (value != null){
                    CoreMethods.PushPageModel<UserPageModel>(value);
                }
            }
        }
    }


}
