using DataBase.Repositories;

namespace DataManager {
    public interface IRTSCollector : IFinamCollector { }
    public class RTSCollector : FinamCollector, IRTSCollector {
        public RTSCollector(IRTSRepository repository)
            : base(repository, "RTSI", 95, 6) { }
    }
}
