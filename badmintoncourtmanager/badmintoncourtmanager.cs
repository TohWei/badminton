using System;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.PageModels;
using FreshMvvm;
using Xamarin.Forms;

namespace badmintoncourtmanager
{
    public class App : Application
    {
        public App()
        {
            //Dependency injection using FreshMvvm
            SetupIOC();

            MainPage = FreshMvvm.FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            //MainPage = FreshMvvm.FreshPageModelResolver.ResolvePageModel<UserListPageModel>();
           
        }

        void SetupIOC()
        {
            FreshIOC.Container.Register<IUserManager, UserManager>();
            FreshIOC.Container.Register<IPlayerManager, PlayerManager>();
            FreshIOC.Container.Register<IQueueManager, QueueManager>();
            FreshIOC.Container.Register<ICourtManager, CourtManager>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
