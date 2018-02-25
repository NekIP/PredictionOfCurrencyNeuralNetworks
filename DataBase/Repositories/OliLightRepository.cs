using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IOliLightRepository : IRepository<Product> { }
    public class OliLightRepository : Repository<Product>, IOliLightRepository {
        public override DbSet<Product> Table() => OliLight;
        public DbSet<Product> OliLight { get; set; }
        public OliLightRepository(IConfiguration configuration) : base(configuration) { }
    }
}
