using DataBase.Repositories;
using Microsoft.Extensions.Configuration;

namespace DataManager {
	public class UsdToRubCurrencyCollector : FinamCollector {
		public UsdToRubCurrencyCollector(IConfiguration configuration) 
			: base("http://export.finam.ru/", new UsdToRubCurrencyRepository(configuration), "USDRUB", 901, 5) { }
	}
}
