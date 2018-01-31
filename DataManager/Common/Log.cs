using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManager {
	public class Log {
		public string OutputFilePath { get; set; }

		private Serializer Serializer;

		public Log(string outputFilePath) {
			OutputFilePath = outputFilePath;
			Serializer = new Serializer();
		}

		public Task Write(string line) => 
			Serializer.AppendToTxt(line, OutputFilePath);

		public Task Write<T>(T entity) => 
			Serializer.AppendToTxt(entity, OutputFilePath);

		public Task Write<T>(T entity, Func<T, string> converter) => 
			Serializer.AppendToTxt(entity, OutputFilePath, converter);

		public async Task<IEnumerable<string>> Read() =>
			(await Serializer.ReadFromTxt(OutputFilePath)).Split('\n');

		public async Task<string> Read(int i) =>
			(await Serializer.ReadFromTxt(OutputFilePath)).Split('\n')[i];

		public async Task<IEnumerable<T>> Read<T>(Func<string, T> converter) =>
			await Serializer.ReadFromTxt(OutputFilePath, converter);

		public async Task<T> Read<T>(int i, Func<string, T> converter) =>
			(await Serializer.ReadFromTxt(OutputFilePath, converter)).ElementAt(i);
	}
}
