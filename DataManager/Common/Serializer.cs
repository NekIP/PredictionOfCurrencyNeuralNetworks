using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataManager {
    public interface ISerializer {
        Task AppendToTxt(string line, string path);
        Task AppendToTxt<T>(T entity, string path, Func<T, string> converter);
        Task AppendToTxt<T>(T entity, string path);
        Task SaveToTxt(string text, string path);
        Task SaveToTxt<T>(IEnumerable<T> array, string path, Func<T, string> converter);
        Task SaveToTxt<T>(IEnumerable<T> array, string path);
        Task<string> ReadFromTxt(string path);
        Task<IEnumerable<T>> ReadFromTxt<T>(string path, Func<string, T> converter);
        Task Serialize(object entity, string path);
        T Deserialize<T>(string path);
    }

	public class Serializer : ISerializer {
		public async Task AppendToTxt(string line, string path) {
			using (var stream = new StreamWriter(File.Open(path, FileMode.Append))) {
				await stream.WriteLineAsync(line);
			}
		}

		public async Task AppendToTxt<T>(T entity, string path, Func<T, string> converter) {
			await AppendToTxt(converter(entity), path);
		}

		public async Task AppendToTxt<T>(T entity, string path) {
			await AppendToTxt(entity, path, x => x.ToString());
		}

		public async Task SaveToTxt(string text, string path) {
			using (var stream = new StreamWriter(File.Open(path, FileMode.OpenOrCreate))) {
				await stream.WriteAsync(text);
			}
		}

		public async Task SaveToTxt<T>(IEnumerable<T> array, string path, Func<T, string> converter) {
			var text = String.Empty;
			foreach (var item in array) {
				text += converter(item) + "\n";
			}
			await SaveToTxt(text, path);
		}

		public async Task SaveToTxt<T>(IEnumerable<T> array, string path) {
			await SaveToTxt(array, path, x => x.ToString());
		}

		public Task<string> ReadFromTxt(string path) {
			using (var stream = new StreamReader(File.Open(path, FileMode.Open))) {
				return stream.ReadToEndAsync();
			}
		}

		public async Task<IEnumerable<T>> ReadFromTxt<T>(string path, Func<string, T> converter) {
			var text = await ReadFromTxt(path);
			var lines = text.Split('\n');
			var result = new List<T>();
			foreach (var line in lines) {
				result.Add(converter(line));
			}
			return result;
		}

		public Task Serialize(object entity, string path) {
            var json = JsonConvert.SerializeObject(entity);
			using (var stream = new StreamWriter(File.Open(path, FileMode.OpenOrCreate))) {
                return stream.WriteAsync(json);
            }
		}

		public T Deserialize<T>(string path) {
			using (var stream = new StreamReader(File.Open(path, FileMode.Open))) {
                var resultText = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(resultText);
            }
		}
	}
}
