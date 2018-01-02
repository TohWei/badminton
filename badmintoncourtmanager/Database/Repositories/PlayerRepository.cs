using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using badmintoncourtmanager.Database.DBSchemas;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Database.Repositories
{
    public class PlayerRepository
    {
		public static Tuple<Player, DateTime> GetPlayer(int id)
		{
			lock (DBHelper.Lock)
			{
				var rows = DBHelper.DBWrapper.GetComplex<TblPlayer>("SELECT * FROM TblPlayer WHERE ID = ?",
				new List<string> { "TblPlayer" }, new object[] { id });

				if (rows.Any())
				{
					var row = rows.First();
					var player = new Player
					{
                        ID = row.ID,
                        FirstName = row.FirstName,
                        LastName = row.LastName,
                        Gender = row.Gender,
                        Level = row.Level,
                        ProfilePhotoFilePath = row.ProfilePhotoFilePath,
                        DateTimeWhenPlayerSetInTheQueue = row.DateTimeWhenPlayerSetInTheQueue,
                        PlayerState = (Models.PlayerState)row.PlayerState,
					};
                    return new Tuple<Player, DateTime>(player, row.LastModified);
				}
				return null;
			}
		}

		public static int SaveOrUpdatePlayer(Player player)
		{
			lock (DBHelper.Lock)
			{
                if (DBHelper.DBWrapper.IsTableExists<TblPlayer>())
				{
                    var result = GetPlayer(player.ID);
					if (result != null && result.Item1 != null)
					{
						return DBHelper.DBWrapper.Update("Update TblPlayer set FirstName = ?, LastName =?, Gender= ? , Level = ? , DOB =?, ProfilePhotoFilePath = ? , DateTimeWhenPlayerSetInTheQueue =? , PlayerState = ? where ID = ?"
                                                         , new object[] { player.FirstName, player.LastName, player.Gender, player.Level, player.DOB, player.ProfilePhotoFilePath, player.DateTimeWhenPlayerSetInTheQueue, player.PlayerState, player.ID });
					}
				}

                var tblPlayerRow = new TblPlayer()
				{
					ID = player.ID,
					FirstName = player.FirstName,
					LastName = player.LastName,
					Gender = player.Gender,
					Level = player.Level,
                    DOB = player.DOB,
                    ProfilePhotoFilePath = player.ProfilePhotoFilePath,
					DateTimeWhenPlayerSetInTheQueue = player.DateTimeWhenPlayerSetInTheQueue,
					PlayerState = (PlayerState)player.PlayerState,
				};

				return DBHelper.DBWrapper.InsertNewRow<TblPlayer>(tblPlayerRow);
			}
		}


        public static int CreatePlayer(Player player)
		{
			lock (DBHelper.Lock)
			{
                if (DBHelper.DBWrapper.IsTableExists<TblPlayer>())
				{
					DBHelper.DBWrapper.ExecuteNonQuery("DELETE from TblPlayer WHERE FirstName = ? AND LastName = ? AND Gender = ? AND DOB = ?", 
                                                       new object[] { player.FirstName, player.LastName, player.Gender, player.DOB });
				}

                var tblPlayerRow = new TblPlayer()
				{
					ID = player.ID,
					FirstName = player.FirstName,
					LastName = player.LastName,
					Gender = player.Gender,
					Level = player.Level,
					DOB = player.DOB,
					DateTimeWhenPlayerSetInTheQueue = player.DateTimeWhenPlayerSetInTheQueue,
					PlayerState = (PlayerState)player.PlayerState,
				};

				return DBHelper.DBWrapper.InsertNewRow<TblPlayer>(tblPlayerRow);
			}
		}

		public static Tuple<List<Player>, DateTime> GetPlayerList()
		{
            IEnumerable<TblPlayer> rows = DBHelper.DBWrapper.Get<TblPlayer>();
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

		public static bool DeletePlayer(int id)
		{
			lock (DBHelper.Lock)
			{
                var deleted = DBHelper.DBWrapper.DeleteData<TblPlayer>(id);
				return deleted;
			}
		}


        public static bool UpdateCheckedInStatus(Player player, bool IsCheckedIn)
        {
            lock(DBHelper.Lock)
            {
				if (DBHelper.DBWrapper.IsTableExists<TblPlayer>())
				{
					var result = GetPlayer(player.ID);
					if (result != null && result.Item1 != null)
					{
						var affectedRow = DBHelper.DBWrapper.Update("Update TblPlayer set CheckedIn = ? where ID = ?", new object[] { IsCheckedIn,  player.ID });
                        if (affectedRow > -1){
                            return true;
                        }
					}
				}
                return false;
            }  
        }
    }
}
