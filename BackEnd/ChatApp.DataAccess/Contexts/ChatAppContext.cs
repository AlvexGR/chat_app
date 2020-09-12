using ChatApp.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.DataAccess.Contexts
{
    public class ChatAppContext : DbContext
    {
        protected ChatAppContext()
        {
        }

        public ChatAppContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
