using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface IRefinancingRateRepository : IRepository<EconomicIndicator> { }
    public class RefinancingRateRepository : Repository<EconomicIndicator>, IRefinancingRateRepository {
        public override DbSet<EconomicIndicator> Table() => RefinancingRate;
        public DbSet<EconomicIndicator> RefinancingRate { get; set; }
        public RefinancingRateRepository(IConfiguration configuration) : base(configuration) { }
    }
}
