using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using badmintoncourtmanager.Database.DBSchemas;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Database.Repositories
{
    public class QueueRepository
    {
		public static Tuple<List<Player>, DateTime> GetQueueList()
		{
			//var rows = DBHelper.DBWrapper.GetComplex<TblPlayer>("SELECT * from TblPlayer p inner join TblQueue q on p.ID = Q.PlayerId", 
			//                                                                     new List<string>(){"TblPlayer", "TblQueue"});

			var rows = DBHelper.DBWrapper.GetComplex<TblPlayer>("SELECT p.ID," +
                                                                "p.FirstName," +
                                                                "p.LastName," +
                                                                "p.Gender," +
                                                                "p.Level," +
                                                                "p.DOB," +
                                                                "p.ProfilePhotoFilePath," +
                                                                "p.DateTimeWhenPlayerSetInTheQueue," +
                                                                "p.PlayerState," +
                                                                "p.CheckedIn from TblPlayer p inner join TblQueue q on p.ID = Q.PlayerId where p.CheckedIn =?", 
                                                                new List<string>(){"TblPlayer", "TblQueue"}, new object[] {true});
			try
			{
				if (rows != null && rows.Any())
				{
					List<Player> playerList = new List<Player>();
					foreach (var item in rows)
					{
						var player = new Player
						{
                            ID = item.ID,
							FirstName = item.FirstName,
							LastName = item.LastName,
							Gender = item.Gender,
							Level = item.Level,
							DOB = item.DOB,
							ProfilePhotoFilePath = item.ProfilePhotoFilePath,
							DateTimeWhenPlayerSetInTheQueue = item.DateTimeWhenPlayerSetInTheQueue,
							PlayerState = (Models.PlayerState)item.PlayerState,
                            CheckedIn = item.CheckedIn
						};
						playerList.Add(player);
					}
					return new Tuple<List<Player>, DateTime>(playerList, rows.First().LastModified);
				}
			}
			catch (Exception)
			{

			}
			return Tuple.Create<List<Player>, DateTime>(null, DateTime.UtcNow);
		}


		public static int AddToQueue(Player player)
		{
			lock (DBHelper.Lock)
			{
				if (DBHelper.DBWrapper.IsTableExists<TblQueue>())
				{
					DBHelper.DBWrapper.ExecuteNonQuery("DELETE from TblQueue WHERE PlayerId = ?",
                                                       new object[] { player.ID});
				}

                var tblQRow = new TblQueue()
                {
                    PlayerId = player.ID,
				};

                var insertedOK = DBHelper.DBWrapper.InsertNewRow<TblQueue>(tblQRow);
                if (insertedOK > 0){
                    UpdatePlayerState(player.ID, PlayerState.InQueue);
                }
                return insertedOK;
			}
		}


		public static bool RemoveFromQueue(int playerId)
		{
			lock (DBHelper.Lock)
			{
                if (DBHelper.DBWrapper.IsTableExists<TblQueue>())
				{
					var deleted =  DBHelper.DBWrapper.ExecuteNonQuery("DELETE from TblQueue WHERE PlayerId = ?",
													   new object[] { playerId });
                    if (deleted > -1){
                        UpdatePlayerState(playerId, PlayerState.NotInQueue);
                        return true;
                    }
				}
                return false;
			}
		}

		public static bool RemoveAllFromQueue()
		{
			lock (DBHelper.Lock)
			{
                var deleted = DBHelper.DBWrapper.DeleteAllData<TblQueue>();
				if (deleted)
				{
                    UpdateAllPlayerState(PlayerState.NotInQueue);
				}
				return deleted;
			}
		}


        public static int UpdatePlayerState(int id, PlayerState playerState = PlayerState.InQueue)
		{
            //lock (DBHelper.Lock)
            Monitor.Enter(DBHelper.Lock);
			{
				if (DBHelper.DBWrapper.IsTableExists<TblPlayer>())
				{
					var updated = DBHelper.DBWrapper.Update("Update TblPlayer set DateTimeWhenPlayerSetInTheQueue =? , PlayerState = ? where ID = ?"
														 , new object[] {
						DateTime.UtcNow,
						playerState,
						id });
                    Monitor.Exit(DBHelper.Lock);
                    return updated;
				}
                return -1;
			}
		}


		public static int UpdateAllPlayerState(PlayerState playerState)
		{
			lock (DBHelper.Lock)
			{
				if (DBHelper.DBWrapper.IsTableExists<TblPlayer>())
				{
					return DBHelper.DBWrapper.Update("Update TblPlayer set DateTimeWhenPlayerSetInTheQueue =? , PlayerState = ?"
													 , new object[] {
						DateTime.UtcNow,
						playerState,
						});
				}
				return -1;
			}
		}
    }
}
