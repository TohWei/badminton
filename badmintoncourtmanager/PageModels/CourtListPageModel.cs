using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Models;
using Xamarin.Forms;

namespace badmintoncourtmanager.PageModels
{
    
    public class CourtListPageModel : FreshMvvm.FreshBasePageModel
    {
        ICourtManager _courtManager;
        IPlayerManager _playerManager;

        //iOC
        public CourtListPageModel(ICourtManager counrtManager, IPlayerManager playerManager)
        {
            _courtManager = counrtManager;
            _playerManager = playerManager;
        }

        public List<Court> CourtList{
            get;
            set;
        }


        public override async void Init(object initData)
        {
            base.Init(initData);

            if (CourtList == null)
            {
                //Step1: Initialise it. Create courts record into db. 
                CourtList = await _courtManager.InitCourts(6);

            }else{
                CourtList = await _courtManager.GetAllCourtDetails();
            }
        }


      
		public async Task RefreshCourtList()
		{
			var result = await _courtManager.GetAllCourtDetails();

			if (result != null)
			{
				CourtList.Clear();

				foreach (var item in result.OrderBy(x => x.ID))
				{
					CourtList.Add(item);
				}
			}
		}

        public async Task<Dictionary<int, Player>> GetPlayer(List<int> playerIds)
        {
            var playerDict = new Dictionary<int, Player>();
			foreach (var p in playerIds)
			{
				var resultPlayer = await _playerManager.GetPlayer(p);
                if (resultPlayer != null){
                    if (!playerDict.ContainsKey(resultPlayer.ID)){
                        playerDict.Add(resultPlayer.ID, resultPlayer);    
                    }
                }
			}
            return playerDict;
        }

		

		public Command CommandStartGame
		{
			get
			{
				return new Command(async (arg) =>
				{
                    int courtId = (int)arg;

                    await Task.Delay(10);

                    var singleCourtDetail = await _courtManager.GetCourtDetails(courtId);
                    var playerIds = singleCourtDetail.PlayerIds;

                    //Todo Loop through the playerId, change the player status to IsPlaying.
                    foreach(var pId in playerIds)
                    {
                        var player = await _playerManager.GetPlayer(pId);
                        player.PlayerState = PlayerState.Playing;
                        await _playerManager.CreateOrUpdatePlayer(player);
                    }

                    /*
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
                    */
				});
			}
		}

	}
}
