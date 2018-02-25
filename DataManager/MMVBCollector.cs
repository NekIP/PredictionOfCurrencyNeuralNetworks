using DataBase.Repositories;

namespace DataManager {
    public interface IMMVBCollector : IFinamCollector { }
    public class MMVBCollector : FinamCollector, IMMVBCollector {
        public MMVBCollector(IMMVBRepository repository)
            : base(repository, "MICEXINDEXCF", 13851, 6) { }
    }
}
