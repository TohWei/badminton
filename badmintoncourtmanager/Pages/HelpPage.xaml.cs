using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace badmintoncourtmanager.Pages
{
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            string txt = "Section: Players - A place to register and manage planyer. This is also a section to check-in and check-out the attendant of a player. ";
            txt += "\t\r";
            txt += "Check-in a player will set the player into the waiting queue.";
            txt += "\t\r";
            txt += "Check-out a player will remove the player from all the queues.";

			txt += "\t\r";
            txt += "\t\r";
            txt += "Section:Queues - show a list of player who are currently waiting in a queue. Player state = InQueue.";
			txt += "\t\r";
			txt += "Show the waiting time of each player in the queue.";
			txt += "\t\r";
            txt += "User can assign player to a court in this section. Maximum 4 players can be assigned to a court.";
			txt += "\t\r";
            txt += "Quite Queue - a function to quite a player from the queue. Note: this will not check-out the player. [Technically will remove player from q table as well as playersInCourt table.]";
			txt += "\t\r";
            txt += "Reset Queue - this will reset the entire queue by clearing all players in queue table. [Technically need to Loop through the player who are in queue and detele the player from playersInCourts.]";
			txt += "\t\r";

			txt += "\t\r";
            txt += "\t\r";
			txt += "Section: Courts - show a list of available courts. Show a list of player who are currently in the court ready to play / playing.";
			txt += "\t\r";
			txt += "The [start game button] in a court - This will update the player state to 'Playing'; will put all the players back to queue.";
            txt += "\t\r";
            txt += "The [finish game button] in a court - This will clear all the players from the court; will put all the players back to queue.";
            HelpText.Text = txt;
        }
    }
}
