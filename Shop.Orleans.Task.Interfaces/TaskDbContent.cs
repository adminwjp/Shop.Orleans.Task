using Microsoft.EntityFrameworkCore;
using Shop.Tasks;
using Utility;

namespace Shop
{
    public class TaskDbContent: DbContext
    {
        public TaskDbContent( DbContextOptions<TaskDbContent> options):base(options)
        {

        }
        public DbSet<TaskEntity> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>(entity =>
            {
                entity.HasKey(it => it.Id);
                if (DbConfig.Flag == DbFlag.MySql)
                {
                    entity.Property(it => it.CreationTime).HasColumnName("creation_time").HasColumnType("datetime");
                    entity.Property(it => it.LastModificationTime).HasColumnName("last_modification_time").HasColumnType("datetime");
                    entity.Property(it => it.DeletionTime).HasColumnName("deletion_time").HasColumnType("datetime");
                    entity.Property(it => it.NextWorkTime).HasColumnName("next_work_time").HasColumnType("datetime");
                }
                entity.Property(it => it.IsDeleted).HasColumnName("is_deleted");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
