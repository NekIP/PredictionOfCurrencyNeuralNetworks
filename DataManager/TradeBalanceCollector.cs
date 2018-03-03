using DataBase.Repositories;

namespace DataManager {
    public interface ITradeBalanceCollector : IEconomicIndicatorCollector { }
    public class TradeBalanceCollector : EconomicIndicatorCollector, ITradeBalanceCollector {
        public TradeBalanceCollector(ITradeBalanceRepository repository) :
            base("Data/trade-balance.csv", repository) { }
    }
}
