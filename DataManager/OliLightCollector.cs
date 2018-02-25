using DataBase.Repositories;

namespace DataManager {
    public interface IOliLightCollector : IFinamCollector { }
    public class OliLightCollector : FinamCollector, IOliLightCollector {
        public OliLightCollector(IOliLightRepository repository)
            : base(repository, "NYMEX.CL", 18948, 24) { }
    }
}
