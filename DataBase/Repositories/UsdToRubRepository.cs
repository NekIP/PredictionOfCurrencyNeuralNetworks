using DataBase.Entities;
using Microsoft.Extensions.Configuration;

namespace DataBase.Repositories {
	public interface IUsdToRubRepository : IRepository<Product> { }
	public class UsdToRubRepository : Repository<Product>, IUsdToRubRepository {
		public UsdToRubRepository(IConfiguration configuration) : base("usdToRub", configuration) { }

	}
}
