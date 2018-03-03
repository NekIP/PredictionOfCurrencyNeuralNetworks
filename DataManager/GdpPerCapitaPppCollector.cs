using DataBase.Repositories;

namespace DataManager {
    public interface IGdpPerCapitaPppCollector : IEconomicIndicatorCollector { }
    public class GdpPerCapitaPppCollector : EconomicIndicatorCollector, IGdpPerCapitaPppCollector {
        public GdpPerCapitaPppCollector(IGdpPerCapitaPppRepository repository) : 
            base("Data/gdp-per-capita-based-on-ppp.csv", repository) { }
    }
}
