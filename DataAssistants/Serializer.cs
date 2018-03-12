using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAssistants {
    public interface ISerializer {
        void AppendToTxt(string line, string path);
        void AppendToTxt<T>(T entity, string path, Func<T, string> converter);
        void AppendToTxt<T>(T entity, string path);
        void SaveToTxt(string text, string path);
        void SaveToTxt<T>(IEnumerable<T> array, string path, Func<T, string> converter);
        void SaveToTxt<T>(IEnumerable<T> array, string path);
        string ReadFromTxt(string path);
        IList<T> ReadFromTxt<T>(string path, Func<string, T> converter);
        void Serialize(object entity, string path);
        T Deserialize<T>(string path);
        bool Exists(string path);
    }

	public class Serializer : ISerializer {
        public bool Exists(string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            return File.Exists(path);
        }

        public void AppendToTxt(string line, string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            ProvideFolders(path);
            using (var stream = new StreamWriter(File.Open(path, FileMode.Append))) {
			    stream.WriteLine(line);
			}
		}

		public void AppendToTxt<T>(T entity, string path, Func<T, string> converter) {
			AppendToTxt(converter(entity), path);
		}

		public void AppendToTxt<T>(T entity, string path) {
			AppendToTxt(entity, path, x => x.ToString());
		}

		public void SaveToTxt(string text, string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            ProvideFolders(path);
            using (var stream = new StreamWriter(File.Open(path, FileMode.OpenOrCreate))) {
				stream.Write(text);
			}
		}

		public void SaveToTxt<T>(IEnumerable<T> array, string path, Func<T, string> converter) {
            var text = String.Empty;
			foreach (var item in array) {
				text += converter(item) + "\n";
			}
			SaveToTxt(text, path);
		}

		public void SaveToTxt<T>(IEnumerable<T> array, string path) {
			SaveToTxt(array, path, x => x.ToString());
		}

		public string ReadFromTxt(string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            using (var stream = new StreamReader(File.Open(path, FileMode.Open))) {
				return stream.ReadToEnd();
			}
		}

		public IList<T> ReadFromTxt<T>(string path, Func<string, T> converter) {
			var text = ReadFromTxt(path);
            if (string.IsNullOrWhiteSpace(text)) {
                return new List<T>();
            }
			var lines = text.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));
			var result = new List<T>();
			foreach (var line in lines) {
				result.Add(converter(line));
			}
			return result;
		}

		public void Serialize(object entity, string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            ProvideFolders(path);
            var json = JsonConvert.SerializeObject(entity);
            if (File.Exists(path)) {
                File.Delete(path);
            }
			using (var stream = new StreamWriter(File.Open(path, FileMode.OpenOrCreate))) {
                stream.Write(json);
            }
		}

		public T Deserialize<T>(string path) {
#if DEBUG
            path = Path.Combine("bin\\Debug\\netcoreapp2.0\\", path);
#endif
            using (var stream = new StreamReader(File.Open(path, FileMode.Open))) {
                var resultText = stream.ReadToEnd();
                var result = JsonConvert.DeserializeObject<T>(resultText);
                return result;
            }
		}

        protected void ProvideFolders(string path) {
            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory)) {
                return;
            }
            var currentDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(directory)) {
                return;
            }
            var folders = directory.Split('\\');
            var currentPath = "";
            for (var i = 0; i < folders.Length; i++) {
                currentPath = Path.Combine(currentPath, folders[i]);
                if (!Directory.Exists(Path.Combine(currentDirectory, currentPath))) {
                    Directory.CreateDirectory(Path.Combine(currentDirectory, currentPath));
                }
            }
        }
	}
}
