using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public interface IPlayerManager
    {
		Task<List<Player>> GetPlayerList();
		Task<int> CreatePlayer(Player player);
		Task<int> CreateOrUpdatePlayer(Player player);
		Task<bool> DeletePlayer(int id);
        Task<bool> UpdateCheckedInStatus(Player player, bool IsCheckedIn);

        Task<Player> GetPlayer(int id);
    }
}
