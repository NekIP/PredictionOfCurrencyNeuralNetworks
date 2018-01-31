using Microsoft.Extensions.Configuration;
using System.IO;

namespace DataAnalysis {
	public class Startup {
		public IConfiguration Run() {
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			return builder.Build();
		}
	}
}
