using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    public partial class CourtListPage : ContentPage
    {
        public CourtListPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetupCourts();
        }

        private async void SetupCourts()
        {
			var vm = BindingContext as PageModels.CourtListPageModel;

            if (vm != null)
            {
                //Draw the UI once. 
                if (MainCourtLayout.Children.Count == 0)
                {
                    foreach (var item in vm.CourtList)
                    {
                        var v = new CustomViews.BadmintonCourtView();
                        var lbl = v.FindByName<Label>("LblCourtName");
                        lbl.Text = item.Name;

                        var btnStart = v.FindByName<Button>("BtnStart");
                        btnStart.CommandParameter = item.ID;

                        MainCourtLayout.Children.Add(v);

                        var bv = new BoxView()
                        {
                            HeightRequest = 30
                        };

                        MainCourtLayout.Children.Add(bv);
                    }
                }else{


                    await vm.RefreshCourtList();

                    //Loop through the UI (Courts)
                    foreach (var v in MainCourtLayout.Children)
                    {
                        if (v.GetType().Equals(typeof(CustomViews.BadmintonCourtView)))
                        {
                            var lblCourtName = v.FindByName<Label>("LblCourtName");
                            var lblPlayer1 = v.FindByName<Label>("LblPlayer1");
                            var lblPlayer2 = v.FindByName<Label>("LblPlayer2");
                            var lblPlayer3 = v.FindByName<Label>("LblPlayer3");
                            var lblPlayer4 = v.FindByName<Label>("LblPlayer4");

                            lblPlayer1.Text = "";
                            lblPlayer2.Text = "";
                            lblPlayer3.Text = "";
                            lblPlayer4.Text = "";

                            var circleImagePlayer1 = v.FindByName<Image>("CircleImagePlayer1");
                            var circleImagePlayer2 = v.FindByName<Image>("CircleImagePlayer2");
                            var circleImagePlayer3 = v.FindByName<Image>("CircleImagePlayer3");
                            var circleImagePlayer4 = v.FindByName<Image>("CircleImagePlayer4");

                            circleImagePlayer1.Source = null;
                            circleImagePlayer2.Source = null;
                            circleImagePlayer3.Source = null;
                            circleImagePlayer4.Source = null;


                            foreach(var x in vm.CourtList)
                            {
                                //If court Name matched. Then start getting player info.
                                if (x.Name.Equals(lblCourtName.Text))
                                {
                                    if (x.PlayerIds != null && x.PlayerIds.Any())
                                    {
                                        var playerDictionary = await vm.GetPlayer(x.PlayerIds);

                                        foreach(var p_id in x.PlayerIds){
                                            if (p_id > -1){
                                                if (lblPlayer1.Text == "")
                                                {
                                                    lblPlayer1.Text = playerDictionary[p_id].FirstName;
                                                    circleImagePlayer1.Source = playerDictionary[p_id].ImageSource;
                                                }
                                                else if (lblPlayer2.Text == ""){
                                                    lblPlayer2.Text = playerDictionary[p_id].FirstName;
                                                    circleImagePlayer2.Source = playerDictionary[p_id].ImageSource;
                                                }
												else if (lblPlayer3.Text == "")
												{
                                                    lblPlayer3.Text = playerDictionary[p_id].FirstName;
													circleImagePlayer3.Source = playerDictionary[p_id].ImageSource;
												}
												else if (lblPlayer4.Text == "")
												{
                                                    lblPlayer4.Text = playerDictionary[p_id].FirstName;
													circleImagePlayer4.Source = playerDictionary[p_id].ImageSource;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

						}
                    }

                }
            }
        }

       
    }
}
