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
			Source = "";
			Repository = new UsdToRubCurrencyRepository(configuration);
		}

		public override Task<Product[]> Get() {
			throw new NotImplementedException();
		}

		public override Task<Product> Get(DateTime date) {
			throw new NotImplementedException();
		}

		public override Task<Product[]> Get(DateTime from, DateTime to) {
			throw new NotImplementedException();
		}
	}
}
