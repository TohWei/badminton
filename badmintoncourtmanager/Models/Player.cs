using System;
using badmintoncourtmanager.Services;
using Xamarin.Forms;

namespace badmintoncourtmanager.Models
{
    public class Player : BaseClass
    {
        public Player()
        {
            Level = 0;
            DOB = DateTime.Today.AddYears(-15);
        }

        public string FirstName
        {
            get;
            set;
        }

        public string LastName
        {
            get;
            set;
        }

        public string Gender
        {
            get;set;
        }

        public int Level
        {
            get;
            set;
        }

        public DateTime DOB{
            get;set;
        }

		public bool CheckedIn
		{
			get;
			set;
		}

        public string ProfilePhotoFilePath { get; set; }


        public ImageSource ImageSource {
            get{
                return ImageSource.FromFile(ProfilePhotoFilePath);
            }
        }


        //Important property for queue ordering.
        public DateTime DateTimeWhenPlayerSetInTheQueue
        {
            get;
            set;
        }

        public PlayerState PlayerState
        {
            get;
            set;
        }

        //Formatted value:
        public string FormattedWaitingInQTime
        {
            get{
                return Util.AsShortRelativeDate(DateTimeWhenPlayerSetInTheQueue, true) + " ago";
            }
        }
    }

}
