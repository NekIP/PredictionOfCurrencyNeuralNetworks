using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IRTSRepository : IRepository<Product> { }
    public class RTSRepository : Repository<Product>, IRTSRepository {
        public override DbSet<Product> Table() => RTS;
        public DbSet<Product> RTS { get; set; }
        public RTSRepository(IConfiguration configuration) : base(configuration) { }
    }
}
