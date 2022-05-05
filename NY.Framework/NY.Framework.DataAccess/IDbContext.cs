﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.DataAccess
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DatabaseFacade GetDatabase();

        void SetModifedState<TEntity>(TEntity entity) where TEntity : class;
        void SetDeletedState<TEntity>(TEntity entity) where TEntity : class;
        

        int SaveChanges();

        
    }
}
