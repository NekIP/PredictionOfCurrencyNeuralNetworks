using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataBase {
	public interface IRepository<T> where T : Entity, new() {
		IRepository<T> Where(Expression<Func<T, bool>> filter);
		Task<List<T>> List();
		Task<List<T>> Execute();
		IRepository<T> SortBy(Expression<Func<T, object>> selector, SortDirection sortDirection = SortDirection.Ascending);
		IRepository<T> Take(int count);
		IRepository<T> Skip(int count);
		Task Add(T item);
		Task AddRange(ICollection<T> items);
		Task Remove(Expression<Func<T, bool>> filter);
		Task RemoveRange(Expression<Func<T, bool>> filter);
		Task Update(Expression<Func<T, bool>> filter, T item);
		Task UpdateMany(Expression<Func<T, bool>> filter, T items);
	}

	public enum SortDirection {
		Ascending,
		Descending
	}

	public class Repository<T> : IRepository<T> where T : Entity, new() {
		protected IMongoCollection<T> Collection { get; set; }

		private IFindFluent<T, T> CurrentContext { get; set; } = null;

		public Repository(string collectionName, IConfiguration configuration) {
			var client = new MongoClient(configuration["connectionStrings:deffaultConnection"]);
			var database = client.GetDatabase(configuration["connectionStrings:deffaultDatabase"]);
			Collection = database.GetCollection<T>(collectionName);
		}

		public long Count {
			get {
				return Collection.Count(new BsonDocument());
			}
		}

		public T this[ObjectId i] {
			get {
				return Collection.Find(x => x.Id == i).FirstOrDefault();
			}
			set {
				Collection.UpdateOne(x => x.Id == i, value.ToBsonDocument());
			}
		}

		public IRepository<T> Where(Expression<Func<T, bool>> filter) {
			if (CurrentContext == null) {
				CurrentContext = Collection.Find(filter);
			}
			else {
				CurrentContext.Filter = filter;
			}
			return this;
		}

		public Task<List<T>> List() {
			return Collection.Find(new BsonDocument()).ToListAsync();
		}

		public Task<List<T>> Execute() {
			if (CurrentContext != null) {
				var result = CurrentContext.ToListAsync();
				CurrentContext = null;
				return result;
			}
			return null;
		}

		public IRepository<T> SortBy(Expression<Func<T, object>> selector, SortDirection sortDirection = SortDirection.Ascending) {
			if (CurrentContext == null) {
				CurrentContext = sortDirection == SortDirection.Ascending
					? Collection.Find(new BsonDocument()).SortBy(selector)
					: Collection.Find(new BsonDocument()).SortByDescending(selector);
			}
			else {
				CurrentContext = sortDirection == SortDirection.Ascending
					? CurrentContext.SortBy(selector)
					: CurrentContext.SortByDescending(selector);
			}
			return this;
		}

		public IRepository<T> Take(int count) {
			if (CurrentContext == null) {
				CurrentContext = Collection.Find(new BsonDocument()).Limit(count);
			}
			else {
				CurrentContext = CurrentContext.Limit(count);
			}
			return this;
		}

		public IRepository<T> Skip(int count) {
			if (CurrentContext == null) {
				CurrentContext = Collection.Find(new BsonDocument()).Skip(count);
			}
			else {
				CurrentContext = CurrentContext.Skip(count);
			}
			return this;
		}

		public Task Add(T item) {
			return Collection.InsertOneAsync(item);
		}

		public Task AddRange(ICollection<T> items) {
			return Collection.InsertManyAsync(items);
		}

		public Task Remove(Expression<Func<T, bool>> filter) {
			return Collection.DeleteOneAsync(filter);
		}

		public Task RemoveRange(Expression<Func<T, bool>> filter) {
			return Collection.DeleteManyAsync(filter);
		}

		public Task Update(Expression<Func<T, bool>> filter, T item) {
			return Collection.UpdateOneAsync(filter, item.ToBsonDocument());
		}

		public Task UpdateMany(Expression<Func<T, bool>> filter, T items) {
			return Collection.UpdateManyAsync(filter, items.ToBsonDocument());
		}
	}
}
