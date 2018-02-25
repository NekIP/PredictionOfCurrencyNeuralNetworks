using DataBase.Repositories;

namespace DataManager {
    public interface IGoldCollector : IFinamCollector { }
    public class GoldCollector : FinamCollector, IGoldCollector {
        public GoldCollector(IGoldRepository repository)
            : base(repository, "comex.GC", 18953, 24) { }
    }
}
