using System;
using System.Collections.Generic;

namespace badmintoncourtmanager.Models
{
    public class Court : BaseClass
    {
        public Court()
        {
        }

		public Guid GuId
		{
			get;
			set;
		}

		//Court Name.
		public string Name
		{
			get;
			set;
		}

		//Is the court fully occupied.
		public bool IsOccupied
		{
			get;
			set;
		}

		
        public List<int> PlayerIds{
            get;
            set;
        }

        /*
        public Player Player1
		{
			get;
			set;
		}

        public Player Player2
		{
			get;
			set;
		}

		public Player Player3
		{
			get;
			set;
		}

        public Player Player4
		{
			get;
			set;
		}
		*/

    }
}
