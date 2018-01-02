using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using badmintoncourtmanager.Managers;
using badmintoncourtmanager.Models;
using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    public partial class QueueListPage : ContentPage
    {
        public QueueListPage()
        {
            InitializeComponent();
        }

		//Bind to ViewCell.
		private void OnBindingContextChanged(object sender, EventArgs e)
		{
			base.OnBindingContextChanged();

            if (BindingContext == null)
            {
                return;
            }else{
                var vm = BindingContext as PageModels.QueueListPageModel;
                if (vm != null)
                {
                    var courts = vm.CourtList;

                    if (courts != null)
                    {
                        //Get the ViewCell. 
                        ViewCell theViewCell = ((ViewCell)sender);

                        //Get the viewCell's bindingContext, which is 'Player' the object.
                        var viewCellSelectedPlayer = (Player)theViewCell.BindingContext;


                        //Find the main StackLayoutHolder. 
                        var courtsHolderStackLayout = theViewCell.FindByName<StackLayout>("CourtsHolder");

                        List<StackLayout> innerStackLayouts = new List<StackLayout>();

                        //If the CourtsHolder is empty, create view during runtime.
                        if (courtsHolderStackLayout.Children.Count == 0)
                        {
                            foreach (var item in courts)
                            {
                                var lblPrefix = new Label();
                                lblPrefix.HorizontalTextAlignment = TextAlignment.Center;
                                lblPrefix.WidthRequest = 60;
                                lblPrefix.Text = "Court";
                                lblPrefix.TextColor = Color.Blue;
                                lblPrefix.BackgroundColor = Color.Silver;

                                var lblCourtName = new Label();
                                lblCourtName.Text = item.Name;
                                lblCourtName.HorizontalTextAlignment = TextAlignment.Center;

                                var innerStackLayout = new StackLayout()
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    IsClippedToBounds = true,
                                    //Margin = 5
                                };
                                innerStackLayout.Children.Add(lblPrefix);
                                innerStackLayout.Children.Add(lblCourtName);
                                innerStackLayout.ClassId = item.GuId.ToString();

                                //Highlight player who already assigned to a court.
                                if (item.PlayerIds != null && item.PlayerIds.Any())
                                {
                                    if (item.PlayerIds.Contains(viewCellSelectedPlayer.ID))
                                    {
										innerStackLayout.BackgroundColor = Color.Gray;
                                    }
                                }else{
                                    innerStackLayout.BackgroundColor = Color.White;
                                }

                                innerStackLayouts.Add(innerStackLayout);


                                Frame frm = new Frame()
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Padding = 0,
                                    Margin = 0,
                                    Content = innerStackLayout
                                };


                                //Tap event on each court in each cell. 
                                TapGestureRecognizer tap = new TapGestureRecognizer();
                                frm.GestureRecognizers.Add(tap);
                                tap.Tapped += async(s1, e1) => {
                                    
                                    CourtManager courtManager = new CourtManager();

                                    //Reset all the frame(button look)'s background color. 
                                    if (innerStackLayouts.Any())
                                    {
                                        //Delete data from PlayersInCourts. To ensure this player is not being assigned to any court.
                                        await courtManager.DeleteSinglePlayerFromAllCourt(viewCellSelectedPlayer);

										foreach (var innerLayout in innerStackLayouts)
										{
											innerLayout.BackgroundColor = Color.White;
										}

									}

                                     //Check as long as it is not fully occupied, then update the player back to playerInCourt db.
                                    var isFullyOccupied = await courtManager.IsCourtFullyOccupied(item);
                                    if (!isFullyOccupied)
                                    {
                                        await courtManager.AssignPlayerToACourt(item.ID, viewCellSelectedPlayer.ID);
                                        innerStackLayout.BackgroundColor = Color.Gray;
                                    }else{
										await DisplayAlert("Oops! It is FULL ", "Court " + item.Name + " is fully occupied.", "Ok");
                                    }

                                       // courtManager.ResetAPlayerInCourt();
										//Need to remove player from the court as well.
                                        /*
                                        var taskList = new List<Task>();

                                        foreach(var c in courts)
                                        {
                                            var tsk = courtManager.ResetAPlayerInCourt(c, cellBindingContext);
											taskList.Add(tsk);
                                        }
                                        try{
                                            Task.WaitAll(taskList.ToArray());    
                                        }catch(AggregateException ex1)
                                        {
                                            System.Diagnostics.Debug.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS EXPECTED)");
											for (int j = 0; j < ex1.InnerExceptions.Count; j++)
											{
												System.Diagnostics.Debug.WriteLine("\n-------------------------------------------------\n{0}", ex1.InnerExceptions[j].ToString());
											}
                                        }
                                        */
									//}

                                   

                                };

                                courtsHolderStackLayout.Children.Add(frm);
                            }
                        }
                    }
                }
            }

            /*
			ViewCell theViewCell = ((ViewCell)sender);
			var item = theViewCell.BindingContext as ListItemModel;
			theViewCell.ContextActions.Clear();

			if (item != null)
			{
				if (item.ListItemType == ListItemTypeEnum.FavoritePlaces
				   || item.ListItemType == ListItemTypeEnum.FavoritePeople)
				{
					theViewCell.ContextActions.Add(new MenuItem()
					{
						Text = "Delete"
					});
				}
			}
			*/
		}



		

    }
}
