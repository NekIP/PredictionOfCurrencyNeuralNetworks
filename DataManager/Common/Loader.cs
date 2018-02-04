using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataManager {
	public class Loader {
        public Task<string> Get(string url, params KeyValuePair<string, string>[] parameters) {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var urlParams = GetUrlParameters(parameters);
            return client.DownloadStringTaskAsync(url + urlParams);
        }

		public Task<string> Post(string url, params KeyValuePair<string, string>[] parameters) {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var urlParams = GetUrlParameters(parameters);
            return client.UploadStringTaskAsync(url, urlParams);
        }

        public async Task<T> Get<T>(string url, Func<string, T> converter, params KeyValuePair<string, string>[] parameters) =>
			converter(await Get(url, parameters));

		public async Task<T> Post<T>(string url, Func<string, T> converter, params KeyValuePair<string, string>[] parameters) =>
			converter(await Post(url, parameters));

		private string GetUrlParameters(KeyValuePair<string, string>[] parameters) =>
			parameters.Length > 0
				? $"?{ String.Join('&', parameters.Select(x => $"{ x.Key }={ x.Value }")) }"
				: string.Empty;
	}
}
