using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataBase {
    public interface IRepository<T> : IDisposable where T: Entity {
        DbSet<T> Table();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        DatabaseFacade Database { get; }
    }

    public class Repository<T> : DbContext, IRepository<T> where T : Entity {
        public virtual DbSet<T> Table() { return null; }

        private IConfiguration Configuration { get; set; }

        public Repository(IConfiguration configuration) {
            Configuration = configuration;
            Database.EnsureCreated();
            SaveChanges();
        }

        public Repository(DbContextOptions<Repository<T>> options) : base(options) {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(Configuration["connectionStrings:deffaultConnection"]);
        }
    }
}
