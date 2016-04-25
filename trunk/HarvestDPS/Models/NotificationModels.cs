using System.Collections.Generic;
using Harvest.Net.Models;

namespace HarvestDPS.Models
{
    public class NotificationIndexModel
    {
        public IList<Client> Clients { get; set; }
        public IList<User> Users { get; set; }
        public IList<Project> Projects { get; set; }
    }
}