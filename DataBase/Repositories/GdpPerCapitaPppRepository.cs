using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IGdpPerCapitaPppRepository : IRepository<EconomicIndicator> { }
    public class GdpPerCapitaPppRepository : Repository<EconomicIndicator>, IGdpPerCapitaPppRepository {
        public override DbSet<EconomicIndicator> Table() => GdpPerCapitaPpp;
        public DbSet<EconomicIndicator> GdpPerCapitaPpp { get; set; }
        public GdpPerCapitaPppRepository(IConfiguration configuration) : base(configuration) { }
    }
}
