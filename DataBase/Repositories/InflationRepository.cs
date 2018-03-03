using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IInflationRepository : IRepository<EconomicIndicator> { }
    public class InflationRepository : Repository<EconomicIndicator>, IInflationRepository {
        public override DbSet<EconomicIndicator> Table() => Inflation;
        public DbSet<EconomicIndicator> Inflation { get; set; }
        public InflationRepository(IConfiguration configuration) : base(configuration) { }
    }
}
