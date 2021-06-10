using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Architecture
{
	public static class WebClient
	{
		public static async Task<string> GetAsync(string url)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

			using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
			using (Stream stream = response.GetResponseStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				return await reader.ReadToEndAsync();
			}
		}

		public static async Task<string> GetHttpClientAsync(string url)
		{
			var response = await new HttpClient().GetAsync("http://michalbialeckicomnetcoreweb20180417060938.azurewebsites.net/api/users/")
				.ConfigureAwait(false);
			var result = await response.Content.ReadAsStringAsync();

			return result;
		}
	}
}
