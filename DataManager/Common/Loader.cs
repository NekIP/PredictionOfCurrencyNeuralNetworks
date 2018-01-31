using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DataManager {
	public class Loader {
		public string Url { get; set; }

		public Loader(string url) {
			Url = url;
		}

		public Task<string> Get(params KeyValuePair<string, string>[] parameters) =>
			RequestTemplate(parameters, "GET");

		public Task<string> Post(params KeyValuePair<string, string>[] parameters) =>
			RequestTemplate(parameters, "POST");

		public async Task<T> Get<T>(Func<string, T> converter, params KeyValuePair<string, string>[] parameters) =>
			converter(await Get(parameters));

		public async Task<T> Post<T>(Func<string, T> converter, params KeyValuePair<string, string>[] parameters) =>
			converter(await Post(parameters));

		private Task<string> RequestTemplate(KeyValuePair<string, string>[] parameters, string method) {
			var client = new WebClient();
			var urlParams = GetUrlParameters(parameters);
			return client.UploadStringTaskAsync(Url, method, urlParams);
		}

		private string GetUrlParameters(KeyValuePair<string, string>[] parameters) =>
			parameters.Length > 0
				? $"?{ String.Join('&', parameters.Select(x => $"{ x.Key }={ x.Value }")) }"
				: string.Empty;
	}
}
