using System;

namespace DataManager {
	public class DataCollectorException : Exception {
		public DataCollectorException() { }
		public DataCollectorException(string message) : base(message) { }
		public DataCollectorException(string message, Exception innerException) : base(message, innerException) { }
	}
}
