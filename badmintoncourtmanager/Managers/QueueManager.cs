using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using badmintoncourtmanager.Database.Repositories;
using badmintoncourtmanager.Models;

namespace badmintoncourtmanager.Managers
{
    public class QueueManager : IQueueManager
    {
        public QueueManager()
        {
        }

        public Task<int> AddToQueue(Player player)
        {
            return Task.Factory.StartNew(() => QueueRepository.AddToQueue(player));
        }

        public Task<List<Player>> GetQueueList()
        {
            return Task.Factory.StartNew(() => QueueRepository.GetQueueList().Item1);
        }

        public Task<bool> RemoveFromQueue(int id)
        {
            return Task.Factory.StartNew(() => QueueRepository.RemoveFromQueue(id));
        }

        public Task<bool>RemoveAllFromQueue()
        {
            return Task.Factory.StartNew(() => QueueRepository.RemoveAllFromQueue());
        }
    }
}
