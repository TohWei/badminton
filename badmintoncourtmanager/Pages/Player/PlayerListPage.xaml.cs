using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    public partial class PlayerListPage : ContentPage
    {
        public PlayerListPage()
        {
            InitializeComponent();

        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            DisplayAlert("Edit Photo", "Take photo", "OK");
        }
    }
}
