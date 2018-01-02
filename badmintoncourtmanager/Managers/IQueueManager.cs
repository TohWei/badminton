using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public interface IQueueManager
    {
        Task<List<Player>> GetQueueList();
        Task<bool> RemoveFromQueue(int id);
        Task<bool> RemoveAllFromQueue();
        Task<int> AddToQueue(Player player);
    }
}
