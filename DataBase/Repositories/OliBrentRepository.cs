using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IOliBrentRepository : IRepository<Product> { }
    public class OliBrentRepository : Repository<Product>, IOliBrentRepository {
        public override DbSet<Product> Table() => OliBrent;
        public DbSet<Product> OliBrent { get; set; }
        public OliBrentRepository(IConfiguration configuration) : base(configuration) { }
    }
}
