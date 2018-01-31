using DataBase.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase.Repositories {
	public interface IProductsRepository : IRepository<Product> {

	}

	public class ProductsRepository : Repository<Product>, IProductsRepository {
		public ProductsRepository(IConfiguration configuration) : base("products", configuration) { }

	}
}
