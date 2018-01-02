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
    public class QueueListPageModel : FreshMvvm.FreshBasePageModel
    {

        IQueueManager _queueManager;
        ICourtManager _courtManager;

        public QueueListPageModel(IQueueManager queueManager, ICourtManager courtManager)
        {
            _queueManager = queueManager;
            _courtManager = courtManager;
        }

		public ObservableCollection<Player> QueueList
		{
			get; set;
		}

        public override async void Init(object initData)
        {
            base.Init(initData);

            //Init Queue.
			QueueList = new ObservableCollection<Player>();

            //todo need to filter it by PlayerState to Not equal to IsPlaying.
            var queueResult = await _queueManager.GetQueueList();
			if (queueResult != null)
			{
                foreach (var item in queueResult.OrderBy(x => x.DateTimeWhenPlayerSetInTheQueue))
				{
					QueueList.Add(item);
				}
			}

            //Initi Courts
            CourtList = new ObservableCollection<Court>();
            var courtResult = await _courtManager.GetAllCourtDetails();
			if (courtResult != null)
			{
				foreach (var item in courtResult)
				{
					System.Diagnostics.Debug.WriteLine("ID:" + item.ID);
					System.Diagnostics.Debug.WriteLine("GuiD:" + item.GuId);
					CourtList.Add(item);
				}
			}

        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            await RefreshCourtList();

            await RefreshQueueList();

        }


        public Command ResetQueueCommand
		{
            get
            {
                return new Command(async()=>{
                    var ok = await CoreMethods.DisplayAlert("Reset the queue","This will remove all players from the queue.","OK","Cancel");
                    if (ok){
                        var removed = await _queueManager.RemoveAllFromQueue();
                        if (removed)
                        {
                            QueueList.Clear();

                            //Todo: need to remove record from tblPlayersInCourts only if playerState is InQueue. Not onPlaying.
                        }
                    }
                });
            }
        }


		public Command TapRemovePlayerFromQueue
		{
			get
			{
				return new Command(async (arg) =>
				{
                    var selectedPlayer = arg as Player;
                    var playerFullName = string.Format("{0} {1}", selectedPlayer.FirstName, selectedPlayer.LastName);
                    var ok = await CoreMethods.DisplayAlert("Remove player from the Queue.", "Remove " +playerFullName + " from the queue will not check out the player.", "OK", "Cancel");
                    if (ok)
                    {
                        //Remove player from TblQueue table.
                        var removed = await _queueManager.RemoveFromQueue(selectedPlayer.ID);
                        if (removed){
                            await CoreMethods.DisplayAlert("Remove from queue","Done","OK");
                            await RefreshQueueList();
                        }


                        //Need to remove player from the TblCourt as well. 
                        if (removed)
                        {
                            await _courtManager.DeleteSinglePlayerFromAllCourt(selectedPlayer);
                        }
                    }
				});
			}
		}


        private async Task RefreshQueueList()
        {
			var result = await _queueManager.GetQueueList();

            QueueList.Clear();

			if (result != null)
			{
                //Filtered by state
                result = result.Where(x => x.PlayerState != PlayerState.Playing).ToList();
				foreach (var item in result.OrderBy(x => x.DateTimeWhenPlayerSetInTheQueue))
				{
					QueueList.Add(item);
				}
            }else{
                RaisePropertyChanged("QueueList");
            }
        }


        #region Court List
        public ObservableCollection<Court> CourtList
        {
            get;
            set;
        }

        private async Task RefreshCourtList()
        {
            var result = await _courtManager.GetAllCourtDetails();

			if (result != null)
			{
                CourtList.Clear();

                foreach (var item in result.OrderBy(x => x.ID))
				{
                    System.Diagnostics.Debug.WriteLine("ID:" + item.ID);
                    System.Diagnostics.Debug.WriteLine("GuiD:" + item.GuId);
                    System.Diagnostics.Debug.WriteLine("Name:" + item.Name);
					CourtList.Add(item);
				}
			}
        }

       
		#endregion

	}
}
