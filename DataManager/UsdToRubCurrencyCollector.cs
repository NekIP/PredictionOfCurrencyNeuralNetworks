using DataBase.Entities;
using DataBase.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataManager {
	public class UsdToRubCurrencyCollector : DataCollector<Product> {
		public UsdToRubCurrencyCollector(IConfiguration configuration) {
			Source = "http://export.finam.ru/";
			Repository = new UsdToRubCurrencyRepository(configuration);
		}

		public override Task<List<Product>> Get() {
			Repository.List();
		}

		public override Task<Product> Get(DateTime date) {
			throw new NotImplementedException();
		}

		public override Task<List<Product>> Get(DateTime from, DateTime to) {
			throw new NotImplementedException();
		}
	}
}
