using System;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Database.DBSchemas
{
    public class TblPlayer : TblBase
    {
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
			get;
			set;
		}

		public int Level
		{
			get;
			set;
		}

        //DOB will keep the player unique. Unless we have email and phone num.
        public DateTime DOB
        { get; set; }

        public string ProfilePhotoFilePath 
        { 
             get; 
            set; 
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

        public bool CheckedIn
        {
            get;
            set;
        }
    }

}
