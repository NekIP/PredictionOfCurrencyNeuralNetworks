using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IGoldRepository : IRepository<Product> { }
    public class GoldRepository : Repository<Product>, IGoldRepository {
        public override DbSet<Product> Table() => Gold;
        public DbSet<Product> Gold { get; set; }
        public GoldRepository(IConfiguration configuration) : base(configuration) { }
    }
}
