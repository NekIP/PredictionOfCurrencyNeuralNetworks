using DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public abstract Task DownloadMissingData(DateTime before, TimeSpan step);

        public virtual Task<List<T>> List() =>
            Repository.Table().ToListAsync();

        public virtual Task<List<T>> List(DateTime from, DateTime to, TimeSpan step) {
            var list = Repository.Table()
                .Where(x => x.Date >= from && x.Date < to)
                .OrderBy(x => x.Date)
                .ToList();
            var result = TakeLastProductForEachStep(list, step);
            return Task.FromResult(result);
        }

        public virtual bool TryGet(DateTime date, TimeSpan step, out T result) {
            var list = Repository.Table().Where(x =>
                new TimeSpan(Math.Abs(x.Date.Ticks - date.Ticks)) < step && x.Date.Ticks <= date.Ticks
            ).ToList();
            var entityExistInRepository = list.Count > 0;
            result = entityExistInRepository ? list.First() : null;
            return entityExistInRepository;
        }

        protected List<T> TakeLastProductForEachStep(List<T> productsSorted, TimeSpan step) {
            var result = new List<T>();
            if (productsSorted.Count == 0) {
                return result;
            }
            var lastDate = productsSorted.Last().Date + step;
            for (var i = productsSorted.Count - 1; i >= 0; i--) {
                var entity = productsSorted[i];
                if (entity.Date <= lastDate - step) {
                    lastDate = entity.Date;
                    result.Add(entity);
                }
            }
            return result.OrderBy(x => x.Date).ToList();
        }

        protected List<T> TakeFirstProductForEachStep(List<T> productsSorted, TimeSpan step) {
            var result = new List<T>();
            if (productsSorted.Count == 0) {
                return result;
            }
            var firstDate = productsSorted.First().Date - step;
            for (var i = 0; i < productsSorted.Count; i++) {
                var entity = productsSorted[i];
                if (entity.Date >= firstDate + step) {
                    firstDate = entity.Date;
                    result.Add(entity);
                }
            }
            return result.OrderBy(x => x.Date).ToList();
        }

        protected void DeleteDuplicateEntries(IEqualityComparer<T> comparer) {
            var all = Repository.Table().ToList();
            var unique = all.Distinct(comparer).ToList();
            var forDelete = all.Except(unique).ToList();
            if (forDelete.Count() > 0) {
                Repository.Table().RemoveRange(forDelete);
                Repository.SaveChanges();
            }
        }

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
