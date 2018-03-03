using DataBase.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
    public interface ITradeBalanceRepository : IRepository<EconomicIndicator> { }
    public class TradeBalanceRepository : Repository<EconomicIndicator>, ITradeBalanceRepository {
        public override DbSet<EconomicIndicator> Table() => TradeBalance;
        public DbSet<EconomicIndicator> TradeBalance { get; set; }
        public TradeBalanceRepository(IConfiguration configuration) : base(configuration) { }
    }
}
