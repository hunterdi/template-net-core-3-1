using Business;
using Business.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Architecture
{
    public class ApplicationDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TagTask> TagsTasks { get; set; }
        public DbSet<File> Files { get; set; }

        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntityMapping).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            //ManagerDomainTracker();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //ManagerDomainTracker();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ManagerDomainTracker()
        {
            ChangeTracker.DetectChanges();

            var changesTracked = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted
                || x.State == EntityState.Added || x.State == EntityState.Modified);

            foreach (var item in changesTracked)
            {
                UpdateLog(item);
                UpdateSoftDeleteStatuses(item);
            }
        }

        private void UpdateLog(EntityEntry item)
        {
            if (item.Entity is BaseDomain entity)
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        entity.Updated = DateTime.Now;
                        break;
                    case EntityState.Added:
                        entity.Created = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entity.Deleted = DateTime.Now;
                        break;
                }
            }
        }

        private void UpdateSoftDeleteStatuses(EntityEntry item)
        {
            if (item.Entity is BaseDomain entity)
            {
                switch (item.State)
                {
                    case EntityState.Modified:
                        entity.Visible = true;
                        break;
                    case EntityState.Added:
                        entity.Visible = true;
                        entity.Active = true;
                        break;
                    case EntityState.Deleted:
                        item.State = EntityState.Modified;
                        entity.Visible = false;
                        entity.Active = false;
                        break;
                }
            }
        }
    }
}
