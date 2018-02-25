using DataBase.Repositories;

namespace DataManager {
    public interface ISAndP500Collector : IFinamCollector { }
    public class SAndP500Collector : FinamCollector, ISAndP500Collector {
        public SAndP500Collector(ISAndPRepository repository)
            : base(repository, "SANDP-500", 90, 6) { }
    }
}
