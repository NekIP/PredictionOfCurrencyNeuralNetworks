using DataBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataManager {
    public interface IDataCollector<T> where T : Entity, new() {
        DateTime GlobalFrom { get; set; }
        string Source { get; }
        IRepository<T> Repository { get; }
        Task<List<T>> List();
        Task<List<T>> List(DateTime from, DateTime to, TimeSpan step);
        bool TryGet(DateTime date, TimeSpan step, out T result);
        Task DownloadMissingData(DateTime before, TimeSpan step);
    }

	public abstract class DataCollector<T> : IDataCollector<T> where T : Entity, new() {
        public DateTime GlobalFrom { get; set; } = new DateTime(2007, 10, 8);
		public string Source { get; protected set; }
		public IRepository<T> Repository { get; protected set; }

		public DataCollector(string source, IRepository<T> repository) {
			Source = source;
			Repository = repository;
		}

		public abstract Task<List<T>> List();
		public abstract Task<List<T>> List(DateTime from, DateTime to, TimeSpan step);
		public abstract bool TryGet(DateTime date, TimeSpan step, out T result);
		public abstract Task DownloadMissingData(DateTime before, TimeSpan step);

		protected void CheckConditionOnException<TException>(bool conditionFunc, string message)
			where TException : Exception, new() {
			if (conditionFunc) {
				throw new DataCollectorException(message, new TException());
			}
		}

		protected void CheckConditionOnException(bool conditionFunc, string message) {
			if (conditionFunc) {
				throw new DataCollectorException(message);
			}
		}
	}
}
