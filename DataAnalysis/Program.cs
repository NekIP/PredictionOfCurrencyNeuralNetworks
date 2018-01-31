using Microsoft.Extensions.Configuration;
using System;

namespace DataAnalysis {
	public static class Program {
		public static IConfiguration Configuration { get; set; }

		public static void Main(string[] args) {
			InitConfiguration();
			Console.ReadKey();
		}

		private static void InitConfiguration() {
			var startup = new Startup();
			Configuration = startup.Run();
		}
	}
}
