using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IMMVBRepository : IRepository<Product> { }
    public class MMVBRepository : Repository<Product>, IMMVBRepository {
        public override DbSet<Product> Table() => MMVB;
        public DbSet<Product> MMVB { get; set; }
        public MMVBRepository(IConfiguration configuration) : base(configuration) { }
    }
}
