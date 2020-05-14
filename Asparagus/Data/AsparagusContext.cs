using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Asparagus.Models;
using System.Threading;

namespace Asparagus.Data
{
    public class AsparagusContext : DbContext
    {
        public AsparagusContext (DbContextOptions<AsparagusContext> options)
            : base(options)
        {
        }

        public DbSet<Asparagus.Models.Person> People { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessSave();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessSave()
        {
            var currentTime = DateTimeOffset.Now;
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is Entity ))
            {
                var entity = item.Entity as Entity;
                entity.CreateDate = currentTime;
                entity.CountOfAsparagus++;
                entity.ModifiedDate = currentTime;
            }

            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Unchanged && e.Entity is Entity))
            {
                var entity = item.Entity as Entity;
                entity.CountOfAsparagus++;
                entity.ModifiedDate = currentTime;
                item.Property(nameof(entity.CreateDate)).IsModified = false;
            }
        }
    }
}
