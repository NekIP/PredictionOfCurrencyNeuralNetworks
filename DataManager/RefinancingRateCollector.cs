using DataBase.Repositories;

namespace DataManager {
    public interface IRefinancingRateCollector : IEconomicIndicatorCollector { }
    public class RefinancingRateCollector : EconomicIndicatorCollector, IRefinancingRateCollector {
        public RefinancingRateCollector(IRefinancingRateRepository repository) : 
            base("Data/refinancing-rate.csv", repository) { }
    }
}
