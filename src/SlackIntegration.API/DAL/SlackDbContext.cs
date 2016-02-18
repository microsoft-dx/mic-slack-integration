using SlackIntegration.SlackLibrary;
using System.Data.Entity;

namespace SlackIntegration.DAL
{
    public class SlackDbContext : DbContext
    {
        public SlackDbContext(string connectionString): base(connectionString)
        {
        }

        public DbSet<SlackMessage> Messages { get; set; }
    }
}