using DataBase.Entities;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
	public interface IUsdToRubCurrencyRepository : IRepository<Product> { }
	public class UsdToRubCurrencyRepository : Repository<Product>, IUsdToRubCurrencyRepository {
		public UsdToRubCurrencyRepository(IConfiguration configuration) : base("usdToRub", configuration) { }
	}
}
