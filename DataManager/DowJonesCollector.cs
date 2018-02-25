using DataBase.Repositories;

namespace DataManager {
    public interface IDowJonesCollector : IFinamCollector { }
    public class DowJonesCollector : FinamCollector, IDowJonesCollector {
        public DowJonesCollector(IDowJonesRepository repository)
            : base(repository, "D&J-IND", 91, 6) { }
    }
}
