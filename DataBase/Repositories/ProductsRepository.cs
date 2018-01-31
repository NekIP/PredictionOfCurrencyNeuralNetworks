using DataBase.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories {
	public interface IProductsRepository : IRepository<Product> {
		Task<List<Product>> GetMostPopular(int count);
	}

	public class ProductsRepository : Repository<Product>, IProductsRepository {
		public ProductsRepository(IConfiguration configuration) : base("products", configuration) { }

		public Task<List<Product>> GetMostPopular(int count) {
			return this.SortBy(x => x.Rating, SortDirection.Descending).Take(count).Execute();
		}
	}
}
