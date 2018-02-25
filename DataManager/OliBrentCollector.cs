using DataBase.Repositories;

namespace DataManager {
    public interface IOliBrentCollector : IFinamCollector { }
    public class OliBrentCollector : FinamCollector, IOliBrentCollector {
        public OliBrentCollector(IOliBrentRepository repository)
            : base(repository, "ICE.BRN", 19473, 24) { }
    }
}
