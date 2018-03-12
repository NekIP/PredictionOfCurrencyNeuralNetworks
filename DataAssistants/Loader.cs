using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAssistants {
    public interface ILoader {
        Task<string> Get(string url, params KeyValuePair<string, string>[] parameters);
        Task<string> Post(string url, params KeyValuePair<string, string>[] parameters);
        Task<T> Get<T>(string url, Func<string, T> converter, params KeyValuePair<string, string>[] parameters);
        Task<T> Post<T>(string url, Func<string, T> converter, params KeyValuePair<string, string>[] parameters);
    }

	public class Loader : ILoader {
        public async Task<string> Get(string url, params KeyValuePair<string, string>[] parameters) {
            var urlParams = GetUrlParameters(parameters);
            var result = await TryGet(url + urlParams);
            if (result is null) {
                var ind = 0;
                while (result == null && ind < 10) {
                    ind++;
                    Console.WriteLine($"Conection failed! Attempt: { ind }");
                    result = await TryGet(url + urlParams);
                    Thread.Sleep(1000);
                }
                if (ind >= 10) {
                    Console.WriteLine($"Conection failed! Application was stoped!");
                    throw new WebException();
                }
                return result;
            }
            return result;
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

        private async Task<string> TryGet(string url) {
            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            try {
                var result = await client.DownloadStringTaskAsync(url);
                return result;
            }
            catch (WebException ex) {
                return null;
            }
        }

		private string GetUrlParameters(KeyValuePair<string, string>[] parameters) =>
			parameters.Length > 0
				? $"?{ String.Join('&', parameters.Select(x => $"{ x.Key }={ x.Value }")) }"
				: string.Empty;
	}
}
