using DataBase.Repositories;

namespace DataManager {
    public interface IUsdToRubCurrencyCollector : IFinamCollector { }

    public class UsdToRubCurrencyCollector : FinamCollector, IUsdToRubCurrencyCollector {
		public UsdToRubCurrencyCollector(IUsdToRubCurrencyRepository repository) 
			: base(repository, "USDRUB", 901, 5) { }
	}
}
