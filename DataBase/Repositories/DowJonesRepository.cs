using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IDowJonesRepository : IRepository<Product> { }
    public class DowJonesRepository : Repository<Product>, IDowJonesRepository {
        public override DbSet<Product> Table() => DowJones;
        public DbSet<Product> DowJones { get; set; }
        public DowJonesRepository(IConfiguration configuration) : base(configuration) { }
    }
}
