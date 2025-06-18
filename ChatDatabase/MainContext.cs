using ChatDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatDatabase
{
    public class MainContext : DbContext
    {
        public DbSet<ConversationEntity> Conversations { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }


        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
