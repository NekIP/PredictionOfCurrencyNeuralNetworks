using System.Threading.Tasks;

namespace DataManager {
	public abstract class DataCollector<T> {
		public T[] Data { get; protected set; }

		public abstract Task<T[]> GetFromSource();
		public abstract Task<T[]> GetFromDb();
		public abstract Task SaveToDb();
		public abstract Task LoadFromDb();
		public abstract Task UpdateFromSource();
	}
}
