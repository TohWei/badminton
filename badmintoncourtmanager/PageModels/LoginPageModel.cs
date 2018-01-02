using System;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Pages;
using FreshMvvm;
using Xamarin.Forms;

namespace badmintoncourtmanager.PageModels
{
    public class LoginPageModel : FreshMvvm.FreshBasePageModel
    {
        public LoginPageModel()
        {
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            RaisePropertyChanged(Username);
            RaisePropertyChanged(Password);
        }

        public Command BtnLoginCommand{
            get{
                return new Command(() =>
                {
                    if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
					{
						CoreMethods.DisplayAlert("Invalid User", "Please provide the right username and password", "OK");
					}
					else
					{
						CoreMethods.DisplayAlert("Login Success", "Welcome", "OK");

						var masterDetailNav = new FreshMasterDetailNavigationContainer();
						masterDetailNav.Init("Dashboard");
                        masterDetailNav.AddPage<PlayerListPageModel>("Players", null);
						masterDetailNav.AddPage<QueueListPageModel>("Queues", null);
						masterDetailNav.AddPage<CourtListPageModel>("Courts", null);
						masterDetailNav.AddPage<UserListPageModel>("Admin Users", null);
						masterDetailNav.AddPage<HelpPageModel>("Help", null);
						Application.Current.MainPage = masterDetailNav;
					}
                });
            }
            set { }
        }


        public bool Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                CoreMethods.DisplayAlert("Invalid User","Please provide the right username and password","OK");
                return false;
            } else{
                CoreMethods.DisplayAlert("Login Success", "Welcome", "OK");

				
				var masterDetailNav = new FreshMasterDetailNavigationContainer();
				masterDetailNav.Init("Dashboard");
                masterDetailNav.AddPage<PlayerListPageModel>("Check-in/out Players", null);
                masterDetailNav.AddPage<QueueListPageModel>("Queues", null);
                masterDetailNav.AddPage<CourtListPageModel>("Courts", null);
                masterDetailNav.AddPage<UserListPageModel>("Admin Users", null);
                masterDetailNav.AddPage<HelpPageModel>("Help", null);
                Application.Current.MainPage = masterDetailNav;
                return true;
            }
        }

		
    }
}
