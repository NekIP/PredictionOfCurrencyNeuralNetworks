using DataBase.Repositories;

namespace DataManager {
    public interface IUsdToRubCurrencyCollector : IFinamCollector { }

    public class UsdToRubCurrencyCollector : FinamCollector, IUsdToRubCurrencyCollector {
		public UsdToRubCurrencyCollector(IUsdToRubCurrencyRepository repository) 
			: base("http://export.finam.ru/", repository, "USDRUB", 901, 5) { }
	}
}
