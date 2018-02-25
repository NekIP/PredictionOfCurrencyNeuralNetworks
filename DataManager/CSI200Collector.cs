using DataBase.Repositories;

namespace DataManager {
    public interface ICSI200Collector : IFinamCollector { }
    public class CSI200Collector : FinamCollector, ICSI200Collector {
        public CSI200Collector(ICSI200Repository repository)
            : base(repository, "INDEX.CSI200", 19594, 6) { }
    }
}
