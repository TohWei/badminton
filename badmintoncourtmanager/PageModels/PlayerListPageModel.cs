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
    public class PlayerListPageModel : FreshMvvm.FreshBasePageModel
    {
        IPlayerManager _playerManager;
        IQueueManager _queueManager;

        public PlayerListPageModel(IPlayerManager playerManager,  IQueueManager queueManager)
        {
            _playerManager = playerManager;
            _queueManager = queueManager;
        }

        public ObservableCollection<Player> PlayerList
        {
            get;set;
        }

        public override async void Init(object initData)
        {
            base.Init(initData);

			PlayerList = new ObservableCollection<Player>();

            var result = await _playerManager.GetPlayerList();

			if (result != null)
			{
                foreach (var item in result.OrderBy(x => x.LastName))
				{
					PlayerList.Add(item);
				}
			}
        }

		//This is called when a pushed Page returns to this Page
        /*** Temp removed. Replaced by the isAppearing...
		public override async void ReverseInit(object returnedData)
		{
			if (returnedData != null)
			{
				var newUserRecord = returnedData as Player;

                var result = await _playerManager.GetPlayerList();

				if (result != null)
				{
                    PlayerList.Clear();

                    foreach (var item in result.OrderBy(x => x.FirstName))
					{
						PlayerList.Add(item);
					}
				}

			}
		}
		*/


		public Command CreateNewPlayerCommand
		{
			get
			{
				return new Command<Player>(async (arg) =>
				{
                    await CoreMethods.PushPageModel<PlayerPageModel>();
				});
			}
		}


        public Command TapCheckOutPlayer
		{
            get {
                return new Command(async(arg) =>
                {
                    var selectedPlayer = arg as Player;
                    var alertMessage = string.Format("Say good-bye to {0} {1}", selectedPlayer.FirstName, selectedPlayer.LastName);

                    //Update player checkedIn status to false;
                    var checkedOutStatus = await _playerManager.UpdateCheckedInStatus(selectedPlayer, false);
                    await CoreMethods.DisplayAlert("Check out player", alertMessage, "OK");

                    //Remove player from Q, if the player is in Q. 
					var removed = await _queueManager.RemoveFromQueue(selectedPlayer.ID);
					if (removed)
					{
						await CoreMethods.DisplayAlert("Remove from queue", "Done", "OK");
						await RefreshPlayerList();
					}
                });
            }
        }


        public Command AddToQueue
        {
            get{
                return new Command(async(arg)=>{
                    var selectedPlayer = arg as Player;

                    //Update player checkedin status to True before putting them into Q.
                    var checkedInStatus = await _playerManager.UpdateCheckedInStatus(selectedPlayer, true);

                    //Add player into Q
                    if (checkedInStatus)
                    {
                        await _queueManager.AddToQueue(selectedPlayer);
                        await CoreMethods.DisplayAlert("Add player to queue, ready to be picked.", "ToQueue Command", "OK");

                        //Refresh the list of players. Because player's status changed.
                        await RefreshPlayerList();
                    }
                });
            }
        }


		Player _selectedPlayer;
		public Player SelectedItem
		{
			get
			{
				return _selectedPlayer;
			}
			set
			{
				_selectedPlayer = value;
				if (value != null)
				{
                   CoreMethods.PushPageModel<PlayerPageModel>(value);
				}
			}
		}



        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
           
            await RefreshPlayerList();
        }



		private async Task RefreshPlayerList()
		{
           	var result = await _playerManager.GetPlayerList();

            PlayerList.Clear();

			if (result != null)
			{
				foreach (var item in result.OrderBy(x => x.LastName))
				{
					PlayerList.Add(item);
				}
			}
			
			//RaisePropertyChanged("PlayerList");
			
		}
    }
}
