using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public interface ICourtManager
    {
        Task<List<Court>> InitCourts(int maxCourts);
        Task<List<Court>> GetAllCourtDetails();
        Task<int> UpdateCourtDetail(Court court);
        Task<bool> IsCourtFullyOccupied(Court court);


        Task<bool> DeleteSinglePlayerFromAllCourt(Player player);
        Task<bool> AssignPlayerToACourt(int courtId, int playerId);
        Task<Court> GetCourtDetails(int courtId);
    }
		
}
