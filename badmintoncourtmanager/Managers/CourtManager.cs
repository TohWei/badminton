using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Database.Repositories;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public class CourtManager : ICourtManager
    {
        public CourtManager()
        {
        }

        //Will be used in Queue list. Provide a list of Q that is not fully occupied. 
        public Task<List<Court>> GetAllCourtDetails()
        {
            return Task.Factory.StartNew(() => CourtRepository.GetAllCourtDetails().Item1);
        }

        //Initialise the courts. So that we can draw the UI. 
        //This should be called ONCE. 
        public Task<List<Court>> InitCourts(int maxCourts)
        {
            return Task.Factory.StartNew(() =>
            {
                var courts = new List<Court>();

                for (int i = 0; i < maxCourts; i++)
                {
                    int courtNum = i + 1;
                    string courtName = courtNum.ToString();
                    var newCourt = new Court() { GuId = Guid.NewGuid(), Name = courtName, IsOccupied = false };
                    courts.Add(newCourt);
                };

                return CourtRepository.InitCourtDetail(courts);

            });
        }

        //Update the court when assigning a player to a court. Need to update the stattus too.
        public Task<int> UpdateCourtDetail(Court court)
        {
            return Task.Factory.StartNew(() => CourtRepository.SaveOrUpdateCourt(court));
        }


        ///NEW area
        public Task<bool> IsCourtFullyOccupied(Court court)
        {
            return Task.Factory.StartNew(() => CourtRepository.IsCourtFullyOccupied(court.ID));
        }

		public Task<bool> DeleteSinglePlayerFromAllCourt(Player player)
		{
			return Task.Factory.StartNew(() => CourtRepository.DeleteSinglePlayerFromCourt(player));
		}

        public Task<bool> AssignPlayerToACourt(int courtId, int playerId)
        {
            return Task.Factory.StartNew(() => CourtRepository.AssignPlayerToACourt(courtId, playerId));
        }

		public Task<Court> GetCourtDetails(int courtId)
		{
            return Task.Factory.StartNew(() => CourtRepository.GetCourtDetail(courtId).Item1);
		}

		


    }
}
