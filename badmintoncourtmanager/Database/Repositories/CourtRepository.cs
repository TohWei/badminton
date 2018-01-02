using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using badmintoncourtmanager.Database.DBSchemas;
using badmintoncourtmanager.Models;
using Newtonsoft.Json;

namespace badmintoncourtmanager.Database.Repositories
{
    public class CourtRepository
    {
        public CourtRepository()
        {
        }

        //This method will return a list of courts from tblCourt. Court details include a list of player from tblPlayersInCourts.
        public static Tuple<List<Court>, DateTime>GetAllCourtDetails()
        {
            lock (DBHelper.Lock)
            {
                //Get court Information.
				var courts = DBHelper.DBWrapper.GetComplex<TblCourt>("SELECT c.Id," +
																"c.GuId," +
																"c.Name," +
																"c.IsOccupied from TblCourt c",
																new List<string>() { "TblCourt"}, new object[] {});
                if (courts != null)
                {
                    var courtList = new List<Court>();

                    //Loop through each court, then combine the player data into the return object. 
                    foreach (var c in courts)
                    {

                        var playerIdList = new List<int>();

                        System.Diagnostics.Debug.WriteLine("Court ID: " + c.ID);
						//Get players who already being assigned to a court. 
                        var playersInCourts = DBHelper.DBWrapper.GetComplex<TblPlayersInCourts>("SELECT pc.CourtId, pc.PlayerId from TblPlayersInCourts pc where pc.CourtId = ?",
                                                                                                new List<string>() { "TblPlayersInCourts" }, new object[] { c.ID});

                        if (playersInCourts != null && playersInCourts.Any()){
                            foreach(var pc in playersInCourts)
                            {
                                playerIdList.Add(pc.PlayerId);
                            }
                        }

                        courtList.Add(new Court()
                        {
                            ID = c.ID,
                            GuId = c.GuId,
                            Name = c.Name,
                            IsOccupied = c.IsOccupied,
                            PlayerIds = playerIdList
                        });
                    }
                    return new Tuple<List<Court>, DateTime>(courtList, courts.First().LastModified);
                }
                return null;
            }
        }



        //public static Tuple<Court, DateTime> GetCourtDetail(int id, Guid guId)
        public static Tuple<Court, DateTime> GetCourtDetail(int id)
		{
			lock (DBHelper.Lock)
			{
				//var rows = DBHelper.DBWrapper.GetComplex<TblCourt>("SELECT * FROM TblCourt WHERE ID = ? or GUID = ?",
				//new List<string> { "TblCourt" }, new object[] { id, guId });
				var rows = DBHelper.DBWrapper.GetComplex<TblCourt>("SELECT * FROM TblCourt WHERE ID = ?",
				new List<string> { "TblCourt" }, new object[] { id });

				var playerIdList = new List<int>();

				if (rows.Any())
				{
					var row = rows.First();


					//Get players who already being assigned to a court. 
					var playersInCourts = DBHelper.DBWrapper.GetComplex<TblPlayersInCourts>("SELECT pc.CourtId, pc.PlayerId from TblPlayersInCourts pc where pc.courtId = ?",
																							new List<string>() { "TblPlayersInCourts" }, new object[] { id });
					if (playersInCourts != null)
					{
						foreach (var pc in playersInCourts)
						{
							playerIdList.Add(pc.PlayerId);
						}
					}

                    var court = new Court
					{
						ID = row.ID,
						GuId = row.GuId,
						Name = row.Name,
						IsOccupied = row.IsOccupied,
                        PlayerIds = playerIdList
					};
                    return new Tuple<Court, DateTime>(court, row.LastModified);
				}
				return null;
			}
		}


        //Initialise once. Create a fixed list of courts into db.
        //Todo need to return data from db. So that ID will have value instead of 0.
		public static List<Court> InitCourtDetail(List<Court> maxListOfCourts)
		{
            lock (DBHelper.Lock)
            {
                try
                {
                    if (DBHelper.DBWrapper.IsTableExists<TblCourt>())
                    {
                        var deleted = DBHelper.DBWrapper.DeleteAllData<TblCourt>();
                        System.Diagnostics.Debug.WriteLine("Deleted :" + deleted);
                    }

                    foreach (var c in maxListOfCourts)
                    {
                        var tblCourtRow = new TblCourt()
                        {
                            GuId = c.GuId,
                            Name = c.Name,
                            IsOccupied = c.IsOccupied
                        };
                        DBHelper.DBWrapper.InsertNewRow<TblCourt>(tblCourtRow);
                    }

                    return GetAllCourtDetails().Item1;
                }catch(Exception ex){
                    return null;
                }
			}
		}
        


		public static int SaveOrUpdateCourt(Court court)
		{
			lock (DBHelper.Lock)
			{
                if (DBHelper.DBWrapper.IsTableExists<TblCourt>())
				{
                    //var result = GetCourtDetail(court.ID, court.GuId);
                    var result = GetCourtDetail(court.ID);
					if (result != null && result.Item1 != null)
					{
						return DBHelper.DBWrapper.Update("Update TblCourt set " +
                                                         "Name = ?, IsOccupied =? " +
                                                         "where GuId = ? or ID = ?"
                                                         , new object[] { 
                            court.Name, 
                            court.IsOccupied,
                            court.GuId, court.ID });
					}
				}

				var tblCourtRow = new TblCourt()
				{
					ID = court.ID,
					GuId = court.GuId,
					Name = court.Name,
					IsOccupied = court.IsOccupied,
				};

                return DBHelper.DBWrapper.InsertNewRow<TblCourt>(tblCourtRow);
			}
		}



        //NEW Area. 
        public static bool IsCourtFullyOccupied(int courtId){
            lock (DBHelper.Lock)
            {
                if (DBHelper.DBWrapper.IsTableExists<TblPlayersInCourts>())
                {
                    var count = DBHelper.DBWrapper.GetCount<int>("SELECT count(*) FROM TblPlayersInCourts WHERE CourtId = ?", new object[] { courtId });
                    //var rows = DBHelper.DBWrapper.GetComplex<int>("SELECT count(*) as totalPlayer FROM TblPlayersInCourts WHERE CourtId = ?",
                    //    new List<string> { "TblPlayersInCourts" }, new object[] { courtId });
                    if (count == 4){
                        return true;
                    }
                }
                return false;
            }
        }


		public static bool DeleteSinglePlayerFromCourt(Player player)
		{
			lock (DBHelper.Lock)
			{
				if (DBHelper.DBWrapper.IsTableExists<TblPlayersInCourts>())
				{
					var affactedRow = DBHelper.DBWrapper.ExecuteNonQuery("DELETE from TblPlayersInCourts WHERE PlayerId = ? ",
													   new object[] { player.ID });

					if (affactedRow > -1)
					{
						return true;
					}
				}
				return false;
			}
		}


        public static bool AssignPlayerToACourt(int courtId, int playerId){
			var tblPlayersInCourts = new TblPlayersInCourts()
			{
                CourtId = courtId,
                PlayerId = playerId
			};

			var affectedRow = DBHelper.DBWrapper.InsertNewRow<TblPlayersInCourts>(tblPlayersInCourts);
            if (affectedRow > -1){
                return true;
            }
            return false;
        }
    }
}
