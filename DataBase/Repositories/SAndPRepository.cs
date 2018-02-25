using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface ISAndPRepository : IRepository<Product> { }
    public class SAndPRepository : Repository<Product>, ISAndPRepository {
        public override DbSet<Product> Table() => SAndP;
        public DbSet<Product> SAndP { get; set; }
        public SAndPRepository(IConfiguration configuration) : base(configuration) { }
    }
}
