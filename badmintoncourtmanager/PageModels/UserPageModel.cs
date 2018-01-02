using System;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Models;
using PropertyChanged;
using Xamarin.Forms;

namespace badmintoncourtmanager.PageModels
{
    [AddINotifyPropertyChangedInterface]
    public class UserPageModel : FreshMvvm.FreshBasePageModel
    {
		//Using iOC to inject.
		IUserManager _userManager;

		public User User { get; set; }
        public string PageTitle
        {
            get;
            set;
        }

		public UserPageModel(IUserManager userManager)
		{
			_userManager = userManager;
		}

		public override void Init(object initData)
		{
			base.Init(initData);

            User = initData as User;
			if (User == null)
			{
                User = new User();
                PageTitle = "Create New Admin User";
            }else{
                PageTitle = "Edit Admin User";
            }

		}

		
		public Command SaveUser
		{
			get
			{
				return new Command(async () =>
				{
                    //await _userManager.CreateUser(User);
                    await _userManager.CreateOrUpdateUser(User);
                    await CoreMethods.DisplayAlert("Button", User.Name, "OK", "Cancel");
                    await CoreMethods.PopPageModel(User);
				});
			}

			set { }
		}

        public Command DeleteUser{
            get{
                return new Command(async() =>
                {
                    var confirmed = await CoreMethods.DisplayAlert("Delete an Admin User", "Are you sure you want to delete this user?", "OK", "Cancel");
                    if (confirmed)
                    {
                        var isDeleted = await _userManager.DeleteUser(User.ID);
                        await CoreMethods.DisplayAlert("Status", isDeleted.ToString(), "OK");
                    }                    
                    await CoreMethods.PopPageModel(User);
                });
            }
        }

    }
}
