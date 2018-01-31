using DataBase;
using System;
using System.Threading.Tasks;

namespace DataManager {
	public abstract class DataCollector<T> where T : Entity, new() {
		public string Source { get; protected set; }
		public IRepository<T> Repository { get; protected set; }

		public abstract Task<T[]> Get();
		public abstract Task<T[]> Get(DateTime date);
		public abstract Task<T[]> Get(DateTime from, DateTime to);
	}
}
