using Microsoft.EntityFrameworkCore;
using ModelsLibrary.MessageModels.Entity;
using ModelsLibrary.UserModels.Entity;

namespace UserApi.Context
{
    public partial class ContextApp : DbContext
    {
        private readonly string _connectionString;

        public ContextApp() { }

        public ContextApp(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<RoleEntity> Roles { get; set; }
        public virtual DbSet<MessageEntity> Messages { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Password)
                    .HasMaxLength(32)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(255);

                entity.HasOne(e => e.RoleType).WithMany(e => e.Users);
            });

            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.SenderId);
                entity.Property(x => x.RecipientId);

                entity.Property(e => e.Text)
                    .HasMaxLength(1000);

                entity.HasOne(x => x.Sender)
                    .WithMany(x => x.SendMessages)
                    .HasForeignKey(x => x.SenderId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Recipient)
                    .WithMany(x => x.ReceiveMessages)
                    .HasForeignKey(x => x.RecipientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
