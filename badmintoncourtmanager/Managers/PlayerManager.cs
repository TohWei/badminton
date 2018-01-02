using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Database.Repositories;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public class PlayerManager : IPlayerManager
    {
        public PlayerManager()
        {
        }

        public Task<int> CreateOrUpdatePlayer(Player player)
        {
            return Task.Factory.StartNew(() => PlayerRepository.SaveOrUpdatePlayer(player));
        }

        public Task<int> CreatePlayer(Player player)
        {
            return Task.Factory.StartNew(() => PlayerRepository.CreatePlayer(player));
        }

        public Task<bool> DeletePlayer(int id)
        {
            return Task.Factory.StartNew(() => PlayerRepository.DeletePlayer(id));
        }

        public Task<List<Player>> GetPlayerList()
        {
			return Task.Factory.StartNew(() => PlayerRepository.GetPlayerList().Item1);
        }

        public Task<bool> UpdateCheckedInStatus(Player player, bool IsCheckedIn)
        {
            return Task.Factory.StartNew(() => PlayerRepository.UpdateCheckedInStatus(player, IsCheckedIn));
        }

		public Task<Player> GetPlayer(int id)
		{
            return Task.Factory.StartNew(() => PlayerRepository.GetPlayer(id).Item1);
		}
    }
}
