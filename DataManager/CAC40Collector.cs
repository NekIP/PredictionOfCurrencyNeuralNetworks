using DataBase.Repositories;

namespace DataManager {
    public interface ICAC40Collector : IFinamCollector { }
    public class CAC40Collector : FinamCollector, ICAC40Collector {
        public CAC40Collector(ICAC40Repository repository)
            : base(repository, "CAC40", 112, 6) { }
    }
}
