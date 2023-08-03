﻿using LogisticService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace LogisticService.Data
{
    public interface IApplicationDbContext
    {
        Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class;
        void Create(object entity);
        void SetModified(object entity);
        void Delete(object entity);
        int SaveChanges();
    }
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public void Create(object entity) => Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
        public void SetModified(object entity) => Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        public void Delete(object entity) => Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
        public void SetValue(object currentValue, object newValue) => Entry(currentValue).CurrentValues.SetValues(newValue);

        public DbSet<Email> Emails { get; set; }
        public DbSet<WorkCase> WorkCases { get; set; }
     
    }
    

    
}