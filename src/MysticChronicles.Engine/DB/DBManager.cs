using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using MysticChronicles.Engine.DB.Tables;

namespace MysticChronicles.Engine.DB
{
    public class DBManager : DbContext
    {
        public DbSet<Games> Games { get; set; }

        public DbSet<PartyMembers> PartyMembers { get; set; }

        public DbSet<GameVariables> GameVariables { get; set; }

        public DbSet<Items> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={Common.Constants.SQLITE_DB_NAME}");
        }

        public void Initialize()
        {
            Database.Migrate();
        }

        public void Delete<T>(T obj) where T : BaseTable
        {
            Set<T>().Remove(obj);
        }

        public async Task<T> SelectOneAsync<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression = null) where T : BaseTable => await Set<T>().FirstOrDefaultAsync(expression);

        public List<T> SelectMany<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression = null) where T: BaseTable => Set<T>().Where(expression).ToList();

        public async Task<int> InsertOneAsync<T>(T obj) where T : BaseTable
        {
            Set<T>().Add(obj);

            return await SaveChangesAsync();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var changeSet = ChangeTracker.Entries();

            if (changeSet == null)
            {
                return base.SaveChangesAsync(cancellationToken);
            }

            foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Member("Created").CurrentValue = DateTime.Now;
                        entry.Member("Active").CurrentValue = true;
                        break;
                }

                entry.Member("Modified").CurrentValue = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}