using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NY.Framework.Infrastructure;
using NY.Framework.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Proxies;

namespace NY.Framework.DataAccess
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    //[DbConfigurationType(typeof(System.Data.Entity.DbConfiguration))]
    public partial class EFDbContext : DbContext, IDbContext
    {
        string ConnectionString = "";
        //DbSet<User> Users;
        //DbSet<StateAndDivision> StateAndDivisions;
        
        public EFDbContext(string ConnectionString) : base() 
        {
            this.ConnectionString = ConnectionString;
            //Database.SetInitializer<EFDbContext>(null);
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(ConnectionString);
            }
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EFDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        


        public DatabaseFacade GetDatabase()
        {
            return base.Database;
        }

        public void SetModifedState<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }
        

        public void SetDeletedState<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Deleted;
        }

        public override int SaveChanges()
        {
            #region ForSoftDelete
            ChangeTracker.DetectChanges();
            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);
            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is ISoftDeleteEntity entity)
                {
                    // Set the entity to unchanged (if we mark the whole entity as Modified, every field gets sent to Db as an update)
                    item.State = EntityState.Unchanged;
                    // Only update the IsDeleted flag - only this will get sent to the Db
                    entity.IsDeleted = true;
                }
            }
            #endregion ForSoftDelete
            return base.SaveChanges();
        }
    }

  
}
