using System;
namespace badmintoncourtmanager.Database.DBSchemas
{
    public class TblCourt  : TblBase
    {
        public TblCourt()
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

    }
}
