using System;
using System.Collections.Generic;
using System.IO;

namespace DataAssistants {
    public interface ILog {
        string OutputFilePath { get; set; }
        bool OutputInConsole { get; set; }
        void Write(string line);
        void Write<T>(T entity);
        void Write<T>(T entity, Func<T, string> converter);
        IList<string> Read();
        string Read(int i);
        IList<T> Read<T>(Func<string, T> converter);
        T Read<T>(int i, Func<string, T> converter);
        void ClearOutputFile();
    }

    public class Log : ILog {
        public string OutputFilePath { get; set; }
        public bool OutputInConsole { get; set; }

        private Serializer Serializer;

        public Log(string outputFilePath, bool outputInConsole = false) {
            OutputFilePath = outputFilePath;
            OutputInConsole = outputInConsole;
            Serializer = new Serializer();
        }

        public void Write(string line) {
            WriteInConsoleIfNeed(line);
            Serializer.AppendToTxt(line, OutputFilePath);
        }

        public void Write<T>(T entity) {
            WriteInConsoleIfNeed(entity.ToString());
            Serializer.AppendToTxt(entity, OutputFilePath);
        }

        public void Write<T>(T entity, Func<T, string> converter) {
            WriteInConsoleIfNeed(converter(entity));
            Serializer.AppendToTxt(entity, OutputFilePath, converter);
         }

        public IList<string> Read() {
            try {
                return Serializer.ReadFromTxt(OutputFilePath).Split('\n');
            }
            catch(FileNotFoundException) {
                return new List<string>();
            }
        }

		public string Read(int i) => Read()[i];

        public IList<T> Read<T>(Func<string, T> converter) {
            try {
                return Serializer.ReadFromTxt(OutputFilePath, converter);
            }
            catch (FileNotFoundException) {
                return new List<T>();
            }
        }

		public T Read<T>(int i, Func<string, T> converter) => Read(converter)[i];

        public void ClearOutputFile() => Serializer.SaveToTxt("", OutputFilePath);

        private void WriteInConsoleIfNeed(string text) {
            if (OutputInConsole) {
                Console.WriteLine($"{ DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") }\t{ text }");
            }
        }
	}
}
