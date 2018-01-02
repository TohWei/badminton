using System;
namespace badmintoncourtmanager.Database.DBSchemas
{
    public class TblPlayersInCourts : TblBase
    {
        //This is a many to many relationship table. 
        public TblPlayersInCourts()
        {
        }

        public int CourtId
        {
            get;
            set;
        }

        public int PlayerId
        {
            get;
            set;    
        }
    }
}
