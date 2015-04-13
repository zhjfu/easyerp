﻿namespace Domain.EntityFramework
{
    using Infrastructure.Domain;
    using System.Data.Entity;

    public class EntityFrameworkUnitOfWork : UnitOfWork
    {
        private readonly DbContext dbContext;

        public EntityFrameworkUnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override void Commit()
        {
            base.Commit();
            this.dbContext.SaveChanges();
        }
    }
}