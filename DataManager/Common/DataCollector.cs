using DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
    public interface IDataCollector {
        DateTime GlobalFrom { get; set; }
        string Source { get; }
        Task<List<Entity>> List();
        Task<List<Entity>> List(DateTime from, DateTime to, TimeSpan step);
        Task Add(Entity entity);
        Task Add(DateTime date, double value);
        Task Remove(long id);
        Task Update(long id, double value);
        bool TryGet(DateTime date, TimeSpan step, out Entity result);
        Task DownloadMissingData(DateTime before, TimeSpan step);
        Entity GetNearLeft(DateTime date);
    }

    public interface IDataCollector<T> : IDataCollector where T : Entity, new() {
        IRepository<T> Repository { get; }
    }

	public abstract class DataCollector<T> : IDataCollector<T> where T : Entity, new() {
        public DateTime GlobalFrom { get; set; } = new DateTime(2007, 10, 8);
		public string Source { get; protected set; }
		public IRepository<T> Repository { get; protected set; }

        protected Cache Cacher { get; set; }

        public DataCollector(IRepository<T> repository, bool isCache = false) {
            Repository = repository;
            Cacher = new Cache(isCache, repository);
        }

        public DataCollector(string source, IRepository<T> repository, bool isCache = false) {
			Source = source;
			Repository = repository;
            Cacher = new Cache(isCache, repository);
        }

        public virtual Entity GetNearLeft(DateTime date) {
            var minDeff = Repository.Table()
                .Where(x => x.Date < date)
                .Min(x => date.Ticks - x.Date.Ticks);
            return Repository.Table().Where(x => date.Ticks - x.Date.Ticks == minDeff).FirstOrDefault();
        }

        public abstract Task DownloadMissingData(DateTime before, TimeSpan step);

        public virtual async Task<List<Entity>> List() {
            if (Cacher.Need) {
                Cacher.Initialize();
                return Cacher.Entries.Select(x => (Entity)x).ToList();
            }
            return await Repository.Table().Select(x => (Entity)x).ToListAsync();
        }

        public virtual Task<List<Entity>> List(DateTime from, DateTime to, TimeSpan step) {
            var list = new List<Entity>();
            if (Cacher.Need) {
                Cacher.Initialize();
                list = Cacher.Entries
                    .Where(x => x.Date >= from && x.Date < to)
                    .OrderBy(x => x.Date)
                    .Select(x => (Entity)x)
                    .ToList();
            }
            else {
                list = Repository.Table()
                    .Where(x => x.Date >= from && x.Date < to)
                    .OrderBy(x => x.Date)
                    .Select(x => (Entity)x)
                    .ToList();
            }
            var result = TakeLastProductForEachStep(list, step);
            return Task.FromResult(result);
        }

        public virtual bool TryGet(DateTime date, TimeSpan step, out Entity result) {
            if (Cacher.Need) {
                Cacher.Initialize();
                result = Cacher.Entries.Where(x =>
                    x.Date.Ticks <= date.Ticks && Math.Abs(x.Date.Ticks - date.Ticks) < step.Ticks
                ).FirstOrDefault();
                return !(result is null);
            }
            else {
                var list = Repository.Table().Where(x =>
                    x.Date.Ticks <= date.Ticks && Math.Abs(x.Date.Ticks - date.Ticks) < step.Ticks
                ).ToList();
                var entityExistInRepository = list.Count > 0;
                result = null;
                if (entityExistInRepository) {
                    result = list.Count == 1 ? list[0] : list.OrderBy(x => x.Date).Last();
                }
                return entityExistInRepository;
            }
        }

        public async Task Add(Entity entity) {
            await Repository.Table().AddAsync((T)entity);
            Repository.SaveChanges();
            DeleteDuplicateEntries();
            Cacher.Update((T)entity);
        }

        public Task Add(DateTime date, double value) {
            var result = new T {
                Date = date
            };
            result.Setter(value);
            return Add(result);
        }

        public Task Remove(long id) {
            Cacher.Clear();
            var entityForDelete = EntityById(id);
            Repository.Table().Remove(entityForDelete);
            return Repository.SaveChangesAsync();
        }

        public Task Update(long id, double value) {
            Cacher.Clear();
            var entityForChange = EntityById(id);
            entityForChange.Setter(value);
            return Repository.SaveChangesAsync();
        }

        protected T EntityById(long id) {
            var entity = Repository.Table().Where(x => x.Id == id).FirstOrDefault();
            if (entity is null) {
                throw new Exception("Entity is not exist");
            }
            return entity;
        }

        protected List<Entity> TakeLastProductForEachStep(List<Entity> productsSorted, TimeSpan step) {
            var result = new List<Entity>();
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

        protected List<Entity> TakeFirstProductForEachStep(List<Entity> productsSorted, TimeSpan step) {
            var result = new List<Entity>();
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

        protected void DeleteDuplicateEntries() {
            var all = Repository.Table().ToList();
            var unique = all.Distinct(new Comparer()).ToList();
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

        protected class Comparer : IEqualityComparer<T> {
            public bool Equals(T x, T y) => x.Date == y.Date;
            public int GetHashCode(T obj) => obj.Date.GetHashCode();
        }

        protected class Cache {
            public IRepository<T> Repository { get; set; }
            public List<T> Entries { get; set; } = new List<T>();
            public DateTime? LeftDate { get; set; } = null;
            public DateTime? RightDate { get; set; } = null;
            public bool WasInit { get; set; } = false;
            public bool Need { get; set; }

            public Cache(bool need, IRepository<T> repository) {
                Need = need;
                Repository = repository;
            }

            public void Clear() {
                Entries = new List<T>();
                LeftDate = null;
                RightDate = null;
                WasInit = false;
            }

            public void Initialize() {
                if (!WasInit) {
                    Entries = Repository.Table().OrderBy(x => x.Date).ToList();
                    LeftDate = Entries.FirstOrDefault()?.Date;
                    RightDate = Entries.LastOrDefault()?.Date;
                    WasInit = true;
                }
            }

            public void Update(T entity) {
                if (Need && WasInit) {
                    Entries.Add(entity);
                }
            }
        }
    }
}
